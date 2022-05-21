using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using static KsWare.Windows.Native;

namespace KsWare.Windows {

	public class DisplayInfo {
		// analog to System.Windows.Forms.Screen https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.screen?view=windowsdesktop-6.0

		#region Static

		public static DisplayInfo[] GetAllDisplays() {
			var displays = new List<DisplayInfo>();

			var devices = new List<DISPLAY_DEVICE>();
			{
				var d = new DISPLAY_DEVICE();
				d.cb = Marshal.SizeOf(d);
				for (uint id = 0; EnumDisplayDevices(null, id, ref d, 0); id++) {
					// Debug.WriteLine($"{id}, {d.DeviceName}, {d.DeviceString}, {d.StateFlags}, {d.DeviceID}, {d.DeviceKey}");
					devices.Add(d);
					d = new DISPLAY_DEVICE();
					d.cb = Marshal.SizeOf(d);
				}
			}

			var vDevMode = DEVMODE.Create();
			foreach (var displayDevice in devices.Where(d => (int) (d.StateFlags & DisplayDeviceStateFlags.AttachedToDesktop) > 0)) {
				var di = new DisplayInfo();
				di.Import(displayDevice);
				EnumDisplaySettings(displayDevice.DeviceName, ENUM_CURRENT_SETTINGS, ref vDevMode);
				di.Import(vDevMode);
				di.ImportDpi();
				di.ImportMonitorInfoEx();

				displays.Add(di);
			}

			return displays.ToArray();
		}

		// public static DisplayInfo FromControl(Control control);

		// public static DisplayInfo FromHandle(IntPtr hwnd);

		// public static DisplayInfo FromPoint(System.Drawing.Point point);

		// TODO continue here
		public DisplayInfo FromRectangle(Rectangle rect) {
			var r = RECT.From(rect);
			var hMonitor = MonitorFromRect(RECT.From(rect), MONITOR_DEFAULTTONULL);
			var allDisplayes = GetAllDisplays();
			allDisplayes.First(d => d.MonitorHandle == hMonitor);
			return null;
		}

		public static DisplayInfo[] AllFromRect(Rectangle rect) {
			var r=RECT.From(rect);
			var allDisplayes = GetAllDisplays();
			var result = new List<DisplayInfo>();
			Native.EnumDisplayMonitors(IntPtr.Zero, ref r,
				delegate (IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor,  IntPtr dwData) {
					// var d = allDisplayes.First(d => d.MonitorHandle == hMonitor);
					// result.Add(d);
					var mi = new MONITORINFOEX();
					mi.Size = Marshal.SizeOf(mi);
					GetMonitorInfo(hMonitor, ref mi);
					return true;
				}, IntPtr.Zero );
			return result.ToArray();
		}

		// public static Rectangle GetBounds(Control ctl);

		// public static Rectangle GetBounds(Point pt);

		// public static Rectangle GetBounds(Rectangle r)

		public static DisplayInfo GetPrimaryDisplay() => GetAllDisplays().First(d => d.IsPrimary);

		public static bool IsCompleteOnVirtualDisplay(Window w) {
			var r = Native.GetWindowPlacement(w).NormalPosition;
			var windowBounds = new Rectangle(r.Left, r.Top, r.Width, r.Height);
			return IsCompleteOnVirtualDisplay(windowBounds);
		}

		public static bool IsCompleteOnVirtualDisplay(Rectangle rectangle) {
			var displays = GetAllDisplays();
			var windowBounds = new Rect(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
			var windowArea = (double) windowBounds.Width * windowBounds.Height;
			var visibleArea = (double) 0.0;
			foreach (var display in displays) {
				var screenBounds = new Rect(display.Bounds.X, display.Bounds.Y, display.Bounds.Width,
					display.Bounds.Height);
				var intersect = windowBounds;
				intersect.Intersect(screenBounds);
				visibleArea += (double) intersect.Width * intersect.Height;
			}

			var visibleAreaPercent = 100.0 / windowArea * visibleArea;
			return visibleAreaPercent > 99.0;
		}

		#endregion Static

		public string DeviceName { get; private set; } // \\.\DISPLAY1

		// public string DeviceString          // Intel(R) HD Graphics 630

		public bool IsPrimary { get; private set; }

		public Rectangle Bounds { get; private set; }

		public int Frequency { get; private set; }

		public int Orientation { get; private set; } //TODO use enum

		public int EffectiveDpiX { get; private set; }

		public int EffectiveDpiY { get; private set; }

		public Rectangle WorkingArea { get; private set; }

		internal IntPtr MonitorHandle { get; private set; }

		internal void Import(DISPLAY_DEVICE displayDevice) {
			DeviceName = displayDevice.DeviceName;
			IsPrimary = (int) (displayDevice.StateFlags & DisplayDeviceStateFlags.PrimaryDevice) != 0;
		}

		internal void Import(DEVMODE devMode) {
			Bounds = new Rectangle(devMode.dmPositionX, devMode.dmPositionY, devMode.dmPelsWidth, devMode.dmPelsHeight);
			Frequency = devMode.dmDisplayFrequency;
			Orientation = devMode.dmDisplayOrientation;
		}

		internal void ImportMonitorInfoEx() {
			if (MonitorHandle == IntPtr.Zero) MonitorHandle = MonitorFromPoint(Bounds.Location, MONITOR_DEFAULTTONULL);
			if (MonitorHandle == IntPtr.Zero) return;
			var mi = new MONITORINFOEX();
			mi.Size = Marshal.SizeOf(mi);
			bool success = GetMonitorInfo(MonitorHandle, ref mi);
			if (success) {
				DisplayInfo di = new DisplayInfo();
				//di.MonitorArea = mi.Monitor;
				WorkingArea = mi.WorkArea;
				//Availability = mi.flags.ToString();
			}
		}

		internal void ImportDpi() {
			if (MonitorHandle == IntPtr.Zero) MonitorHandle = MonitorFromPoint(Bounds.Location, MONITOR_DEFAULTTONULL);
			if (MonitorHandle == IntPtr.Zero) return;
			GetDpiForMonitor(MonitorHandle, Native.MONITOR_DPI_TYPE.EFFECTIVE_DPI, out var dpiX, out var dpiY);
			EffectiveDpiX = (int) dpiX;
			EffectiveDpiY = (int) dpiY;
		}
	}
}