using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSX39Mog
{
    /// <summary>
    /// Windows8.1のタッチイベントを拾えるボタン
    /// </summary>
    public class TouchSupportedButton:Button
    {
        /// <summary>
        /// Window Proc
        /// </summary>
        /// <param name="Msg">Windowメッセージ</param>
        protected override void WndProc(ref Message Msg)
        {
            switch(Msg.Msg)
            {
                case 0x0246:
                    OnMouseDown(new MouseEventArgs(System.Windows.Forms.MouseButtons.None, 1, 0, 0, 0));

                    Msg.Result = (IntPtr)1;
                    break;

                case 0x0247:
                    OnMouseUp(new MouseEventArgs(System.Windows.Forms.MouseButtons.None, 1, 0, 0, 0));

                    Msg.Result = (IntPtr)1;
                    break;
            }

            base.WndProc(ref Msg);
        }
    }
}
