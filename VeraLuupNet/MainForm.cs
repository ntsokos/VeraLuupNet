using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VeraLuupNet.Framework;
using VeraLuupNet.Framework.Enums;
using VeraLuupNet.Helpers;
using VeraLuupNet.Utils;
using VeraLuupNet.Extentions;
using System.IO;

namespace VeraLuupNet
{
    public partial class MainForm : Form
    {
        #region [ properties ]

        VeraMasterClass Vera;

        #endregion

        #region [ constructor ]

        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region [ event handlers ]

        private void miAbout_Click(object sender, EventArgs e)
        {
            var wnd = new AboutVeraLuupNetBox();
            wnd.ShowDialog();
        }

        private void miSettings_Click(object sender, EventArgs e)
        {
            var wnd = new VeraLuupNet.Dialogs.SettingsDialog();
            wnd.ShowDialog();
        }

        private void btnRUN_Click(object sender, EventArgs e)
        {
            this.CheckVera();
            if (!this.Vera.IsInitialized)
                return;

            var request = this.txtRequest.Text.Trim();
            var luupRequestStr = string.Format("data_request?id={0}", request);

            this.flowLayoutPanel1.AddVeraRequest(luupRequestStr);

            var reply = this.Vera.LuupRequest(luupRequestStr);

            this.flowLayoutPanel1.AddVeraReply(reply);
        }

        #endregion

        #region [ callbacks ]

        private void VeraMessagesCallBack(MessageTypeEnum messageType, string message)
        {
            //var addText = string.Format("{0} - {1}\r\n", messageType, message);
            this.flowLayoutPanel1.AddMessage(messageType, message);
        }

        #endregion

        #region [ helpers ]

        private void CheckVera()
        {
            if (this.Vera == null)
                this.Vera = new VeraMasterClass(new SessionHolder(), this.VeraMessagesCallBack);

            if (!this.Vera.IsInitialized)
            {
                var username = VeraLuupNet.Properties.Settings.Default.VeraUserName;
                var sha1password = VeraLuupNet.Properties.Settings.Default.VeraSha1Password;

                if (new string[] { username, sha1password }.Any(i => string.IsNullOrEmpty(i)))
                {
                    MessageBox.Show("Go to settings and enter username and password", "Settings",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                this.Vera.Initialize(username, sha1password);
            }

        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {

            flowLayoutPanel1.AddMessage(MessageTypeEnum.Debug, "Debug Message");

            var jsontest = @"D:\nikos\SourceTFSOnline\EntrodusIOTProject\EntroIOT.Web\EntroIOT.Web\VERA\ExampleFiles\Luup_Status.json";
            var reply = File.ReadAllText(jsontest);
            flowLayoutPanel1.AddVeraReply(reply);

        }
        
    }
}
