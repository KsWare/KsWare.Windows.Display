using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace KsWare.Windows {

	internal static partial class Native {

		internal static WINDOWPLACEMENT GetWindowPlacement(Window w) {
			var hWnd = new WindowInteropHelper(w).Handle;
			var placement = new WINDOWPLACEMENT();
			placement.Length = Marshal.SizeOf(placement);
			GetWindowPlacement(hWnd, ref placement);
			return placement;
		}

		internal static EnumDisplayMonitorResult[] GetDisplayMonitors(Rectangle rect, bool fillMonitorInfo=true) {
			var re=RECT.From(rect);
			var result = new List<EnumDisplayMonitorResult>();
			Native.EnumDisplayMonitors(IntPtr.Zero, ref re,
				delegate (IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor,  IntPtr dwData) {
					var r = new EnumDisplayMonitorResult();
					r.MonitorHandle=hMonitor;
					r.MonitorDC = hdcMonitor;
					r.Rect = lprcMonitor;
					if (fillMonitorInfo) {
						var mi = new MONITORINFOEX();
						mi.Size = Marshal.SizeOf(mi);
						GetMonitorInfo(hMonitor, ref mi);
						r.DeviceName=mi.DeviceName;
						r.Flags=mi.Flags;
						r.Monitor=mi.Monitor;
						r.WorkArea=mi.WorkArea;
					}
					result.Add(r);
					return true;
				}, IntPtr.Zero );
			return result.ToArray();
		}

		internal struct EnumDisplayMonitorResult {
			//from EnumMonitorsDelegate
			public IntPtr MonitorHandle { get; set; }	// hMonitor
			public IntPtr MonitorDC { get; set; }		// hdcMonitor
			public Rectangle Rect { get; set; }			// lprcMonitor

			// from MONITORINFOEX
			public string DeviceName { get; set; }
			public uint Flags { get; set; }
			public Rectangle Monitor { get; set; }
			public Rectangle WorkArea { get; set; }
		}

		internal const int ENUM_CURRENT_SETTINGS = -1;
		internal const int ENUM_REGISTRY_SETTINGS = -2;

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		internal static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum,
			ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

		[DllImport("user32.dll")]
		internal static extern bool EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);

		[Flags]
		internal enum DisplayDeviceStateFlags : int {
			/// <summary>The device is part of the desktop.</summary>
			AttachedToDesktop = 0x1,
			MultiDriver = 0x2,

			/// <summary>The device is part of the desktop.</summary>
			PrimaryDevice = 0x4,

			/// <summary>Represents a pseudo device used to mirror application drawing for remoting or other purposes.</summary>
			MirroringDriver = 0x8,

			/// <summary>The device is VGA compatible.</summary>
			VGACompatible = 0x10,

			/// <summary>The device is removable; it cannot be the primary display.</summary>
			Removable = 0x20,

			/// <summary>The device has more display modes than its output devices support.</summary>
			ModesPruned = 0x8000000,
			Remote = 0x4000000,
			Disconnect = 0x2000000
		}

		// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-monitorfrompoint
		[DllImport("user32")]
		internal static extern IntPtr MonitorFromPoint([In] System.Drawing.Point pt, [In] uint dwFlags);

		[DllImport("user32")]
		internal static extern IntPtr MonitorFromRect([In] RECT pt, [In] uint dwFlags);

		/// <remarks>https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-enumdisplaymonitors</remarks>
		[DllImport("user32.dll")]
		internal static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, EnumMonitorsDelegate lpfnEnum, IntPtr dwData);

		[DllImport("user32.dll")]
		internal static extern bool EnumDisplayMonitors(IntPtr hdc, ref RECT lprcClip, EnumMonitorsDelegate lpfnEnum, IntPtr dwData);

		/// <remarks>https://docs.microsoft.com/en-us/windows/win32/api/winuser/nc-winuser-monitorenumproc</remarks>
		internal delegate bool EnumMonitorsDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		internal static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFOEX lpmi);

		// [DllImport("user32.dll")]
		// static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

		// https://docs.microsoft.com/en-us/windows/win32/api/shellscalingapi/nf-shellscalingapi-getdpiformonitor
		[DllImport("shcore")]
		internal static extern IntPtr GetDpiForMonitor([In] IntPtr hmonitor, [In] MONITOR_DPI_TYPE dpiType, [Out] out uint dpiX, [Out] out uint dpiY);

		[DllImport("user32.dll")]
		internal static extern int GetDpiForWindow(IntPtr hWnd);

		/// <remarks>https://docs.microsoft.com/en-us/windows/win32/api/shellscalingapi/nf-shellscalingapi-getprocessdpiawareness</remarks>
		[DllImport("SHcore.dll")]
		internal static extern int GetProcessDpiAwareness(IntPtr hWnd, out PROCESS_DPI_AWARENESS value);

		/// <remarks>https://docs.microsoft.com/en-us/windows/win32/api/shellscalingapi/nf-shellscalingapi-setprocessdpiawareness</remarks>
		[DllImport("shcore.dll")]
		private static extern int SetProcessDpiAwareness(IntPtr value);

		public static int SetProcessDpiAwareness(PROCESS_DPI_AWARENESS value)
			=> SetProcessDpiAwareness((IntPtr) value);

		[DllImport("user32.dll")]
		internal static extern IntPtr GetWindowDpiAwarenessContext(IntPtr hWnd);

		[DllImport("user32.dll")]
		internal static extern int GetAwarenessFromDpiAwarenessContext(IntPtr dpiContext);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool AreDpiAwarenessContextsEqual(IntPtr dpiContextA, IntPtr dpiContextB);
		internal static bool AreDpiAwarenessContextsEqual(DPI_AWARENESS_CONTEXT dpiContextA, DPI_AWARENESS_CONTEXT dpiContextB)
			=> AreDpiAwarenessContextsEqual((IntPtr)dpiContextA, (IntPtr)dpiContextB);

		// #define DPI_AWARENESS_CONTEXT_UNAWARE              ((DPI_AWARENESS_CONTEXT)-1)
		// #define DPI_AWARENESS_CONTEXT_SYSTEM_AWARE         ((DPI_AWARENESS_CONTEXT)-2)
		// #define DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE    ((DPI_AWARENESS_CONTEXT)-3)
		// #define DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 ((DPI_AWARENESS_CONTEXT)-4)

		/// <remarks>https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setthreaddpihostingbehavior</remarks>
		[DllImport("user32.dll")]
		private static extern IntPtr SetThreadDpiHostingBehavior(IntPtr value);

		/// <summary>
		/// Sets the thread's <see cref="DPI_HOSTING_BEHAVIOR"/>. This behavior allows windows created in the thread to host child windows with a different DPI_AWARENESS_CONTEXT.
		/// </summary>
		/// <param name="value">The new <see cref="DPI_HOSTING_BEHAVIOR"/> value for the current thread.</param>
		/// <returns>The previous <see cref="DPI_HOSTING_BEHAVIOR"/> for the thread. If the hosting behavior passed in is invalid, the thread will not be updated and the return value will be DPI_HOSTING_BEHAVIOR_INVALID. You can use this value to restore the old DPI_HOSTING_BEHAVIOR after overriding it with a predefined value.</returns>
		/// <remarks>https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setthreaddpihostingbehavior</remarks>
		internal static DPI_HOSTING_BEHAVIOR SetThreadDpiHostingBehavior(DPI_HOSTING_BEHAVIOR value) 
			=> (DPI_HOSTING_BEHAVIOR) SetThreadDpiHostingBehavior((IntPtr) value);

		[DllImport("user32.dll", EntryPoint = "GetThreadDpiHostingBehavior")]
		private static extern IntPtr GetThreadDpiHostingBehaviorPointer();

		internal static DPI_HOSTING_BEHAVIOR GetThreadDpiHostingBehavior()
			=> (DPI_HOSTING_BEHAVIOR) GetThreadDpiHostingBehaviorPointer();

		/// <remarks>https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setthreaddpiawarenesscontext</remarks>
		[DllImport("user32.dll")]
		internal static extern IntPtr SetThreadDpiAwarenessContext(IntPtr dpiContext);
		internal static DPI_AWARENESS_CONTEXT SetThreadDpiAwarenessContext(DPI_AWARENESS_CONTEXT dpiContext)
			=> (DPI_AWARENESS_CONTEXT) SetThreadDpiAwarenessContext((IntPtr) dpiContext);

		/// <remarks>https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getthreaddpiawarenesscontext</remarks>
		[DllImport("user32.dll",EntryPoint = "GetThreadDpiAwarenessContext")]
		private static extern IntPtr GetThreadDpiAwarenessContextPointer();

		internal static DPI_AWARENESS_CONTEXT GetThreadDpiAwarenessContext(bool normalize = false) {
			var v = (DPI_AWARENESS_CONTEXT) GetThreadDpiAwarenessContextPointer();
			return normalize ? NormalizeThreadDpiAwarenessContext(v) : v;
		}

		internal static DPI_AWARENESS_CONTEXT NormalizeThreadDpiAwarenessContext(DPI_AWARENESS_CONTEXT ctx) {
			switch (true) {
				case true when AreDpiAwarenessContextsEqual(ctx, DPI_AWARENESS_CONTEXT.UNAWARE): return DPI_AWARENESS_CONTEXT.UNAWARE;
				case true when AreDpiAwarenessContextsEqual(ctx, DPI_AWARENESS_CONTEXT.SYSTEM_AWARE): return DPI_AWARENESS_CONTEXT.SYSTEM_AWARE;
				case true when AreDpiAwarenessContextsEqual(ctx, DPI_AWARENESS_CONTEXT.PER_MONITOR_AWARE): return DPI_AWARENESS_CONTEXT.PER_MONITOR_AWARE;
				case true when AreDpiAwarenessContextsEqual(ctx, DPI_AWARENESS_CONTEXT.PER_MONITOR_AWARE_V2): return DPI_AWARENESS_CONTEXT.PER_MONITOR_AWARE_V2;
				case true when AreDpiAwarenessContextsEqual(ctx, DPI_AWARENESS_CONTEXT.UNAWARE_GDI_SCALED): return DPI_AWARENESS_CONTEXT.UNAWARE_GDI_SCALED;
				default: return ctx;
			}
		}

		internal enum DPI_AWARENESS_CONTEXT : long {
			UNAWARE = -1,
			SYSTEM_AWARE = -2,
			PER_MONITOR_AWARE = -3,
			PER_MONITOR_AWARE_V2 = -4,
			UNAWARE_GDI_SCALED = -5,
		}
		
		/// <remarks>https://docs.microsoft.com/en-us/windows/win32/api/windef/ne-windef-dpi_hosting_behavior</remarks>
		internal enum DPI_HOSTING_BEHAVIOR {
			INVALID = -1, // DPI_HOSTING_BEHAVIOR_INVALID
			DEFAULT = 0, // DPI_HOSTING_BEHAVIOR_DEFAULT
			MIXED = 1 // DPI_HOSTING_BEHAVIOR_MIXED
		}

		internal enum PROCESS_DPI_AWARENESS {
			PROCESS_DPI_UNAWARE = 0,
			PROCESS_SYSTEM_DPI_AWARE = 1,
			PROCESS_PER_MONITOR_DPI_AWARE = 2
		}

		internal enum DPI_AWARENESS {
			INVALID = -1, // DPI_AWARENESS_INVALID
			UNAWARE = 0,	// DPI_AWARENESS_UNAWARE
			SYSTEM_AWARE = 1,	// DPI_AWARENESS_SYSTEM_AWARE
			PER_MONITOR_AWARE = 2 // DPI_AWARENESS_PER_MONITOR_AWARE
		}

		internal enum MONITOR_DPI_TYPE /*MONITOR_DPI_TYPE*/ {
			EFFECTIVE_DPI = 0, // MDT_EFFECTIVE_DPI
			ANGULAR_DPI = 1, // MDT_ANGULAR_DPI
			RAW_DPI = 2, // MDT_RAW_DPI
			DEFAULT  // MDT_DEFAULT
		};

		internal const int MONITOR_DEFAULTTONULL = 0x00000000; // Returns NULL.
		internal const int MONITOR_DEFAULTTOPRIMARY = 0x00000001; //Returns a handle to the primary display monitor.
		internal const int MONITOR_DEFAULTTONEAREST = 0x00000002; //Returns a handle to the display monitor that is nearest to the point.

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

		/// <summary>
		/// Sets the show state and the restored, minimized, and maximized positions of the specified window.
		/// </summary>
		/// <param name="hWnd">
		/// A handle to the window.
		/// </param>
		/// <param name="lpwndpl">
		/// A pointer to a WINDOWPLACEMENT structure that specifies the new show state and window positions.
		/// <para>
		/// Before calling SetWindowPlacement, set the length member of the WINDOWPLACEMENT structure to sizeof(WINDOWPLACEMENT). SetWindowPlacement fails if the length member is not set correctly.
		/// </para>
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is nonzero.
		/// <para>
		/// If the function fails, the return value is zero. To get extended error information, call GetLastError.
		/// </para>
		/// </returns>
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);
		
		internal const int WPF_ASYNCWINDOWPLACEMENT = 0x0004; // If the calling thread and the thread that owns the window are attached to different input queues, the system posts the request to the thread that owns the window.This prevents the calling thread from blocking its execution while other threads process the request.
		internal const int WPF_RESTORETOMAXIMIZED = 0x0002; //The restored window will be maximized, regardless of whether it was maximized before it was minimized. This setting is only valid the next time the window is restored.It does not change the default restoration behavior. This flag is only valid when the SW_SHOWMINIMIZED value is specified for the showCmd member.
		internal const int WPF_SETMINPOSITION = 0x0001; // The coordinates of the minimized window may be specified. This flag must be specified if the coordinates are set in the ptMinPosition member.

		internal enum ShowCmd : int {
			SW_HIDE = 0,
			SW_SHOWNORMAL = 1,
			SW_NORMAL = 1,
			SW_SHOWMINIMIZED = 2,
			SW_SHOWMAXIMIZED = 3,
			SW_MAXIMIZE = 3,
			SW_SHOWNOACTIVATE = 4,
			SW_SHOW = 5,
			SW_MINIMIZE = 6,
			SW_SHOWMINNOACTIVE = 7,
			SW_SHOWNA = 8,
			SW_RESTORE = 9,
			SW_SHOWDEFAULT = 10,
			SW_FORCEMINIMIZE = 11,
			SW_MAX = 11
		}
	}
}