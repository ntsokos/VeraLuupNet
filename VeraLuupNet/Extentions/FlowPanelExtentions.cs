using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VeraLuupNet.Framework.Enums;
using VeraLuupNet.Helpers;

namespace VeraLuupNet.Extentions
{
    public static class FlowPanelExtentions
    {
        public static void AddVeraRequest(this FlowLayoutPanel source, string request)
        {
            var panel = new FlowLayoutPanel();
            panel.AutoSize = true;
            panel.WrapContents = false;
            panel.FlowDirection = FlowDirection.TopDown;

            var lbl = new Label();
            lbl.AutoSize = true;
            lbl.Margin = new Padding(10, 10, 10, 0);
            lbl.Text = "Request";
            lbl.ForeColor = Color.FromName("White");
            lbl.BackColor = MessageTypeEnum.Success.ToColor();
            lbl.BorderStyle = BorderStyle.FixedSingle;
            lbl.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold);
            panel.Controls.Add(lbl);

            var lbl2 = new Label();
            lbl2.AutoSize = true;
            lbl2.Margin = new Padding(10, 5, 10, 5);
            lbl2.Text = request;
            lbl2.Font = new Font("Microsoft Sans Serif", 10f);
            panel.Controls.Add(lbl2);

            source.Controls.Add(panel);
            Application.DoEvents();
        }

        public static void AddVeraReply(this FlowLayoutPanel source, string veraReply)
        {
            var panel = new FlowLayoutPanel();
            panel.AutoSize = true;
            panel.WrapContents = false;
            panel.Dock = DockStyle.Right;
            panel.FlowDirection = FlowDirection.TopDown;

            var lbl = new Label();
            lbl.AutoSize = true;
            lbl.Margin = new Padding(10, 10, 10, 0);
            lbl.Text = "Reply";
            lbl.ForeColor = Color.FromName("White");
            lbl.BackColor = MessageTypeEnum.Success.ToColor();
            lbl.BorderStyle = BorderStyle.FixedSingle;
            lbl.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold);
            panel.Controls.Add(lbl);

            var finalStr = veraReply;
            if (VeraLuupNetHelpers.IsJson(finalStr))
                finalStr = VeraLuupNetHelpers.FormatJSON(finalStr);

            var txt = new TextBox();
            txt.ReadOnly = false;
            txt.Multiline = true;
            txt.Text = finalStr;
            txt.Margin = new Padding(10, 5, 10, 5);
            txt.Font = new Font("Microsoft Sans Serif", 10f);
            txt.BorderStyle = BorderStyle.None;
            txt.Height = (txt.Font.Height + 2) * txt.Lines.Count();
            txt.Width = source.Width - 40;
            panel.Controls.Add(txt);

            var myToolTip = new System.Windows.Forms.ToolTip
            {
                AutomaticDelay = 5000,
                AutoPopDelay = 50000,
                InitialDelay = 500,
                ReshowDelay = 5000
            };

            myToolTip.SetToolTip(txt, "Double click to copy to clipboard");
            txt.DoubleClick +=
                (sender, e) =>
                {
                    System.Windows.Forms.Clipboard.SetText((sender as TextBox).Text);
                    MessageBox.Show("Copied to clipboard");
                };

            source.Controls.Add(panel);

            Application.DoEvents();
        }

        public static void AddMessage(this FlowLayoutPanel source, MessageTypeEnum messageType, string message)
        {
            var panel = new FlowLayoutPanel();
            panel.AutoSize = true;
            panel.WrapContents = false;
            panel.FlowDirection = FlowDirection.TopDown;

            var lbl = new Label();
            lbl.AutoSize = true;
            lbl.Margin = new Padding(10, 10, 10, 0);
            lbl.Text = messageType.ToString();
            lbl.ForeColor = Color.FromName("White");
            lbl.BackColor = messageType.ToColor();
            lbl.BorderStyle = BorderStyle.FixedSingle;
            lbl.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold);
            panel.Controls.Add(lbl);

            var lbl2 = new Label();
            lbl2.AutoSize = true;
            lbl2.Margin = new Padding(10, 5, 10, 5);
            lbl2.Text = message;
            lbl2.Font = new Font("Microsoft Sans Serif", 10f);
            panel.Controls.Add(lbl2);

            source.Controls.Add(panel);
            Application.DoEvents();
        }

        private static Color ToColor(this MessageTypeEnum source)
        {

            switch (source)
            {
                case MessageTypeEnum.Debug:
                    return System.Drawing.ColorTranslator.FromHtml("#31b0d5");
                case MessageTypeEnum.Information:
                    return System.Drawing.ColorTranslator.FromHtml("#286090");
                case MessageTypeEnum.Success:
                    return System.Drawing.ColorTranslator.FromHtml("#449d44");
                case MessageTypeEnum.Warning:
                    return System.Drawing.ColorTranslator.FromHtml("#ec971f");
                case MessageTypeEnum.Error:
                    return System.Drawing.ColorTranslator.FromHtml("#c9302c");
                default:
                    return Color.FromName("black");
            }
        }
    }
}
