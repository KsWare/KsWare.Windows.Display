using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using static KsWare.Windows.Native;

namespace KsWare.Windows {

	public static class WindowExtensions {

		public static WindowPlacement GetPlacement(this Window w) 
			=> WindowPlacement.From(Native.GetWindowPlacement(w));

		public static void SetPlacement(this Window w, WindowPlacement placement)
			=> SetPlacement(w, placement.Bounds, placement.State);

		public static void SetPlacement(this Window w, Rectangle bounds, WindowState state) {
			var hWnd = new WindowInteropHelper(w).Handle;
			var placement = new WINDOWPLACEMENT();
			placement.Length = Marshal.SizeOf(placement);
			placement.NormalPosition = new Native.RECT(bounds.Left, bounds.Top, bounds.Right, bounds.Bottom);
			placement.MinPosition = new Native.POINT(-1, -1);
			placement.MaxPosition = new Native.POINT(-1, -1);
			switch (state) {
				case WindowState.Normal: placement.ShowCmd = Native.ShowCmd.SW_NORMAL; break;
				case WindowState.Minimized: placement.ShowCmd = Native.ShowCmd.SW_MINIMIZE; break;
				case WindowState.Maximized: placement.ShowCmd = Native.ShowCmd.SW_MAXIMIZE; break;
			}
			placement.Flags = WPF_ASYNCWINDOWPLACEMENT;
			Native.SetWindowPlacement(hWnd, ref placement);
		}

		public static int GetDpi(this Window w) {
			var hWnd = new WindowInteropHelper(w).Handle;
			return Native.GetDpiForWindow(hWnd);
		}
	}
}

