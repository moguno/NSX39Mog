using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace WindowsFormsApplication2
{
    class NSX39
    {
        protected static NSX39 MikuTan = null;
        protected OutputDevice MikuOutDev = null;

        protected NSX39()
        {
        }

        public static NSX39 GetInstance()
        {
            if (MikuTan == null)
            {
                MikuTan = new NSX39();
            }

            return MikuTan;
        }

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

        public void Note(bool NoteOn, int Channel, int NoteNo, int Velocity)
        {
            if (!IsActive)
            {
                return;
            }

            var NoteMsg = new ChannelMessage(NoteOn ? ChannelCommand.NoteOn : ChannelCommand.NoteOff, Channel, NoteNo, Velocity);
            MikuOutDev.Send(NoteMsg);
        }

        public void ProgramChange(int Channel, short Program)
        {
            if (!IsActive)
            {
                return;
            }

            var PChange = new ChannelMessage(ChannelCommand.ProgramChange, Channel, Program);
            MikuOutDev.Send(PChange);
        }

        public void Reset()
        {
            if (!IsActive)
            {
                return;
            }

            /* XG System ON */
            var XGOn = new SysExMessage(new byte[] { 0xf0, 0x43, 0x10, 0x4c, 0x00, 0x00, 0x7e, 0x00, 0xf7 });

            MikuOutDev.Send(XGOn);
        }

        public bool IsActive
        {
            get
            {
                return (MikuOutDev != null);
            }
        }

        public void Close()
        {
            if (MikuOutDev != null)
            {
                MikuOutDev.Close();

                MikuOutDev = null;
            }
        }


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

            var DepthMsg = new ChannelMessage(ChannelCommand.Controller, 0, 91, Depth);

            MikuOutDev.Send(DepthMsg);
        }


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

            var DepthMsg = new ChannelMessage(ChannelCommand.Controller, 0, 93, Depth);

            MikuOutDev.Send(DepthMsg);
        }


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

            var DepthMsg = new ChannelMessage(ChannelCommand.Controller, 0, 94, Depth);

            MikuOutDev.Send(DepthMsg);
        }


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
