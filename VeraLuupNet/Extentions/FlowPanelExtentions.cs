using System;
using System.Collections.Generic;
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
        public static void AddVeraReply(this FlowLayoutPanel source, string veraReply)
        {
            var finalStr = veraReply;
            if (VeraLuupNetHelpers.IsJson(finalStr))
                finalStr = VeraLuupNetHelpers.FormatJSON(finalStr);

            var textbox = new TextBox();
            textbox.Multiline = true;
            textbox.AppendText(finalStr);

            source.Controls.Add(textbox);
        }

        public static void AddText(this FlowLayoutPanel source, string text)
        {
            var lbl = new Label();
            lbl.AutoSize = true;
            lbl.Height = 30;
            lbl.Text = text;

            source.Controls.Add(lbl);
        }

        public static void AddMessage(this FlowLayoutPanel source, MessageTypeEnum messageType, string message)
        {
            var txt = string.Format("{0} - {1}\r\n", messageType, message);
            source.AddText(txt);
        }
        
    }
}
