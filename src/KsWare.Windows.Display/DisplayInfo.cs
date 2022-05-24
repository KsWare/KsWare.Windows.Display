using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using static KsWare.Windows.Native;

namespace KsWare.Windows {

	public partial class DisplayInfo {
		// analog to System.Windows.Forms.Screen https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.screen?view=windowsdesktop-6.0

		public DisplayInfo() {
			
		}

		/// <summary>Gets a value indicating whether the display settings changed since last update of th is instance.</summary>
		/// <value>   <c>true</c> if display settings changed; otherwise, <c>false</c>.</value>
		public bool HaveDisplaySettingsChanged { get; internal set; } 

		public string DeviceName { get; private set; } // \\.\DISPLAY1

		// public string DeviceString          // Intel(R) HD Graphics 630

		public bool IsPrimary { get; private set; }

		public Rectangle Bounds { get; private set; }

		public int Frequency { get; private set; }

		public int Orientation { get; private set; } //TODO use enum

		public int EffectiveDpiX { get; private set; }

		public int EffectiveDpiY { get; private set; }

		/// <summary>
		/// Gets the working area.
		/// A <see cref="Rectangle"/> that specifies the work area rectangle of the display monitor, expressed in virtual-screen coordinates.
		/// </summary>
		/// <value>The working area.</value>
		/// <remarks>Note that if the monitor is not the primary display monitor, some of the rectangle's coordinates may be negative values.</remarks>
		public Rectangle WorkingArea { get; private set; }

		internal IntPtr MonitorHandle { get; private set; }

		/// <summary>
		/// Gets the name of the monitor.
		/// </summary>
		/// <value>The name of the monitor or "MultiMonitor".</value>
		public string MonitorName { get; private set; }

		/// <summary>
		/// Gets the monitor information.
		/// </summary>
		/// <value>The monitor information.</value>
		public MonitorInfo[] MonitorInfo { get; private set; }

		internal void Import(DISPLAY_DEVICE displayDevice) {
			DeviceName = displayDevice.DeviceName;
			IsPrimary = (int) (displayDevice.StateFlags & DisplayDeviceStateFlags.PrimaryDevice) != 0;
		}

		internal void ImportDisplaySettings() {
			var devMode = Native.DEVMODE.Create();
			Native.EnumDisplaySettings(DeviceName, Native.ENUM_CURRENT_SETTINGS, ref devMode);
			Bounds = new Rectangle(devMode.dmPositionX, devMode.dmPositionY, devMode.dmPelsWidth, devMode.dmPelsHeight);
			Frequency = devMode.dmDisplayFrequency;
			Orientation = devMode.dmDisplayOrientation;
		}

		internal void UpdateMonitorDevices() {
			var devices = new List<DISPLAY_DEVICE>();
			var dd = DISPLAY_DEVICE.New();
			for (uint monitorIndex = 0; EnumDisplayDevices(DeviceName, monitorIndex, ref dd, 0); monitorIndex++) {
				// Debug.WriteLine($"    {monitorIndex}, {dd.DeviceName}, {dd.DeviceString}, {dd.StateFlags}, {dd.DeviceID}, {dd.DeviceKey}");
				devices.Add(dd);
				dd = DISPLAY_DEVICE.New();
			}

			if (devices.Count == 1) {
				MonitorName = devices[0].DeviceString;
			}
			else if (devices.Count > 1) {
				MonitorName = "MultiMonitor";
			}

			MonitorInfo = devices.Select(Windows.MonitorInfo.From).ToArray();
		}

		internal void ImportMonitorInfoEx(){
			if (MonitorHandle == IntPtr.Zero) MonitorHandle = MonitorFromPoint(Bounds.Location, MONITOR_DEFAULTTONULL);
			if (MonitorHandle == IntPtr.Zero) return;
			using (DpiBehavior.SetThreadDpiPerMonitorAwareV2()) {
				var mi = new MONITORINFOEX();
				mi.Size = Marshal.SizeOf(mi);
				bool success = GetMonitorInfo(MonitorHandle, ref mi);
				if (!success) return;

				_ = mi.Monitor;		// 
				WorkingArea = mi.WorkArea;
				_ = mi.Flags;		// 1 = MONITORINFOF_PRIMARY
				_ = mi.DeviceName;	// \\.\DISPLAY4
				//Availability = mi.flags.ToString();
			}
		}

		internal void ImportDpi() {
			// TODO

			// GetDpiFoThis API is not DPI aware and should not be used if the calling thread is per-monitor DPI aware.
			// For the DPI-aware version of this API, see GetDpiForWindow.
			if (MonitorHandle == IntPtr.Zero) MonitorHandle = MonitorFromPoint(Bounds.Location, MONITOR_DEFAULTTONULL);
			if (MonitorHandle == IntPtr.Zero) return;
			GetDpiForMonitor(MonitorHandle, Native.MONITOR_DPI_TYPE.EFFECTIVE_DPI, out var dpiX, out var dpiY);
			EffectiveDpiX = (int) dpiX;
			EffectiveDpiY = (int) dpiY;
		}



		internal void NotifyDisplaySettingsChanged() {
			HaveDisplaySettingsChanged = true;
			Array.ForEach(MonitorInfo,mi=>mi.NotifyDisplaySettingsChanged());
		}
	}
}