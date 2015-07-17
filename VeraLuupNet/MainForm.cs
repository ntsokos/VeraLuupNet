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
using VeraLuupNet.Utils;

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

        private void mnAbout_Click(object sender, EventArgs e)
        {
            var wnd = new AboutVeraLuupNetBox();
            wnd.ShowDialog();
        }

        #endregion

        #region [ callbacks ]

        private void VeraMessagesCallBack(MessageTypeEnum messageType, string message)
        {
            var addText = string.Format("{0} - {1}\r\n", messageType, message);
            this.txtVeraMessages.AppendText(addText);
        }

        #endregion

        private void btnRUN_Click(object sender, EventArgs e)
        {
            if (this.Vera == null)
                this.Vera = new VeraMasterClass(new SessionHolder(), this.VeraMessagesCallBack);

            if (!this.Vera.IsInitialized)
            {
                this.Vera.Initialize();
            }

            if (!this.Vera.IsInitialized)
                return;
        }

    }
}
