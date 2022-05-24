using System;

namespace KsWare.Windows {

	public sealed class MonitorInfo {

		internal Native.DISPLAY_DEVICE NativeData { get; private set; }

		/// <summary>Gets a value indicating whether the display settings changed since last update of th is instance.</summary>
		/// <value>   <c>true</c> if display settings changed; otherwise, <c>false</c>.</value>
		public bool HaveDisplaySettingsChanged { get; private set; } 

		public string DeviceName { get; private set; }

		public string DeviceString { get; private set; }

		// public DisplayDeviceStateFlags StateFlags { get; private set; }
		//
		// public string DeviceID { get; private set; }
		//
		// public string DeviceKey { get; private set; }

		internal static MonitorInfo From(Native.DISPLAY_DEVICE dd) {
			return new MonitorInfo {
				NativeData = dd,
				DeviceName = dd.DeviceName,			// \\.\DISPLAY1\Monitor0,			
				DeviceString = dd.DeviceString,		// Dell U2412M(Digital),
				// StateFlags = dd.StateFlags,		// AttachedToDesktop, MultiDriver
				// DeviceID = dd.DeviceID,			// MONITOR\DELA07A\{4d36e96e-e325-11ce-bfc1-08002be10318}\0004
				// DeviceKey = dd.DeviceKey			// \Registry\Machine\System\CurrentControlSet\Control\Class\{4d36e96e-e325-11ce-bfc1-08002be10318}\0004
			};
		}

		internal void NotifyDisplaySettingsChanged() {
			HaveDisplaySettingsChanged = true;
		}

	}

}