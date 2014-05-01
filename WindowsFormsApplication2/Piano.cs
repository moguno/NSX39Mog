using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSX39Mog
{
    /// <summary>
    /// ピアノウィジェットクラス
    /// </summary>
    public class Piano
    {
        protected Panel BasePanel;
        protected Action<int, bool> NoteFunc;
        protected List<Button> WhiteKeys = new List<Button>();
        protected List<Button> BlackKeys = new List<Button>();
        protected bool TouchMode = false;

        protected const int Octaves = 2;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ABasePanel">ピアノを貼り付けるパネル</param>
        /// <param name="ANoteFunc">鍵盤を押したり離したりしたときの処理</param>
        public Piano(Panel ABasePanel, Action<int, bool> ANoteFunc)
        {
            BasePanel = ABasePanel;
            NoteFunc = ANoteFunc;

            CreatePianoKeys();
            ResizePianoKeys();

            BasePanel.Resize += (sender, e) =>
                {
                    ResizePianoKeys();
                };
        }


        /// <summary>
        /// 鍵盤のサイズを調整する。
        /// </summary>
        public void ResizePianoKeys()
        {
            int WhiteWidth = BasePanel.Width / WhiteKeys.Count;

            for (int i = 0; i < WhiteKeys.Count; i++)
            {
                WhiteKeys[i].Top = 0;
                WhiteKeys[i].Height = BasePanel.Height;

                WhiteKeys[i].Left = WhiteWidth * i;
                WhiteKeys[i].Width = WhiteWidth;
            }

            var Mask = new bool[] { true, true, false, true, true, true, false };
            int BlackWidth = (int)((double)WhiteWidth * 0.7);

            int TargetKeyIndex = 0;

            for (int i = 0; i < Octaves * 7; i++)
            {
                if (Mask[i % 7])
                {
                    BlackKeys[TargetKeyIndex].Top = 0;
                    BlackKeys[TargetKeyIndex].Height = (int)((double)BasePanel.Height * 0.6);

                    BlackKeys[TargetKeyIndex].Left = WhiteWidth * (i + 1) - (int)((double)(BlackWidth) * 0.5);
                    BlackKeys[TargetKeyIndex].Width = BlackWidth;

                    TargetKeyIndex++;
                }
            }
        }


        /// <summary>
        /// 鍵盤用のボタンを生成する。
        /// </summary>
        protected void CreatePianoKeys()
        {
            for (int i = 0; i < Octaves * 5; i++)
            {
                var Key = new TouchSupportedButton();
                Key.BackColor = Color.Black;
                Key.MouseDown += new MouseEventHandler(OnMouseDown);
                Key.MouseUp += new MouseEventHandler(OnMouseUp);
                BlackKeys.Add(Key);
            }

            BasePanel.Controls.AddRange(BlackKeys.ToArray());

            for (int i = 0; i < Octaves * 7 + 1; i++)
            {
                var Key = new TouchSupportedButton();
                Key.BackColor = Color.White;
                Key.MouseDown += new MouseEventHandler(OnMouseDown);
                Key.MouseUp += new MouseEventHandler(OnMouseUp);
                WhiteKeys.Add(Key);
            }

            BasePanel.Controls.AddRange(WhiteKeys.ToArray());
        }


        /// <summary>
        /// 鍵盤を押したり離したときのイベントを発生させる。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="NoteOn">true：押したとき　false：離したとき</param>
        protected void FireNoteEvent(object sender, MouseEventArgs e, bool NoteOn)
        {
            int index;
            var WhiteDelta = new int[] { 0, 2, 4, 5, 7, 9, 11 };
            var BlackDelta = new int[] { 1, 3, 6, 8, 10 };
            int NoteNo = -1;

            // タッチスクリーン対策
            if (e.Button == MouseButtons.None)
            {
                TouchMode = true;
            }

            if (TouchMode && e.Button != MouseButtons.None)
            {
                return;
            }

            index = WhiteKeys.IndexOf((Button)sender);

            if (index != -1)
            {
                NoteNo = WhiteDelta[index % WhiteDelta.Length] + 12 * (index / WhiteDelta.Length);
            }
            else
            {
                index = BlackKeys.IndexOf((Button)sender);

                NoteNo = BlackDelta[index % BlackDelta.Length] + 12 * (index / BlackDelta.Length);
            }

            NoteFunc.Invoke(NoteNo, NoteOn);
        }


        /// <summary>
        /// 鍵盤が押されたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnMouseDown(object sender, MouseEventArgs e)
        {
            FireNoteEvent(sender, e, true);
        }


        /// <summary>
        /// 鍵盤が離されたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnMouseUp(object sender, MouseEventArgs e)
        {
            FireNoteEvent(sender, e, false);
        }
    }
}
