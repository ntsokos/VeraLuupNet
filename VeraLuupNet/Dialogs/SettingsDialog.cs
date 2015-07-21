using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VeraLuupNet.Dialogs
{
    public partial class SettingsDialog : Form
    {

        #region [ consts ]

        private const string PASS_SEED = "oZ7QE6LcLJp6fiWzdqZc";

        #endregion

        #region [ constructor ]

        public SettingsDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region [ event handlers ]

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.Validate())
                this.SaveSettings();

        }

        #endregion

        #region [ private helpers ]

        private bool Validate()
        {
            var valErrors = new List<string>();

            if (string.IsNullOrEmpty(this.txtUsername.Text))
                valErrors.Add("Please input username");

            if (string.IsNullOrEmpty(this.txtPassword.Text))
                valErrors.Add("Please input password");


            if (valErrors.Any())
            {
                var msg = string.Join("\r\n", valErrors);
                MessageBox.Show(msg, "Validation errors", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            var ret = !valErrors.Any();
            return ret;
        }

        #endregion

        private void SaveSettings()
        {

            VeraLuupNet.Properties.Settings.Default.VeraUserName = this.txtUsername.Text.Trim();
            VeraLuupNet.Properties.Settings.Default.VeraSha1Password =
                this.Hash(this.txtUsername.Text.Trim(), this.txtPassword.Text.Trim(), PASS_SEED);

            VeraLuupNet.Properties.Settings.Default.Save();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;

        }

        private string Hash(string username, string password, string salt)
        {
            var finalString = username.ToLower() + password + salt;
            var finalSha1PassBytes = Encoding.UTF8.GetBytes(finalString);
            var retBytes = new SHA1Cng().ComputeHash(finalSha1PassBytes);

            var ret = BitConverter.ToString(retBytes).Replace("-", "");



            return ret;

        }
    }
}
