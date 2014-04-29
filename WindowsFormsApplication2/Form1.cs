using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace NSX39Mog
{
    public partial class Form1 : Form
    {
        protected List<Controller> Controllers = new List<Controller>();
        protected Piano PianoPane = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }


        /// <summary>
        /// すべてのコントローラーの設定をポケミクに送る。
        /// </summary>
        protected void ApplyAll()
        {
            Controllers.ForEach((Controller) =>
            {
                Controller.Apply();
            }
            );
        }


        /// <summary>
        /// ポケミクプラグアンドプレイ用のポーリングタイマ処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            NSX39 Miku = NSX39.GetInstance();

            Miku.PlugAndPlay(() =>
            {
                Miku.Reset();
                ApplyAll();
            });

            if (Miku.IsActive)
            {
                toolStripStatusLabel1.Text = "ポケミクにんしきちゅう！";
            }
            else
            {
                toolStripStatusLabel1.Text = "ポケミクがいないよ";
            }
        }


        /// <summary>
        /// フォームがロードされた時に各種初期化を行う。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {

            Controllers.Add(new ComboAndSlider(groupBox1, Constants.Reverbs, (Obj) =>
            {
                var This = (ComboAndSlider)Obj;

                NSX39.GetInstance().Reverb(This.Item.Data, This.Value);
            }
            ));


            Controllers.Add(new ComboAndSlider(groupBox2, Constants.Choruses, (Obj) =>
            {
                var This = (ComboAndSlider)Obj;

                NSX39.GetInstance().Chorus(This.Item.Data, This.Value);
            }
            ));


            Controllers.Add(new ComboAndSlider(groupBox3, Constants.VarEffects, (Obj) =>
            {
                var This = (ComboAndSlider)Obj;

                NSX39.GetInstance().VariationEffect(This.Item.Data, This.Value);
            }
            ));


            PianoPane = new Piano(panel3, (NoteNo, NoteOn) =>
            {
                NSX39.GetInstance().Note(NoteOn, toolStripComboBox1.SelectedIndex, 12 * toolStripComboBox3.SelectedIndex + NoteNo, 127);
            });


            Controllers.Add(new ToolComboOnly(toolStripComboBox2, Constants.Programs, (Obj) =>
            {
                var This = (ToolComboOnly)Obj;

                NSX39.GetInstance().ProgramChange(1, This.Index);
            }));


            Controllers.Add(new TextAndButton(panel1, (Obj) =>
            {
                var This = (TextAndButton)Obj;

                var Reg = new Regex("(?<Lyric>[あ-ん][ぁぃぅぇぉゃゅょ]?)");

                var Matches = Reg.Matches(This.Text);

                var LyricArray = new List<byte>();

                foreach (Match Mat in Matches)
                {
                    byte Tmp;

                    if (Constants.Lyrics.TryGetValue(Mat.Value, out Tmp))
                    {
                        LyricArray.Add(Tmp);
                    }
                    else
                    {
                        LyricArray.Add(Constants.Lyrics["ん"]);
                    }
                }

                NSX39.GetInstance().Lyrics(LyricArray.ToArray());
            }));


            toolStripComboBox1.SelectedIndex = 0;
            toolStripComboBox3.SelectedIndex = 5;
        }


        /// <summary>
        /// フォームを閉じるときにポケミクを閉じる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            NSX39.GetInstance().Close();
        }


        /// <summary>
        /// ピアノのリサイズの時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel2_Resize(object sender, EventArgs e)
        {
            if (PianoPane != null)
            {
                PianoPane.ResizePianoKeys();
            }
        }
    }
}