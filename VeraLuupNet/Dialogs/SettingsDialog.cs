using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VeraLuupNet.Framework.Helpers;

namespace VeraLuupNet.Dialogs
{
    public partial class SettingsDialog : Form
    {

        #region [ constructor ]

        public SettingsDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region [ event handlers ]

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!this.Validate())
                return;

            if (!this.SaveSettings())
                return;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
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

        private bool SaveSettings()
        {

            VeraLuupNet.Properties.Settings.Default.VeraUserName = this.txtUsername.Text.Trim();
            VeraLuupNet.Properties.Settings.Default.VeraSha1Password =
                FrameworkHelpers.GetHashedPassword(this.txtUsername.Text.Trim(), this.txtPassword.Text.Trim());

            VeraLuupNet.Properties.Settings.Default.Save();

            return true;
        }

        #endregion

    }
}
