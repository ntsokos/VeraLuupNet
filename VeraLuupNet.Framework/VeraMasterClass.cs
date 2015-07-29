using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Threading;
using System.Web.Script.Serialization;
using VeraLuupNet.Framework.Enums;
using VeraLuupNet.Framework.Interfaces;
using VeraLuupNet.Framework.JsonModels;

namespace VeraLuupNet.Framework
{
    public class VeraMasterClass
    {



        public const string AUTHASERVER = "us-autha.mios.com"; // works ok 

        #region [ consts ]

        #endregion

        #region [ properties ]

        private string Username = "";
        private string SHA1Password = "";

        private ISessionInterface Session { get; set; }
        private Action<MessageTypeEnum, string> MessageCallBack { get; set; }

        public bool IsInitialized { get; set; }

        // this comes from Session if needed
        private string RelaySessionToken { get; set; }
        private string ServerRelay { get; set; }
        private string PK_device { get; set; }
        private string Server_Device { get; set; }
        private string SessionToken { get; set; }
        private string AuthToken { get; set; }
        private string AuthSigToken { get; set; }

        #endregion

        #region [ constructor ]

        public VeraMasterClass(ISessionInterface session, Action<MessageTypeEnum, string> messageCallBack)
        {
            this.Session = session;
            this.MessageCallBack = messageCallBack;
        }

        #endregion

        #region [ private helpers ]

        private AuthVeraModel GetAuthModel()
        {
            using (WebClient client = new WebClient())
            {

                var authUrl = string.Format("https://{0}/autha/auth/username/{1}?SHA1Password={2}&PK_Oem=1",
                    AUTHASERVER, Username, SHA1Password);
                string reply = client.DownloadString(authUrl);

                var authModel = new JavaScriptSerializer().Deserialize<AuthVeraModel>(reply);

                return authModel;
            }
        }

        private AuthTokenModel GetAuthTokenModel(string authToken)
        {
            byte[] data = Convert.FromBase64String(authToken);
            string decodedJson = Encoding.UTF8.GetString(data);

            var ret = new JavaScriptSerializer().Deserialize<AuthTokenModel>(decodedJson);
            return ret;
        }

        private string GetSessionToken(string server, string authToken, string authSigToken)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("MMSAuth", authToken);
                client.Headers.Add("MMSAuthSig", authSigToken);

                var url = string.Format("https://{0}/info/session/token", server);
                var reply = client.DownloadString(url);

                return reply;
            }
        }

        private DevicesReplyModel GetDevicesModel(string sessionTokenServer, string sessionToken, string pk_account)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("MMSSession", sessionToken);

                var url = string.Format("https://{0}/account/account/account/{1}/devices", sessionTokenServer, pk_account);
                var reply = client.DownloadString(url);

                var devicesModel = new JavaScriptSerializer().Deserialize<DevicesReplyModel>(reply);
                return devicesModel;
            }
        }


        private DeviceDeviceModel GetDeviceDeviceModel(string sessionToken, string[] service_devices, string pk_device)
        {
            var exceptionsList = new List<Exception>();


            foreach (var deviceUrl in service_devices) // Try both URLS. one of the two has the session, the other fails
            {
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        this.AddMessage(MessageTypeEnum.Information, string.Format("Vera - Try service device: {0}", deviceUrl));

                        client.Headers.Add("MMSSession", sessionToken);

                        var url = string.Format("https://{0}/device/device/device/{1}", deviceUrl, pk_device);
                        var reply = client.DownloadString(url);

                        var ret = new JavaScriptSerializer().Deserialize<DeviceDeviceModel>(reply);
                        return ret;
                    }
                }
                catch (Exception ex)
                {
                    exceptionsList.Add(ex);
                }
            }


            if (exceptionsList.Any())
                throw exceptionsList.Last(); // if nothing found, fire last error;

            return null;

        }

        private string LuupRequest(string sessionToken, string serverRelay, string pk_device, string request)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("MMSSession", sessionToken);
                var url = string.Format("https://{0}/relay/relay/relay/device/{1}/port_3480/{2}", serverRelay, pk_device, request);
                var reply = client.DownloadString(url);

                return reply;
            }
        }

        private void AddMessage(MessageTypeEnum messageType, string message)
        {
            MessageCallBack.Invoke(messageType, message);
        }

        private bool CreateVeraSession()
        {
            try
            {

                this.AddMessage(MessageTypeEnum.Information, "Vera - Authorizing");

                var authModel = this.GetAuthModel();


                var authToken = authModel.Identity;
                var authSigToken = authModel.IdentitySignature;

                var authTokenModel = this.GetAuthTokenModel(authToken);

                this.AddMessage(MessageTypeEnum.Information, "Vera - Get Authorize Session");
                var sessionToken = this.GetSessionToken(authTokenModel.Server_Auth, authToken, authSigToken);

                this.AddMessage(MessageTypeEnum.Information, "Vera - Get Devices");
                var devicesReply = this.GetDevicesModel(authTokenModel.Server_Auth, sessionToken, authTokenModel.PK_Account);

                var device = devicesReply.Devices.First();

                this.SessionToken = sessionToken;
                this.PK_device = device.PK_Device;
                this.AuthToken = authToken;
                this.AuthSigToken = authSigToken;


                this.AddMessage(MessageTypeEnum.Information, "Vera - Get Specific Device");
                var serverDeviceURLs = new string[] { device.Server_Device, device.Server_Device_Alt };
                var deviceDevice = this.GetDeviceDeviceModel(this.SessionToken, serverDeviceURLs, this.PK_device);

                this.AddMessage(MessageTypeEnum.Information, "Vera - Get Device Session");
                var relaySessionToken = this.GetSessionToken(deviceDevice.Server_Relay, this.AuthToken, this.AuthSigToken);
                this.RelaySessionToken = relaySessionToken;
                this.ServerRelay = deviceDevice.Server_Relay;

                Session.AddValue("VERA_RelaySessionToken", this.RelaySessionToken);
                Session.AddValue("VERA_ServerRelay", this.ServerRelay);
                Session.AddValue("VERA_PK_device", this.PK_device);

                Session.AddValue("VERA_sessionToken", this.SessionToken);
                Session.AddValue("VERA_AuthToken", this.AuthToken);
                Session.AddValue("VERA_AuthSigToken", this.AuthSigToken);

                return true;
            }
            catch (Exception ex)
            {
                this.AddMessage(MessageTypeEnum.Error, ex.ToString());
                return false;
            }

        }

        #endregion

        #region [ methods ]

        public string LuupRequest(string request)
        {
            var luupReply = this.LuupRequest(this.RelaySessionToken, this.ServerRelay, this.PK_device, request);
            return luupReply;
        }

        public void Initialize(string userID, string sha1password)
        {
            this.Username = userID;
            this.SHA1Password = sha1password;

            this.RelaySessionToken = Session.GetValue("VERA_RelaySessionToken");
            this.ServerRelay = Session.GetValue("VERA_ServerRelay");
            this.PK_device = Session.GetValue("VERA_PK_device");

            this.SessionToken = Session.GetValue("VERA_sessionToken");
            this.AuthToken = Session.GetValue("VERA_AuthToken");
            this.AuthSigToken = Session.GetValue("VERA_AuthSigToken");

            var neededVariables = new string[] { this.RelaySessionToken, this.ServerRelay, this.PK_device };
            var needsNewVeraSession = neededVariables.Any(i => string.IsNullOrEmpty(i));

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            if (needsNewVeraSession)
            {
                this.AddMessage(MessageTypeEnum.Information, "Initializing new Vera Session");
                this.IsInitialized = this.CreateVeraSession();
            }
            else
                this.AddMessage(MessageTypeEnum.Information, "Used last Vera Session");
        }

        public LuupStatusReply GetLuupStatus()
        {
            var reply = this.LuupRequest("data_request?id=status");
            var ret = new JavaScriptSerializer().Deserialize<LuupStatusReply>(reply);
            return ret;
        }

        #endregion
    }
}