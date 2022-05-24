using KsWare.Windows.Internal;
using System;
using System.Drawing;
using System.Linq;
using System.Windows;
using static KsWare.Windows.Native;

namespace KsWare.Windows {

	// Static part of DisplayInfo
	partial class DisplayInfo {
		/*
			\\.\DISPLAY1,			Intel(R) HD Graphics 630,	AttachedToDesktop, ModesPruned
			\\.\DISPLAY1\Monitor0,	Dell U2412M(Digital),		AttachedToDesktop, MultiDriver
			\\.\DISPLAY4,			NVIDIA Quadro P3000,		AttachedToDesktop, PrimaryDevice
			\\.\DISPLAY4\Monitor0,	Generic PnP Monitor,		AttachedToDesktop, MultiDriver,
		*/

		static DisplayInfo() {
			Microsoft.Win32.SystemEvents.DisplaySettingsChanged+=SystemEventsOnDisplaySettingsChanged;
			Update();
		}

		private static void SystemEventsOnDisplaySettingsChanged(object sender, EventArgs e) {
			var oldDisplays = Displays;
			Update();
			if (oldDisplays != null) Array.ForEach(oldDisplays, d => d.HaveDisplaySettingsChanged = true);
			DisplaySettingsChanged?.Invoke(sender,e);
		}

		private static void Update() {
			Displays = Helper.GetAllDisplays();
		}

		/// <inheritdoc cref="Microsoft.Win32.SystemEvents.DisplaySettingsChanged"/>
		public static event EventHandler DisplaySettingsChanged;

		public static DisplayInfo[] Displays { get; private set; }

		// public static DisplayInfo FromControl(Control control);

		// public static DisplayInfo FromHandle(IntPtr hwnd);

		// public static DisplayInfo FromPoint(System.Drawing.Point point);

		// // TODO continue here
		// public DisplayInfo FromRectangle(Rectangle rect) {
		// 	var r = RECT.From(rect);
		// 	var hMonitor = MonitorFromRect(RECT.From(rect), MONITOR_DEFAULTTONULL);
		// 	var GetMon
		// 	var allDisplays = GetAllDisplays();
		// 	allDisplays.First(d => d.Bounds == hMonitor);
		// 	return null;
		// }

		public static DisplayInfo[] AllFromRect(Rectangle rect) => Helper.GetAllDisplaysFromRect(rect);

		// public static Rectangle GetBounds(Control ctl);

		// public static Rectangle GetBounds(Point pt);

		// public static Rectangle GetBounds(Rectangle r)

		/// <summary>
		/// Gets the primary display.
		/// </summary>
		/// <returns>DisplayInfo.</returns>
		public static DisplayInfo GetPrimaryDisplay() => Displays.First(d => d.IsPrimary);

		public static bool IsCompleteOnVirtualDisplay(Window w) {
			var r = Native.GetWindowPlacement(w).NormalPosition;
			var windowBounds = new Rectangle(r.Left, r.Top, r.Width, r.Height);
			return IsCompleteOnVirtualDisplay(windowBounds);
		}

		/// <summary>
		/// Determines whether the specified rectangle is complete on virtual display.
		/// </summary>
		/// <param name="rectangle">The rectangle.</param>
		/// <returns><c>true</c> if [is complete on virtual display] [the specified rectangle]; otherwise, <c>false</c>.</returns>
		public static bool IsCompleteOnVirtualDisplay(Rectangle rectangle) {
			var displays = (DisplayInfo[])Displays.Clone();
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

	}

}