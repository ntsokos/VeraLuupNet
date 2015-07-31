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
using VeraLuupNet.Framework.Helpers;

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

                Debug.WriteLine(authUrl);
                string reply = client.DownloadString(authUrl);

                var authModel = new JavaScriptSerializer().Deserialize<AuthVeraModel>(reply);

                return authModel;
            }
        }

        private AuthTokenModel DeserializeAuthTokenModel(string authToken)
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

                Debug.WriteLine(url);
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

                Debug.WriteLine(url);
                var reply = client.DownloadString(url);

                var devicesModel = new JavaScriptSerializer().Deserialize<DevicesReplyModel>(reply);
                return devicesModel;
            }
        }

        private DeviceDeviceModel GetDeviceDeviceModel(string sessionToken, string deviceUrl, string pk_device)
        {
            using (WebClient client = new WebClient())
            {

                client.Headers.Add("MMSSession", sessionToken);

                var url = string.Format("https://{0}/device/device/device/{1}", deviceUrl, pk_device);

                Debug.WriteLine(url);
                var reply = client.DownloadString(url);

                var ret = new JavaScriptSerializer().Deserialize<DeviceDeviceModel>(reply);
                return ret;
            }

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
                this.AddMessage(MessageTypeEnum.Debug, "Vera - Authorizing");

                var authModel = this.GetAuthModel();

                var authToken = authModel.Identity;
                var authSigToken = authModel.IdentitySignature;

                var authTokenModel = this.DeserializeAuthTokenModel(authToken);

                var serverAccountUrl = FrameworkHelpers.GetRandomUrl(authModel.Server_Account, authModel.Server_Account_Alt);
                this.AddMessage(MessageTypeEnum.Debug, "Vera - Get Server Account Session");
                var serverAccountSession = this.GetSessionToken(serverAccountUrl, authToken, authSigToken);
                this.AddMessage(MessageTypeEnum.Debug, "Vera - Get Account Devices");
                var devicesReply = this.GetDevicesModel(serverAccountUrl, serverAccountSession, authTokenModel.PK_Account);

                var device = devicesReply.Devices.First();
                this.PK_device = device.PK_Device;

                var serverDeviceUrl = FrameworkHelpers.GetRandomUrl(device.Server_Device, device.Server_Device_Alt);
                this.AddMessage(MessageTypeEnum.Debug, "Vera - Get Server Device Session");
                var serverDeviceSession = this.GetSessionToken(serverDeviceUrl, authToken, authSigToken); 
                this.AddMessage(MessageTypeEnum.Debug, "Vera - Get Specific Device");
                var deviceDevice = this.GetDeviceDeviceModel(serverDeviceSession, serverDeviceUrl, this.PK_device);

                this.AddMessage(MessageTypeEnum.Debug, "Vera - Get Relay Session");
                var relaySessionToken = this.GetSessionToken(deviceDevice.Server_Relay, authToken, authSigToken);

                this.RelaySessionToken = relaySessionToken;
                this.ServerRelay = deviceDevice.Server_Relay;

                Session.AddValue("VERA_RelaySessionToken", this.RelaySessionToken);
                Session.AddValue("VERA_ServerRelay", this.ServerRelay);
                Session.AddValue("VERA_PK_device", this.PK_device);

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