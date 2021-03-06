using System.Runtime.InteropServices;

namespace KsWare.Windows {

	internal static partial class Native {

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		internal struct DISPLAY_DEVICE {

			[MarshalAs(UnmanagedType.U4)] public int cb;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string DeviceName;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string DeviceString;

			[MarshalAs(UnmanagedType.U4)] 
			public DisplayDeviceStateFlags StateFlags;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string DeviceID;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string DeviceKey;

			public static DISPLAY_DEVICE New() {
				var dd = new DISPLAY_DEVICE();
				dd.cb = Marshal.SizeOf(dd);
				return dd;
			}

		}
	}
}