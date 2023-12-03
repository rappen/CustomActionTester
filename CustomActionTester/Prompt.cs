using System.Drawing;
using System.Windows.Forms;

namespace Rappen.XTB.CAT
{
    public static class Prompt
    {
        public static string ShowDialog(string text, string caption, string startvalue = "", bool multiline = false)
        {
            var textLabel = new Label
            {
                Left = 50,
                Top = 20,
                Width = 430,
                Text = text
            };
            var textBox = new TextBox
            {
                Left = 50,
                Top = 45,
                Width = 400,
                Height = multiline ? 270 : 20,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                Text = startvalue,
                Multiline = multiline
            };
            var cancellation = new Button
            {
                Text = "Cancel",
                Left = 220,
                Width = 100,
                Top = multiline ? 330 : 80,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                DialogResult = DialogResult.Cancel
            };
            var confirmation = new Button
            {
                Text = "OK",
                Left = 350,
                Width = 100,
                Top = multiline ? 330 : 80,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                DialogResult = DialogResult.OK
            };
            var prompt = new Form
            {
                BackColor = SystemColors.Window,
                Width = 500,
                Height = multiline ? 400 : 150,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                CancelButton = cancellation,
                AcceptButton = multiline ? null : confirmation
            };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(cancellation);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            if (prompt.ShowDialog() == DialogResult.OK)
            {
                return textBox.Text;
            }
            return startvalue;
        }
    }
}