using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace LED_Controller.Interface
{
    public static class BioLEDInterface
    {
        private const string Mightex_BLSDriver_SDK_Filename = @"\Mightex_BLSDriver_SDK.dll";

        [DllImport(Mightex_BLSDriver_SDK_Filename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MTUSB_BLSDriverInitDevices();

        [DllImport(Mightex_BLSDriver_SDK_Filename, CharSet = CharSet.Ansi, CallingConvention= CallingConvention.Cdecl)]
        public static extern int MTUSB_BLSDriverOpenDevice(int DeviceIndex);

        [DllImport(Mightex_BLSDriver_SDK_Filename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MTUSB_BLSDriverCloseDevice(int DeviceHandle);

        [DllImport(Mightex_BLSDriver_SDK_Filename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MTUSB_BLSDriverGetSerialNo(int DeviceHandle, [MarshalAs(UnmanagedType.LPStr)] StringBuilder SerialNumber, int Size);

        [DllImport(Mightex_BLSDriver_SDK_Filename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MTUSB_BLSDriverGetChannels(int DeviceHandle);

        [DllImport(Mightex_BLSDriver_SDK_Filename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MTUSB_BLSDriverSetMode(int DeviceHandle, int Channel, int Mode);

        [DllImport(Mightex_BLSDriver_SDK_Filename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MTUSB_BLSDriverSetNormalCurrent(int DeviceHandle, int Channel, int Current);

//Profile MODES go here

        [DllImport(Mightex_BLSDriver_SDK_Filename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MTUSB_BLSDriverSetFollowModeDetail(int DeviceHandle, int Channel, int OnCurrent, int OffCurrent);

        [DllImport(Mightex_BLSDriver_SDK_Filename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MTUSB_BLSDriverSoftStart(int DeviceHandle, int Channel);

        [DllImport(Mightex_BLSDriver_SDK_Filename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MTUSB_BLSDriverResetDevice(int DeviceHandle);

        [DllImport(Mightex_BLSDriver_SDK_Filename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MTUSB_BLSDriverStorePara(int DeviceHandle);
    }
}
