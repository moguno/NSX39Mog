using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSX39Mog
{
    /// <summary>
    /// コンボボックス用の名前とデータを格納するクラス
    /// </summary>
    public class ComboItem
    {
        /// <summary>
        /// コンボボックスに表示する文字列
        /// </summary>
        public string Key;

        /// <summary>
        /// 値
        /// </summary>
        public byte[] Data;

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
    /// テキストボックスとボタンから成るコントローラ（主に歌詞用）
    /// </summary>
    public class TextAndButton : Controller
    {
        protected Panel Group;
        protected TextBox XText;
        protected Button ApplyButton;

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
        public TextAndButton(Panel AGroup, Action<Controller> AOnApply)
            : base(AOnApply)
        {
            Group = AGroup;

            XText = Group.Controls.OfType<TextBox>().ElementAt<TextBox>(0);
            ApplyButton = Group.Controls.OfType<Button>().ElementAt<Button>(0);

            ApplyButton.Click += (sender, e) =>
            {
                Apply();
            };
        }
    }


    /// <summary>
    /// コンボボックスのみのコントローラ（主にプログラムチェンジ用）
    /// </summary>
    public class ToolComboOnly : Controller
    {
        protected ToolStripComboBox Combo;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ACombo">コントローラ化するコンボボックス</param>
        /// <param name="Items">コンボボックスに表示する文字列</param>
        /// <param name="AOnApply">コントローラの値をポケミクに送るときの処理</param>
        public ToolComboOnly(ToolStripComboBox ACombo, List<string> Items, Action<Controller> AOnApply)
            : base(AOnApply)
        {
            Combo = ACombo;

            Initialize(Items);
        }


        /// <summary>
        /// コントローラの初期化
        /// </summary>
        /// <param name="Items">コンボボックスに表示する文字列</param>
        protected void Initialize(List<string> Items)
        {
            Combo.Items.AddRange(Items.ToArray());
            Combo.SelectedIndex = 0;

            Combo.SelectedIndexChanged += (sender, e) =>
            {
                Apply();
            };
        }


        public short Index
        {
            get
            {
                return (short)Combo.SelectedIndex;
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
        protected TrackBar Slider;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="AGroup">コントローラ化するグループボックス</param>
        /// <param name="Items">コンボボックスに表示する項目</param>
        /// <param name="AOnApply">コントローラの値をポケミクに送るときの処理</param>
        public ComboAndSlider(GroupBox AGroup, List<ComboItem> Items, Action<Controller> AOnApply)
            : base(AOnApply)
        {
            Group = AGroup;

            Combo = Group.Controls.OfType<ComboBox>().ElementAt<ComboBox>(0);
            Slider = Group.Controls.OfType<TrackBar>().ElementAt<TrackBar>(0);

            Initialize(Items);
        }

        public ComboItem Item
        {
            get
            {
                return (ComboItem)Combo.SelectedItem;
            }
        }

        public short Value
        {
            get
            {
                return (short)Slider.Value;
            }
        }

        /// <summary>
        /// コントローラの初期化
        /// </summary>
        /// <param name="Items">コンボボックスに表示する項目</param>
        public void Initialize(List<ComboItem> Items)
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
}
