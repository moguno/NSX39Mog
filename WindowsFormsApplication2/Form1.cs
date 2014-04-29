using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        protected List<Controller> Controllers = new List<Controller>();
        protected Piano PianoPane = null;

        public Form1()
        {
            InitializeComponent();
        }

        protected void ApplyAll()
        {
            Controllers.ForEach((Controller) =>
            {
                Controller.Apply();
            }
            );
        }

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

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var Reverbs = new List<ComboItem>()
            {
                new ComboItem() { Key = "off", Data = new byte[] { 0x00, 0x00 }},
                new ComboItem() { Key = "hall1", Data = new byte[] { 0x01, 0x00 }},
                new ComboItem() { Key = "hall2", Data = new byte[] { 0x01, 0x10 }},
                new ComboItem() { Key = "hall3", Data = new byte[] { 0x01, 0x11 }},
                new ComboItem() { Key = "hall4", Data = new byte[] { 0x01, 0x12 }},
                new ComboItem() { Key = "hall5", Data = new byte[] { 0x01, 0x01 }},
                new ComboItem() { Key = "hall_m", Data = new byte[] { 0x01, 0x06 }},
                new ComboItem() { Key = "hall_l", Data = new byte[] { 0x01, 0x07 }},

                new ComboItem() { Key = "room1", Data = new byte[] { 0x02, 0x10 }},
                new ComboItem() { Key = "room2", Data = new byte[] { 0x02, 0x11 }},
                new ComboItem() { Key = "room3", Data = new byte[] { 0x02, 0x12 }},
                new ComboItem() { Key = "room4", Data = new byte[] { 0x02, 0x13 }},
                new ComboItem() { Key = "room5", Data = new byte[] { 0x02, 0x00 }},
                new ComboItem() { Key = "room6", Data = new byte[] { 0x02, 0x01 }},
                new ComboItem() { Key = "room7", Data = new byte[] { 0x02, 0x02 }},
                new ComboItem() { Key = "room_s", Data = new byte[] { 0x02, 0x05 }},
                new ComboItem() { Key = "room_m", Data = new byte[] { 0x02, 0x06 }},
                new ComboItem() { Key = "room_l", Data = new byte[] { 0x02, 0x07 }},

                new ComboItem() { Key = "stage1", Data = new byte[] { 0x03, 0x10 }},
                new ComboItem() { Key = "stage2", Data = new byte[] { 0x03, 0x11 }},
                new ComboItem() { Key = "stage3", Data = new byte[] { 0x03, 0x00 }},
                new ComboItem() { Key = "stage4", Data = new byte[] { 0x03, 0x01 }},

                new ComboItem() { Key = "plate1", Data = new byte[] { 0x04, 0x10 }},
                new ComboItem() { Key = "plate2", Data = new byte[] { 0x04, 0x11 }},
                new ComboItem() { Key = "plate3", Data = new byte[] { 0x04, 0x00 }},
                new ComboItem() { Key = "gm_plate", Data = new byte[] { 0x04, 0x07 }},

                new ComboItem() { Key = "white_room", Data = new byte[] { 0x10, 0x00 }},
                new ComboItem() { Key = "tunnel", Data = new byte[] { 0x11, 0x00 }},
                new ComboItem() { Key = "canyon", Data = new byte[] { 0x12, 0x00 }},
                new ComboItem() { Key = "basement", Data = new byte[] { 0x13, 0x00 }},
            };

            Controllers.Add(new ComboAndSlider(groupBox1, Reverbs, (Obj) =>
            {
                var This = (ComboAndSlider)Obj;

                NSX39.GetInstance().Reverb(This.Item.Data, This.Value);
            }
            ));


            var Choruses = new List<ComboItem>
            {
                new ComboItem() { Key = "off", Data = new byte[] { 0x00, 0x00 }},

                new ComboItem() { Key = "chorus1", Data = new byte[] { 0x42, 0x11 }},
                new ComboItem() { Key = "chorus2", Data = new byte[] { 0x42, 0x08 }},
                new ComboItem() { Key = "chorus3", Data = new byte[] { 0x42, 0x10 }},
                new ComboItem() { Key = "chorus4", Data = new byte[] { 0x42, 0x01 }},
                new ComboItem() { Key = "chorus5", Data = new byte[] { 0x41, 0x02 }},
                new ComboItem() { Key = "chorus6", Data = new byte[] { 0x41, 0x00 }},
                new ComboItem() { Key = "chorus7", Data = new byte[] { 0x41, 0x01 }},
                new ComboItem() { Key = "chorus8", Data = new byte[] { 0x41, 0x08 }},
                new ComboItem() { Key = "gm_chorus1", Data = new byte[] { 0x41, 0x03 }},
                new ComboItem() { Key = "gm_chorus2", Data = new byte[] { 0x41, 0x04 }},
                new ComboItem() { Key = "gm_chorus3", Data = new byte[] { 0x41, 0x05 }},
                new ComboItem() { Key = "gm_chorus4", Data = new byte[] { 0x41, 0x06 }},
                new ComboItem() { Key = "fb_chorus", Data = new byte[] { 0x41, 0x07 }},

                new ComboItem() { Key = "celeste1", Data = new byte[] { 0x42, 0x00 }},
                new ComboItem() { Key = "celeste2", Data = new byte[] { 0x42, 0x02 }},

                new ComboItem() { Key = "flanger1", Data = new byte[] { 0x43, 0x08 }},
                new ComboItem() { Key = "flanger2", Data = new byte[] { 0x43, 0x10 }},
                new ComboItem() { Key = "flanger3", Data = new byte[] { 0x43, 0x11 }},
                new ComboItem() { Key = "flanger4", Data = new byte[] { 0x43, 0x01 }},

                new ComboItem() { Key = "flanger5", Data = new byte[] { 0x43, 0x00 }},
                new ComboItem() { Key = "gm_flanger2", Data = new byte[] { 0x43, 0x07 }},

                new ComboItem() { Key = "symphonic1", Data = new byte[] { 0x44, 0x10 }},
                new ComboItem() { Key = "symphonic2", Data = new byte[] { 0x44, 0x00 }},
                new ComboItem() { Key = "symphonic3", Data = new byte[] { 0x44, 0x12 }},
            };

            Controllers.Add(new ComboAndSlider(groupBox2, Choruses, (Obj) =>
            {
                var This = (ComboAndSlider)Obj;

                NSX39.GetInstance().Chorus(This.Item.Data, This.Value);
            }
            ));


            var VarEffects = new List<ComboItem>
            {
                new ComboItem() {Key = "HALL1", Data = new byte[] {1, 0}},
                new ComboItem() {Key = "HALL2", Data = new byte[] {1, 16}},
                new ComboItem() {Key = "HALL3", Data = new byte[] {1, 17}},
                new ComboItem() {Key = "HALL4", Data = new byte[] {1, 18}},
                new ComboItem() {Key = "HALL5", Data = new byte[] {1, 1}},
                new ComboItem() {Key = "HALL_M", Data = new byte[] {1, 6}},
                new ComboItem() {Key = "HALL_L", Data = new byte[] {1, 7}},
                new ComboItem() {Key = "ROOM1", Data = new byte[] {2, 16}},
                new ComboItem() {Key = "ROOM2", Data = new byte[] {2, 17}},
                new ComboItem() {Key = "ROOM3", Data = new byte[] {2, 18}},
                new ComboItem() {Key = "ROOM4", Data = new byte[] {2, 19}},
                new ComboItem() {Key = "ROOM5", Data = new byte[] {2, 0}},
                new ComboItem() {Key = "ROOM6", Data = new byte[] {2, 1}},
                new ComboItem() {Key = "ROOM7", Data = new byte[] {2, 2}},
                new ComboItem() {Key = "ROOM_S", Data = new byte[] {2, 5}},
                new ComboItem() {Key = "ROOM_M", Data = new byte[] {2, 6}},
                new ComboItem() {Key = "ROOM_L", Data = new byte[] {2, 7}},
                new ComboItem() {Key = "STAGE1", Data = new byte[] {3, 16}},
                new ComboItem() {Key = "STAGE2", Data = new byte[] {3, 17}},
                new ComboItem() {Key = "STAGE3", Data = new byte[] {3, 0}},
                new ComboItem() {Key = "STAGE4", Data = new byte[] {3, 1}},
                new ComboItem() {Key = "PLATE1", Data = new byte[] {4, 16}},
                new ComboItem() {Key = "PLATE2", Data = new byte[] {4, 17}},
                new ComboItem() {Key = "PLATE3", Data = new byte[] {4, 0}},
                new ComboItem() {Key = "GM_PLATE", Data = new byte[] {4, 7}},
                new ComboItem() {Key = "WHITE_ROOM", Data = new byte[] {16, 0}},
                new ComboItem() {Key = "TUNNEL_Simulates", Data = new byte[] {17, 0}},
                new ComboItem() {Key = "CANYON_A", Data = new byte[] {18, 0}},
                new ComboItem() {Key = "BASEMENT_A", Data = new byte[] {19, 0}},
                new ComboItem() {Key = "DELAY_LCR1", Data = new byte[] {5, 16}},
                new ComboItem() {Key = "DELAY_LCR2", Data = new byte[] {5, 0}},
                new ComboItem() {Key = "DELAY_LR", Data = new byte[] {6, 0}},
                new ComboItem() {Key = "ECHO_Two", Data = new byte[] {7, 0}},
                new ComboItem() {Key = "CROSS_DELAY", Data = new byte[] {8, 0}},
                new ComboItem() {Key = "TEMPO_DELAY", Data = new byte[] {21, 0}},
                new ComboItem() {Key = "TEMPO_ECHO", Data = new byte[] {21, 8}},
                new ComboItem() {Key = "TEMPO_CROSS", Data = new byte[] {22, 0}},
                new ComboItem() {Key = "KARAOKE1_20", Data = new byte[] {20, 0}},
                new ComboItem() {Key = "KARAOKE2_Echo", Data = new byte[] {20, 1}},
                new ComboItem() {Key = "KARAOKE3_20", Data = new byte[] {20, 2}},
                new ComboItem() {Key = "ER1_9", Data = new byte[] {9, 0}},
                new ComboItem() {Key = "ER2_9", Data = new byte[] {9, 1}},
                new ComboItem() {Key = "GATE_REVERB", Data = new byte[] {10, 0}},
                new ComboItem() {Key = "REVERS_GATE", Data = new byte[] {11, 0}},
                new ComboItem() {Key = "CHORUS1", Data = new byte[] {66, 17}},
                new ComboItem() {Key = "CHORUS2", Data = new byte[] {66, 8}},
                new ComboItem() {Key = "CHORUS3", Data = new byte[] {66, 16}},
                new ComboItem() {Key = "CHORUS4", Data = new byte[] {66, 1}},
                new ComboItem() {Key = "CHORUS5", Data = new byte[] {65, 2}},
                new ComboItem() {Key = "CHORUS6", Data = new byte[] {65, 0}},
                new ComboItem() {Key = "CHORUS7", Data = new byte[] {65, 1}},
                new ComboItem() {Key = "CHORUS8", Data = new byte[] {65, 8}},
                new ComboItem() {Key = "GM_CHORUS1", Data = new byte[] {65, 3}},
                new ComboItem() {Key = "GM_CHORUS2", Data = new byte[] {65, 4}},
                new ComboItem() {Key = "GM_CHORUS3", Data = new byte[] {65, 5}},
                new ComboItem() {Key = "GM_CHORUS4", Data = new byte[] {65, 6}},
                new ComboItem() {Key = "FB_CHORUS", Data = new byte[] {0, 0}},
                new ComboItem() {Key = "CELESTE1", Data = new byte[] {66, 0}},
                new ComboItem() {Key = "CELESTE2", Data = new byte[] {66, 2}},
                new ComboItem() {Key = "SYMPHONIC1", Data = new byte[] {68, 16}},
                new ComboItem() {Key = "SYMPHONIC2", Data = new byte[] {68, 0}},
                new ComboItem() {Key = "ENS", Data = new byte[] {87, 0}},
                new ComboItem() {Key = "FLANGER1", Data = new byte[] {67, 8}},
                new ComboItem() {Key = "FLANGER2", Data = new byte[] {67, 16}},
                new ComboItem() {Key = "FLANGER3", Data = new byte[] {67, 17}},
                new ComboItem() {Key = "FLANGER4", Data = new byte[] {67, 1}},
                new ComboItem() {Key = "FLANGER5", Data = new byte[] {67, 0}},
                new ComboItem() {Key = "GM_FLANGER", Data = new byte[] {67, 7}},
                new ComboItem() {Key = "T_FLANGER", Data = new byte[] {107, 0}},
                new ComboItem() {Key = "PHASER1", Data = new byte[] {72, 0}},
                new ComboItem() {Key = "PHASER2", Data = new byte[] {72, 8}},
                new ComboItem() {Key = "EP_PHASER2", Data = new byte[] {72, 18}},
                new ComboItem() {Key = "EP_PHASER3", Data = new byte[] {72, 16}},
                new ComboItem() {Key = "T_PHASER", Data = new byte[] {108, 0}},
                new ComboItem() {Key = "DIST_HEAVY", Data = new byte[] {73, 0}},
                new ComboItem() {Key = "ST_DIST", Data = new byte[] {73, 8}},
                new ComboItem() {Key = "COMP+DIST1", Data = new byte[] {73, 16}},
                new ComboItem() {Key = "COMP+DIST2", Data = new byte[] {73, 1}},
                new ComboItem() {Key = "OVERDRIVE", Data = new byte[] {74, 0}},
                new ComboItem() {Key = "ST_OD", Data = new byte[] {74, 8}},
                new ComboItem() {Key = "DIST_HARD1", Data = new byte[] {75, 16}},
                new ComboItem() {Key = "DIST_HARD2", Data = new byte[] {75, 22}},
                new ComboItem() {Key = "DIST_SOFT1", Data = new byte[] {75, 17}},
                new ComboItem() {Key = "DIST_SOFT2", Data = new byte[] {75, 23}},
                new ComboItem() {Key = "ST_DIST", Data = new byte[] {75, 18}},
                new ComboItem() {Key = "ST_DIST", Data = new byte[] {75, 19}},
                new ComboItem() {Key = "V_DIST_HARD", Data = new byte[] {98, 0}},
                new ComboItem() {Key = "V_DIST_SOFT", Data = new byte[] {98, 2}},
                new ComboItem() {Key = "AMP_SIM1", Data = new byte[] {75, 0}},
                new ComboItem() {Key = "AMP_SIM2", Data = new byte[] {75, 1}},
                new ComboItem() {Key = "ST_AMP1", Data = new byte[] {75, 20}},
                new ComboItem() {Key = "ST_AMP2", Data = new byte[] {75, 21}},
                new ComboItem() {Key = "ST_AMP3", Data = new byte[] {75, 8}},
                new ComboItem() {Key = "ST_AMP4", Data = new byte[] {75, 24}},
                new ComboItem() {Key = "ST_AMP5", Data = new byte[] {75, 25}},
                new ComboItem() {Key = "ST_AMP6", Data = new byte[] {75, 26}},
                new ComboItem() {Key = "DST+DELAY1", Data = new byte[] {95, 16}},
                new ComboItem() {Key = "DST+DELAY2", Data = new byte[] {95, 0}},
                new ComboItem() {Key = "OD+DELAY1", Data = new byte[] {95, 17}},
                new ComboItem() {Key = "OD+DELAY2", Data = new byte[] {95, 1}},
                new ComboItem() {Key = "CMP+DST+DLY1", Data = new byte[] {96, 16}},
                new ComboItem() {Key = "CMP+DST+DLY2", Data = new byte[] {96, 0}},
                new ComboItem() {Key = "CMP+OD+DLY1", Data = new byte[] {96, 17}},
                new ComboItem() {Key = "CMP+OD+DLY2", Data = new byte[] {96, 1}},
                new ComboItem() {Key = "V_DST", Data = new byte[] {98, 1}},
                new ComboItem() {Key = "V_DST", Data = new byte[] {98, 3}},
                new ComboItem() {Key = "DST+TDLY", Data = new byte[] {100, 0}},
                new ComboItem() {Key = "OD+TDLY", Data = new byte[] {100, 1}},
                new ComboItem() {Key = "CMP+DST+TDL", Data = new byte[] {101, 0}},
                new ComboItem() {Key = "CMP+OD+TDLY1", Data = new byte[] {101, 1}},
                new ComboItem() {Key = "CMP+OD+TDLY2", Data = new byte[] {101, 16}},
                new ComboItem() {Key = "CMP+OD+TDLY3", Data = new byte[] {101, 17}},
                new ComboItem() {Key = "CMP+OD+TDLY4", Data = new byte[] {101, 18}},
                new ComboItem() {Key = "CMP+OD+TDLY5", Data = new byte[] {101, 19}},
                new ComboItem() {Key = "CMP+OD+TDLY6", Data = new byte[] {101, 20}},
                new ComboItem() {Key = "V_DST_H+TDLY1", Data = new byte[] {103, 0}},
                new ComboItem() {Key = "V_DST_S+TDL1", Data = new byte[] {103, 1}},
                new ComboItem() {Key = "PITCH_CHG1", Data = new byte[] {80, 16}},
                new ComboItem() {Key = "PITCH_CHG2", Data = new byte[] {80, 0}},
                new ComboItem() {Key = "PITCH_CHG3", Data = new byte[] {80, 1}},
                new ComboItem() {Key = "AUTO_WAH1", Data = new byte[] {78, 16}},
                new ComboItem() {Key = "AUTO_WAH2", Data = new byte[] {78, 0}},
                new ComboItem() {Key = "AT_WAH+DST1", Data = new byte[] {78, 17}},
                new ComboItem() {Key = "AT_WAH+DST2", Data = new byte[] {78, 1}},
                new ComboItem() {Key = "AT_WAH+OD1", Data = new byte[] {78, 18}},
                new ComboItem() {Key = "AT_WAH+OD2", Data = new byte[] {78, 2}},
                new ComboItem() {Key = "TOUCH_WAH1", Data = new byte[] {82, 0}},
                new ComboItem() {Key = "TOUCH_WAH2", Data = new byte[] {82, 8}},
                new ComboItem() {Key = "TC_WAH+DST1", Data = new byte[] {82, 16}},
                new ComboItem() {Key = "TC_WAH+DST2", Data = new byte[] {82, 1}},
                new ComboItem() {Key = "TC_WAH+OD1", Data = new byte[] {82, 17}},
                new ComboItem() {Key = "TC_WAH+OD2", Data = new byte[] {82, 2}},
                new ComboItem() {Key = "CLAVI_TC", Data = new byte[] {82, 18}},
                new ComboItem() {Key = "EP_TC", Data = new byte[] {82, 19}},
                new ComboItem() {Key = "WH+DST+DLY1", Data = new byte[] {97, 16}},
                new ComboItem() {Key = "WH+DST+DLY2", Data = new byte[] {97, 0}},
                new ComboItem() {Key = "WH+DST+TDLY", Data = new byte[] {102, 0}},
                new ComboItem() {Key = "WH+OD+DLY1", Data = new byte[] {97, 17}},
                new ComboItem() {Key = "WH+OD+DLY2", Data = new byte[] {97, 1}},
                new ComboItem() {Key = "WH+OD+TDLY1", Data = new byte[] {102, 1}},
                new ComboItem() {Key = "WH+OD+TDLY2", Data = new byte[] {102, 16}},
                new ComboItem() {Key = "MBAND_COMP", Data = new byte[] {105, 0}},
                new ComboItem() {Key = "COMPRESSOR", Data = new byte[] {84, 0}},
                new ComboItem() {Key = "NOISE", Data = new byte[] {83, 0}},
                new ComboItem() {Key = "ROTARY_SP1", Data = new byte[] {69, 16}},
                new ComboItem() {Key = "ROTARY_SP2", Data = new byte[] {71, 17}},
                new ComboItem() {Key = "ROTARY_SP3", Data = new byte[] {71, 18}},
                new ComboItem() {Key = "ROTARY_SP4", Data = new byte[] {70, 17}},
                new ComboItem() {Key = "ROTARY_SP5", Data = new byte[] {66, 18}},
                new ComboItem() {Key = "ROTARY_SP6", Data = new byte[] {69, 0}},
                new ComboItem() {Key = "ROTARY_SP7", Data = new byte[] {71, 22}},
                new ComboItem() {Key = "2WAY_ROT", Data = new byte[] {86, 0}},
                new ComboItem() {Key = "DST+ROT_SP", Data = new byte[] {69, 1}},
                new ComboItem() {Key = "DST+2ROT_SP", Data = new byte[] {86, 1}},
                new ComboItem() {Key = "OD+ROT_SP", Data = new byte[] {69, 2}},
                new ComboItem() {Key = "OD+2ROT_SP", Data = new byte[] {86, 2}},
                new ComboItem() {Key = "AMP+ROT_SP", Data = new byte[] {69, 3}},
                new ComboItem() {Key = "AMP+2ROT_SP", Data = new byte[] {86, 3}},
                new ComboItem() {Key = "DUAL_ROT", Data = new byte[] {99, 0}},
                new ComboItem() {Key = "DUAL_ROT", Data = new byte[] {99, 1}},
                new ComboItem() {Key = "TREMOLO1_70", Data = new byte[] {70, 16}},
                new ComboItem() {Key = "TREMOLO2_71", Data = new byte[] {71, 19}},
                new ComboItem() {Key = "TREMOLO3_Rich", Data = new byte[] {70, 0}},
                new ComboItem() {Key = "EP_TREMOLO", Data = new byte[] {70, 18}},
                new ComboItem() {Key = "GT_TREMOLO1", Data = new byte[] {71, 20}},
                new ComboItem() {Key = "GT_TREMOLO2", Data = new byte[] {70, 19}},
                new ComboItem() {Key = "AUTO_PAN1", Data = new byte[] {71, 16}},
                new ComboItem() {Key = "AUTO_PAN2", Data = new byte[] {71, 0}},
                new ComboItem() {Key = "EP_AUTOPAN", Data = new byte[] {71, 21}},
                new ComboItem() {Key = "AUTO_PAN3", Data = new byte[] {71, 1}},
                new ComboItem() {Key = "EQ_DISCO", Data = new byte[] {76, 16}},
                new ComboItem() {Key = "EQ_TEL", Data = new byte[] {76, 17}},
                new ComboItem() {Key = "2BAND_EQ", Data = new byte[] {77, 0}},
                new ComboItem() {Key = "3BAND_EQ", Data = new byte[] {76, 0}},
                new ComboItem() {Key = "HM_ENHANCE1", Data = new byte[] {81, 16}},
                new ComboItem() {Key = "HM_ENHANCE2", Data = new byte[] {81, 0}},
                new ComboItem() {Key = "ST_3BAND", Data = new byte[] {76, 18}},
                new ComboItem() {Key = "VCE_CANCEL", Data = new byte[] {85, 0}},
                new ComboItem() {Key = "AMBIENCE_Blurs", Data = new byte[] {88, 0}},
                new ComboItem() {Key = "TALKING_MOD", Data = new byte[] {93, 0}},
                new ComboItem() {Key = "ISOLATOR_Controls", Data = new byte[] {115, 0}},
                new ComboItem() {Key = "NO_EFFECT", Data = new byte[] {0, 0}},
                new ComboItem() {Key = "THRU_Bypass", Data = new byte[] {64, 0}},
            };

            Controllers.Add(new ComboAndSlider(groupBox3, VarEffects, (Obj) =>
            {
                var This = (ComboAndSlider)Obj;

                NSX39.GetInstance().VariationEffect(This.Item.Data, This.Value);
            }
            ));


            PianoPane = new Piano(panel3, (NoteNo, NoteOn) =>
            {
                NSX39.GetInstance().Note(NoteOn, toolStripComboBox1.SelectedIndex, 12 * toolStripComboBox3.SelectedIndex + NoteNo, 127);
            });

            toolStripComboBox1.SelectedIndex = 0;
            toolStripComboBox3.SelectedIndex = 5;

            var Programs = new List<string> {
                "Acoustic Piano",
                "Bright Piano",
                "Electric Grand Piano",
                "Honky-tonk Piano",
                "Electric Piano",
                "Electric Piano 2",
                "Harpsichord",
                "Clavi",
                "Celesta",
                "Glockenspiel",
                "Musical box",
                "Vibraphone",
                "Marimba",
                "Xylophone",
                "Tubular Bell",
                "Dulcimer",
                "Drawbar Organ",
                "Percussive Organ",
                "Rock Organ",
                "Church organ",
                "Reed organ",
                "Accordion",
                "Harmonica",
                "Tango Accordion",
                "Acoustic Guitar (nylon)",
                "Acoustic Guitar (steel)",
                "Electric Guitar (jazz)",
                "Electric Guitar (clean)",
                "Electric Guitar (muted)",
                "Overdriven Guitar",
                "Distortion Guitar",
                "Guitar harmonics",
                "Acoustic Bass",
                "Electric Bass (finger)",
                "Electric Bass (pick)",
                "Fretless Bass",
                "Slap Bass 1",
                "Slap Bass 2",
                "Synth Bass 1",
                "Synth Bass 2",
                "Violin",
                "Viola",
                "Cello",
                "Double bass",
                "Tremolo Strings",
                "Pizzicato Strings",
                "Orchestral Harp",
                "Timpani",
                "String Ensemble 1",
                "String Ensemble 2",
                "Synth Strings 1",
                "Synth Strings 2",
                "Voice Aahs",
                "Voice Oohs",
                "Synth Voice",
                "Orchestra Hit",
                "Trumpet",
                "Trombone",
                "Tuba",
                "Muted Trumpet",
                "French horn",
                "Brass Section",
                "Synth Brass 1",
                "Synth Brass 2",
                "Soprano Sax",
                "Alto Sax",
                "Tenor Sax",
                "Baritone Sax",
                "Oboe",
                "English Horn",
                "Bassoon",
                "Clarinet",
                "Piccolo",
                "Flute",
                "Recorder",
                "Pan Flute",
                "Blown Bottle",
                "Shakuhachi",
                "Whistle",
                "Ocarina",
                "Lead 1 (square)",
                "Lead 2 (sawtooth)",
                "Lead 3 (calliope)",
                "Lead 4 (chiff)",
                "Lead 5 (charang)",
                "Lead 6 (voice)",
                "Lead 7 (fifths)",
                "Lead 8 (bass + lead)",
                "Pad 1 (Fantasia)",
                "Pad 2 (warm)",
                "Pad 3 (polysynth)",
                "Pad 4 (choir)",
                "Pad 5 (bowed)",
                "Pad 6 (metallic)",
                "Pad 7 (halo)",
                "Pad 8 (sweep)",
                "FX 1 (rain)",
                "FX 2 (soundtrack)",
                "FX 3 (crystal)",
                "FX 4 (atmosphere)",
                "FX 5 (brightness)",
                "FX 6 (goblins)",
                "FX 7 (echoes)",
                "FX 8 (sci-fi)",
                "Sitar",
                "Banjo",
                "Shamisen",
                "Koto",
                "Kalimba",
                "Bagpipe",
                "Fiddle",
                "Shanai",
                "Tinkle Bell",
                "Agogo",
                "Steel Drums",
                "Woodblock",
                "Taiko Drum",
                "Melodic Tom",
                "Synth Drum",
                "Reverse Cymbal",
                "Guitar Fret Noise",
                "Breath Noise",
                "Seashore",
                "Bird Tweet",
                "Telephone Ring",
                "Helicopter",
                "Applause",
                "Gunshot",
            };

            Controllers.Add(new ToolComboOnly(toolStripComboBox2, Programs, (Obj) =>
            {
                var This = (ToolComboOnly)Obj;

                NSX39.GetInstance().ProgramChange(1, This.Index);
            }));


            var Lyrics = new Dictionary<string, byte> {
                {"あ" , 0x00},
                {"い" , 0x01},
                {"う" , 0x02},
                {"え" , 0x03},
                {"お" , 0x04},

                {"か" , 0x05},
                {"き" , 0x06},
                {"く" , 0x07},
                {"け" , 0x08},
                {"こ" , 0x09},

                {"が" , 0x0A},
                {"ぎ" , 0x0B},
                {"ぐ" , 0x0C},
                {"げ" , 0x0D},
                {"ご" , 0x0E},

                {"きゃ" , 0x0F},
                {"きゅ" , 0x10},
                {"きょ" , 0x11},

                {"ぎゃ" , 0x12},
                {"ぎゅ" , 0x13},
                {"ぎょ" , 0x14},

                {"さ" , 0x15},
                {"すぃ" , 0x16},
                {"す" , 0x17},
                {"せ" , 0x18},
                {"そ" , 0x19},

                {"ざ" , 0x1A},
                {"ずぃ" , 0x1B},
                {"ず" , 0x1C},
                {"ぜ" , 0x1D},
                {"ぞ" , 0x1E},

                {"しゃ" , 0x1F},
                {"し" , 0x20},
                {"しゅ" , 0x21},
                {"しぇ" , 0x22},
                {"しょ" , 0x23},

                {"じゃ" , 0x24},
                {"じ" , 0x25},
                {"じゅ" , 0x26},
                {"じぇ" , 0x27},
                {"じょ" , 0x28},

                {"た" , 0x29},
                {"てぃ" , 0x2A},
                {"とぅ" , 0x2B},
                {"て" , 0x2C},
                {"と" , 0x2D},

                {"だ" , 0x2E},
                {"でぃ" , 0x2F},
                {"どぅ" , 0x30},
                {"で" , 0x31},
                {"ど" , 0x32},

                {"てゅ" , 0x33},
                {"でゅ" , 0x34},

                {"ちゃ" , 0x35},
                {"ち" , 0x36},
                {"ちゅ" , 0x37},
                {"ちぇ" , 0x38},
                {"ちょ" , 0x39},

                {"つぁ" , 0x3A},
                {"つぃ" , 0x3B},
                {"つ" , 0x3C},
                {"つぇ" , 0x3D},
                {"つぉ" , 0x3E},

                {"な" , 0x3F},
                {"に" , 0x40},
                {"ぬ" , 0x41},
                {"ね" , 0x42},
                {"の" , 0x43},

                {"にゃ" , 0x44},
                {"にゅ" , 0x45},
                {"にょ" , 0x46},

                {"は" , 0x47},
                {"ひ" , 0x48},
                {"ふ" , 0x49},
                {"へ" , 0x4A},
                {"ほ" , 0x4B},

                {"ば" , 0x4C},
                {"び" , 0x4D},
                {"ぶ" , 0x4E},
                {"べ" , 0x4F},
                {"ぼ" , 0x50},

                {"ぱ" , 0x51},
                {"ぴ" , 0x52},
                {"ぷ" , 0x53},
                {"ぺ" , 0x54},
                {"ぽ" , 0x55},

                {"ひゃ" , 0x56},
                {"ひゅ" , 0x57},
                {"ひょ" , 0x58},

                {"びゃ" , 0x59},
                {"びゅ" , 0x5A},
                {"びょ" , 0x5B},

                {"ぴゃ" , 0x5C},
                {"ぴゅ" , 0x5D},
                {"ぴょ" , 0x5E},

                {"ふぁ" , 0x5F},
                {"ふぃ" , 0x60},
                {"ふゅ" , 0x61},
                {"ふぇ" , 0x62},
                {"ふぉ" , 0x63},

                {"ま" , 0x64},
                {"み" , 0x65},
                {"む" , 0x66},
                {"め" , 0x67},
                {"も" , 0x68},

                {"みゃ" , 0x69},
                {"みゅ" , 0x6A},
                {"みょ" , 0x6B},

                {"や" , 0x6C},
                {"ゆ" , 0x6D},
                {"よ" , 0x6E},

                {"ら" , 0x6F},
                {"り" , 0x70},
                {"る" , 0x71},
                {"れ" , 0x72},
                {"ろ" , 0x73},

                {"りゃ" , 0x74},
                {"りゅ" , 0x75},
                {"りょ" , 0x76},

                {"わ" , 0x77},
                {"うぃ" , 0x78},
                {"うぇ" , 0x79},
                {"うぉ" , 0x7A},

                {"ん" , 0x7B},
                {"ん２" , 0x7C},
                {"ん３" , 0x7D},
                {"ん４" , 0x7E},
                {"ん５" , 0x7F}
            };


            Controllers.Add(new TextAndButton(panel1, (Obj) =>
            {
                var This = (TextAndButton)Obj;

                var Reg = new Regex("(?<Lyric>[あ-ん][ぁぃぅぇぉゃゅょ]?)");

                var Matches = Reg.Matches(This.Text);

                var LyricArray = new List<byte>();

                foreach (Match Mat in Matches)
                {
                    byte Tmp;

                    if (Lyrics.TryGetValue(Mat.Value, out Tmp))
                    {
                        LyricArray.Add(Tmp);
                    }
                    else
                    {
                        LyricArray.Add(Lyrics["ん"]);
                    }
                }

                NSX39.GetInstance().Lyrics(LyricArray.ToArray());
            }));
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            NSX39.GetInstance().Close();
        }

        private void button15_Click(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Resize(object sender, EventArgs e)
        {
            if (PianoPane != null)
            {
                PianoPane.ResizePianoKeys();
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }


    public class Piano
    {
        protected Panel BasePanel;
        protected Action<int, bool> NoteFunc;
        protected List<Button> WhiteKeys = new List<Button>();
        protected List<Button> BlackKeys = new List<Button>();
        protected bool TouchMode = false;

        protected const int Octaves = 2;


        public Piano(Panel ABasePanel, Action<int, bool> ANoteFunc)
        {
            BasePanel = ABasePanel;
            NoteFunc = ANoteFunc;

            CreatePianoKeys();
            ResizePianoKeys();
        }


        public void ResizePianoKeys()
        {
            int WhiteWidth = BasePanel.Width / (Octaves * 7);

            for (int i = 0; i < Octaves * 7; i++)
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

            for (int i = 0; i < Octaves * 7; i++)
            {
                var Key = new TouchSupportedButton();
                Key.BackColor = Color.White;
                Key.MouseDown += new MouseEventHandler(OnMouseDown);
                Key.MouseUp += new MouseEventHandler(OnMouseUp);
                WhiteKeys.Add(Key);
            }

            BasePanel.Controls.AddRange(WhiteKeys.ToArray());
        }


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


        protected void OnMouseDown(object sender, MouseEventArgs e)
        {
            FireNoteEvent(sender, e, true);
        }


        protected void OnMouseUp(object sender, MouseEventArgs e)
        {
            FireNoteEvent(sender, e, false);
        }
    }


    public class ComboItem
    {
        public string Key;
        public byte[] Data;

        public override string ToString()
        {
            return Key;
        }
    }


    public class Controller
    {
        protected Action<Controller> OnApply;

        public Controller(Action<Controller> AOnApply)
        {
            OnApply = AOnApply;
        }

        public void Apply()
        {
            OnApply.Invoke(this);
        }
    }


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


    public class ToolComboOnly : Controller
    {
        protected ToolStripComboBox Combo;

        public ToolComboOnly(ToolStripComboBox ACombo, List<string> Items, Action<Controller> AOnApply)
            : base(AOnApply)
        {
            Combo = ACombo;

            Initialize(Items);
        }


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


    public class ComboAndSlider : Controller
    {
        protected GroupBox Group;
        protected ComboBox Combo;
        protected TrackBar Slider;

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