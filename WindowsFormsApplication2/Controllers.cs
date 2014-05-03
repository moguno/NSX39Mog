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
    /// コンボボックス用の名前とデータを格納するクラス
    /// </summary>
    public class ComboItem<T>
    {
        /// <summary>
        /// コンボボックスに表示する文字列
        /// </summary>
        public string Key;

        /// <summary>
        /// 値
        /// </summary>
        public T Data;

        /// <summary>
        /// コンボボックスに表示する文字列を返す
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Key;
        }
    }


    /// <summary>
    /// コントローラの基底クラス
    /// </summary>
    public class Controller
    {
        protected Action<Controller> OnApply;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="AOnApply">コントローラの値をポケミクに送るときの処理</param>
        public Controller(Action<Controller> AOnApply)
        {
            OnApply = AOnApply;
        }

        /// <summary>
        /// コントローラの値をポケミクに送る
        /// </summary>
        public void Apply()
        {
            OnApply.Invoke(this);
        }
    }


    /// <summary>
    /// リッチテキストボックスとボタンから成るコントローラ（主に歌詞用）
    /// </summary>
    public class TextAndButton : Controller
    {
        protected Panel Group;
        protected RichTextBox XText;
        protected Button ApplyButton;
        protected Func<string, Tuple<int, int>[]> CheckError = null;

        public string Text
        {
            get
            {
                return XText.Text;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="AGroup">コントローラ化するグループボックス</param>
        /// <param name="AOnApply">コントローラの値をポケミクに送るときの処理</param>
        public TextAndButton(Panel AGroup, Action<Controller> AOnApply, Func<string, Tuple<int, int>[]> ACheckError)
            : base(AOnApply)
        {
            Group = AGroup;

            XText = Group.Controls.OfType<RichTextBox>().ElementAt<RichTextBox>(0);
            ApplyButton = Group.Controls.OfType<Button>().ElementAt<Button>(0);

            CheckError = ACheckError;


            ApplyButton.Click += (sender, e) =>
            {
                ACheckError.Invoke(XText.Text).ToList<Tuple<int, int>>().ForEach((ErrorRange) =>
                {
                    XText.Select(ErrorRange.Item1, ErrorRange.Item2);
                    XText.SelectionColor = Color.Red;
                });

                XText.Select(0, 0);

                Apply();
            };

            XText.GotFocus += (sender, e) =>
            {
                int CursorPos = XText.SelectionStart;

                XText.SelectAll();
                XText.SelectionColor = Color.Black;
                XText.Select(CursorPos, 0);
            };
        }
    }


    /// <summary>
    /// タッチ対応のときは押してるときだけ有効なボタン。
    /// タッチ非対応のときはトグルボタン
    /// </summary>
    public class ToggleAndTouchButton : Controller
    {
        protected TouchSupportedButton Button;
        protected bool TouchMode = false;
        protected bool XOn = false;
        
        public bool On
        {
            get
            {
                return XOn;
            }
        }

        /// <summary>
        /// ボタンのON/OFFが切り替わったイベントを発生させる
        /// </summary>
        /// <param name="sender">マウスイベント発生コントロール</param>
        /// <param name="e">マウスイベント</param>
        /// <param name="SwitchOn">現在のボタンの状態</param>
        protected void FireButtonEvent(object sender, MouseEventArgs e, bool SwitchOn)
        {
            // タッチスクリーン対策
            if (e.Button == MouseButtons.None)
            {
                TouchMode = true;
            }
            else
            {
                return;
            }

            XOn = SwitchOn;

            Apply();
        }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="AButton">コントローラ化するタッチ対応ボタン</param>
        /// <param name="AOnApply">コントローラの値をポケミクに送るときの処理</param>
        public ToggleAndTouchButton(TouchSupportedButton AButton, Action<Controller> AOnApply)
            : base(AOnApply)
        {
            Button = AButton;

            Button.MouseDown += (sender, e) =>
                {
                    FireButtonEvent(sender, e, true);
                };

            Button.MouseUp += (sender, e) =>
                {
                    FireButtonEvent(sender, e, false);
                };

            Button.Click += (sender, e) =>
                {
                    if (!TouchMode)
                    {
                        XOn = !XOn;

                        Apply();
                    }
                };
        }

    }


    /// <summary>
    /// コンボボックスのみのコントローラ（主にプログラムチェンジ用）
    /// </summary>
    public class ComboOnly<T> : Controller
    {
        protected ComboBox Combo;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ACombo">コントローラ化するコンボボックス</param>
        /// <param name="Items">コンボボックスに表示する文字列</param>
        /// <param name="AOnApply">コントローラの値をポケミクに送るときの処理</param>
        public ComboOnly(ComboBox ACombo, List<ComboItem<T>> Items, Action<Controller> AOnApply)
            : base(AOnApply)
        {
            Combo = ACombo;

            Initialize(Items);
        }


        /// <summary>
        /// コントローラの初期化
        /// </summary>
        /// <param name="Items">コンボボックスに表示する文字列</param>
        protected void Initialize(List<ComboItem<T>> Items)
        {
            Combo.Items.AddRange(Items.ToArray());
            Combo.SelectedIndex = 0;

            Combo.SelectedIndexChanged += (sender, e) =>
            {
                Apply();
            };
        }

        public ComboItem<T> Item
        {
            get
            {
                return (ComboItem<T>)Combo.SelectedItem;
            }
        }
    }


    /// <summary>
    /// コンボボックスとスライダーから成るコントローラ（エフェクト用）
    /// </summary>
    public class ComboAndSlider : Controller
    {
        protected GroupBox Group;
        protected ComboBox Combo;
        protected ScrollBar Slider;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="AGroup">コントローラ化するグループボックス</param>
        /// <param name="Items">コンボボックスに表示する項目</param>
        /// <param name="AOnApply">コントローラの値をポケミクに送るときの処理</param>
        public ComboAndSlider(GroupBox AGroup, List<ComboItem<byte[]>> Items, Action<Controller> AOnApply)
            : base(AOnApply)
        {
            Group = AGroup;

            Combo = Group.Controls.OfType<ComboBox>().ElementAt<ComboBox>(0);
            Slider = Group.Controls.OfType<ScrollBar>().ElementAt<ScrollBar>(0);

            Initialize(Items);
        }

        public ComboItem<byte[]> Item
        {
            get
            {
                return (ComboItem<byte[]>)Combo.SelectedItem;
            }
        }

        public byte Value
        {
            get
            {
                return (byte)Slider.Value;
            }
        }

        /// <summary>
        /// コントローラの初期化
        /// </summary>
        /// <param name="Items">コンボボックスに表示する項目</param>
        public void Initialize(List<ComboItem<byte[]>> Items)
        {
            Combo.Items.Clear();
            Combo.Items.AddRange(Items.ToArray());
            Combo.SelectedIndex = 0;

            Combo.SelectedValueChanged += (sender, e) =>
            {
                Apply();
            };

            Slider.ValueChanged += (sender, e) =>
            {
                Apply();
            };
        }
    }


    /// <summary>
    /// スライダーのみのコントローラ
    /// </summary>
    public class SliderOnly : Controller
    {
        protected ScrollBar Slider;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ASlider">コントローラ化するスクロールバー</param>
        /// <param name="AOnApply">コントローラの値をポケミクに送るときの処理</param>
        public SliderOnly(ScrollBar ASlider, Action<Controller> AOnApply)
            : base(AOnApply)
        {
            Slider = ASlider;

            Slider.ValueChanged += (sender, e) =>
            {
                Apply();
            };
        }

        public byte Value
        {
            get
            {
                return (byte)Slider.Value;
            }
        }
    }
}
