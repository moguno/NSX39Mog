using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NSX39Mog
{
    /// <summary>
    /// ポケミク制御用クラス
    /// </summary>
    class NSX39
    {
        protected static NSX39 MikuTan = null;
        protected OutputDevice MikuOutDev = null;
        protected const int CHANNELS = 2;

        /// <summary>
        /// シングルトンにしたいのでprotectedのコンストラクタ
        /// </summary>
        protected NSX39()
        {
        }

        /// <summary>
        /// インスタンスを生成する
        /// </summary>
        /// <returns>インスタンス</returns>
        public static NSX39 GetInstance()
        {
            if (MikuTan == null)
            {
                MikuTan = new NSX39();
            }

            return MikuTan;
        }

        /// <summary>
        /// 歌詞を送信する
        /// </summary>
        /// <param name="Lyrics">歌詞コードの配列</param>
        public void Lyrics(byte[] Lyrics)
        {
            if (!IsActive)
            {
                return;
            }

            var LyricEx = new List<byte>();
            LyricEx.AddRange(new byte[] { 0xf0, 0x43, 0x79, 0x09, 0x11, 0x0a, 0x00 });
            LyricEx.AddRange(Lyrics);
            LyricEx.Add(0xf7);

            var ExMsg = new SysExMessage(LyricEx.ToArray());

            MikuOutDev.Send(ExMsg);
        }


        /// <summary>
        /// 音符データを送る
        /// </summary>
        /// <param name="NoteOn">true：ノートON false:ノートOFF</param>
        /// <param name="Channel">MIDIチャンネル</param>
        /// <param name="NoteNo">MIDIノート番号</param>
        /// <param name="Velocity">ベロシティ</param>
        public void Note(bool NoteOn, int Channel, int NoteNo, int Velocity)
        {
            if (!IsActive)
            {
                return;
            }

            var NoteMsg = new ChannelMessage(NoteOn ? ChannelCommand.NoteOn : ChannelCommand.NoteOff, Channel, NoteNo, Velocity);
            MikuOutDev.Send(NoteMsg);
        }


        /// <summary>
        /// プログラムチェンジを送る
        /// </summary>
        /// <param name="Channel">MIDIチャンネル</param>
        /// <param name="Program">楽器番号</param>
        public void ProgramChange(int Channel, short Program)
        {
            if (!IsActive)
            {
                return;
            }

            var PChange = new ChannelMessage(ChannelCommand.ProgramChange, Channel, Program);
            MikuOutDev.Send(PChange);
        }


        /// <summary>
        /// ポケミクをリセットする
        /// </summary>
        public void Reset()
        {
            if (!IsActive)
            {
                return;
            }

            /* All Parameter Reset */
            var AllParamReset = new SysExMessage(new byte[] { 0xf0, 0x43, 0x10, 0x4c, 0x00, 0x00, 0x7f, 0x00, 0xf7 });

            MikuOutDev.Send(AllParamReset);


            /* XG System ON */
            var XGOn = new SysExMessage(new byte[] { 0xf0, 0x43, 0x10, 0x4c, 0x00, 0x00, 0x7e, 0x00, 0xf7 });

            MikuOutDev.Send(XGOn);
        }


        /// <summary>
        /// ポケミクが接続中か？
        /// </summary>
        public bool IsActive
        {
            get
            {
                return (MikuOutDev != null);
            }
        }


        /// <summary>
        /// ポケミクを閉じる
        /// </summary>
        public void Close()
        {
            if (MikuOutDev != null)
            {
                MikuOutDev.Close();

                MikuOutDev = null;
            }
        }


        /// <summary>
        /// ポケミクのMIDI IDを探す
        /// </summary>
        /// <returns></returns>
        public int GetMikuID()
        {
            int Found = -1;

            for (int i = 0; i < OutputDevice.DeviceCount; i++)
            {
                Debug.WriteLine(OutputDevice.GetDeviceCapabilities(i).name);
                if (OutputDevice.GetDeviceCapabilities(i).name.Equals("NSX-39 "))
                {
                    Debug.WriteLine("Miku detected.");

                    Found = i;

                    break;
                }
            }

            return Found;
        }


        /// <summary>
        /// リバーブを送る
        /// </summary>
        /// <param name="Type">種別コード</param>
        /// <param name="Depth">強さ</param>
        public void Reverb(byte[] Type, short Depth)
        {
            if (!IsActive)
            {
                return;
            }

            var ReverbEx = new List<byte>();

            ReverbEx.AddRange(new byte[] { 0xf0, 0x43, 0x10, 0x4c, 0x02, 0x01, 0x00 });
            ReverbEx.AddRange(Type);
            ReverbEx.Add(0xf7);

            var ExMsg = new SysExMessage(ReverbEx.ToArray());

            MikuOutDev.Send(ExMsg);

            for (int i = 0; i < CHANNELS; i++)
            {
                var DepthMsg = new ChannelMessage(ChannelCommand.Controller, i, 91, Depth);

                MikuOutDev.Send(DepthMsg);
            }
        }


        /// <summary>
        /// コーラスを送る
        /// </summary>
        /// <param name="Type">種別コード</param>
        /// <param name="Depth">強さ</param>
        public void Chorus(byte[] Type, short Depth)
        {
            if (!IsActive)
            {
                return;
            }

            var ChorusEx = new List<byte>();

            ChorusEx.AddRange(new byte[] { 0xf0, 0x43, 0x10, 0x4c, 0x02, 0x01, 0x20 });
            ChorusEx.AddRange(Type);
            ChorusEx.Add(0xf7);

            var ExMsg = new SysExMessage(ChorusEx.ToArray());

            MikuOutDev.Send(ExMsg);

            for (int i = 0; i < CHANNELS; i++)
            {
                var DepthMsg = new ChannelMessage(ChannelCommand.Controller, i, 93, Depth);

                MikuOutDev.Send(DepthMsg);
            }
        }


        /// <summary>
        /// バリエーションエフェクトを送る
        /// </summary>
        /// <param name="Type">種別コード</param>
        /// <param name="Depth">強さ</param>
        public void VariationEffect(byte[] Type, short Depth)
        {
            if (!IsActive)
            {
                return;
            }

            var SystemEffectEx = new SysExMessage(new byte[] { 0xf0, 0x43, 0x10, 0x4c, 0x02, 0x01, 0x5a, 0x01, 0xf7 });

            MikuOutDev.Send(SystemEffectEx);


            var VariationEx = new List<byte>();

            VariationEx.AddRange(new byte[] { 0xf0, 0x43, 0x10, 0x4c, 0x02, 0x01, 0x40 });
            VariationEx.AddRange(Type);
            VariationEx.Add(0xf7);

            var ExMsg = new SysExMessage(VariationEx.ToArray());

            MikuOutDev.Send(ExMsg);

            for (int i = 0; i < CHANNELS; i++)
            {
                var DepthMsg = new ChannelMessage(ChannelCommand.Controller, i, 94, Depth);

                MikuOutDev.Send(DepthMsg);
            }
        }


        /// <summary>
        /// ポケミクが繋がったらデバイスを開く
        /// </summary>
        /// <param name="OpenFunc">デバイスを開いた後にやる処理</param>
        public void PlugAndPlay(Action OpenFunc = null)
        {
            if (GetMikuID() == -1)
            {
                if (MikuOutDev != null)
                {
                    Close();
                }

                return;
            }

            if (MikuOutDev == null)
            {
                Open();

                if (OpenFunc != null)
                {
                    OpenFunc.Invoke();
                }
            }
        }


        /// <summary>
        /// ポケミクを開く
        /// </summary>
        /// <param name="ID">ポケミクのID。-1の場合探索する</param>
        public void Open(int ID = -1)
        {
            if (ID == -1)
            {
                ID = GetMikuID();
            }

            if (ID != -1)
            {
                MikuOutDev = new OutputDevice(ID);
            }
        }
    }
}
