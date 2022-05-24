using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;


namespace KsWare.Windows.Internal {

	internal class Helper {

		public static DisplayInfo[] GetAllDisplays() {

			var devices = new List<Native.DISPLAY_DEVICE>();
			{
				var dd = Native.DISPLAY_DEVICE.New();
				for (uint id = 0; Native.EnumDisplayDevices(null, id, ref dd, 0); id++) {
					// Debug.WriteLine($"{id}, {dd.DeviceName}, {dd.DeviceString}, {dd.StateFlags}, {dd.DeviceID}, {dd.DeviceKey}");
					devices.Add(dd);
					dd = Native.DISPLAY_DEVICE.New();
				}
			}

			var displays = new List<DisplayInfo>();


			foreach (var displayDevice in devices.Where(d => (int)(d.StateFlags & Native.DisplayDeviceStateFlags.AttachedToDesktop) > 0)) {
				var di = new DisplayInfo();
				di.Import(displayDevice);
				di.UpdateMonitorDevices();
				di.ImportMonitorInfoEx();
				di.ImportDisplaySettings();
				di.ImportDpi();

				displays.Add(di);
			}

			return displays.ToArray();
		}

		public static DisplayInfo[] GetAllDisplaysFromRect(Rectangle rect) {
			var r = Native.RECT.From(rect);
			var allDisplays = DisplayInfo.Displays.ToDictionary(d => d.Bounds, d => d);
			var result = new List<DisplayInfo>();
			using (DpiBehavior.SetThreadDpiPerMonitorAwareV2()) {
				Native.EnumDisplayMonitors(IntPtr.Zero, ref r,
					delegate (IntPtr hMonitor, IntPtr hdcMonitor, ref Native.RECT lprcMonitor, IntPtr dwData) {
						var mi = Native.MONITORINFOEX.New();
						if (!Native.GetMonitorInfo(hMonitor, ref mi)) return true; // oh why?
						if (!allDisplays.TryGetValue(mi.Monitor.ToRectangle(), out var di)) return true; // oh why?
						result.Add(di);
						return true;
					}, IntPtr.Zero);
			}
			return result.ToArray();
		}

	}

}
