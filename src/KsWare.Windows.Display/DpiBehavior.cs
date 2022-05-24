using System;
using KsWare.Windows.Internal;

namespace KsWare.Windows {

	// https://mariusbancila.ro/blog/2021/05/19/how-to-build-high-dpi-aware-native-desktop-applications/

	// https://docs.microsoft.com/en-us/windows/win32/hidpi/setting-the-default-dpi-awareness-for-a-process
	
	public sealed class DpiBehavior : IDisposable {

		/// <summary>
		/// Sets the process-default DPI awareness level. 
		/// </summary>
		/// <param name="dac">The DPI awareness level</param>
		/// <remarks>
		/// <para>It is recommended that you set the process-default DPI awareness via application manifest. See <see href="https://docs.microsoft.com/en-us/windows/win32/hidpi/setting-the-default-dpi-awareness-for-a-process">Setting the default DPI awareness</see> for a process for more information. Setting the process-default DPI awareness via API call can lead to unexpected application behavior.</para>
		/// </remarks>
		/// <seealso href="https://docs.microsoft.com/en-us/windows/win32/hidpi/setting-the-default-dpi-awareness-for-a-process">Setting the default DPI awareness for a process.</seealso>
		public static void SetProcessDpiAwareness(DpiAwarenessContext dac) {
			Native.SetProcessDpiAwareness((IntPtr)dac);
		}

		/// <summary>
		/// Retrieves the <see cref="DpiHostingBehavior"/> from the current thread.
		/// </summary>
		/// <returns>The <see cref="DpiHostingBehavior"/> of the current thread.</returns>
		/// <remarks>This API returns the hosting behavior set by an earlier call of <see cref="SetThreadDpiHostingBehavior"/>, or <see cref="DpiHostingBehavior.Default"/> if no earlier call has been made.</remarks>
		public static DpiHostingBehavior GetThreadDpiHostingBehavior() {
			return (DpiHostingBehavior) (int) Native.GetThreadDpiHostingBehavior();
		}

		public static DpiHostingBehavior SetThreadDpiHostingBehavior(DpiHostingBehavior behavior) {
			return (DpiHostingBehavior)Native.SetThreadDpiHostingBehavior((Native.DPI_HOSTING_BEHAVIOR) behavior);
		}

		public static DpiAwarenessContext GetThreadDpiAwarenessContext() {
			return (DpiAwarenessContext) Native.GetThreadDpiAwarenessContext(true);
		}

		public static DpiBehavior SetThreadDpiAwarenessContext(DpiAwarenessContext context) {
			if (!WinVer.IsWindows10v1803OrNewer) throw new PlatformNotSupportedException("This function s needs Windows 10 build 1803 or newer.");
			// https://github.com/microsoft/Windows-classic-samples/blob/master/Samples/DPIAwarenessPerWindow/client/DpiAwarenessContext.cpp
			var backup = new DpiBehavior {
				_prevBehavior = Native.SetThreadDpiHostingBehavior(Native.DPI_HOSTING_BEHAVIOR.MIXED),
				_prevDpiContext = Native.SetThreadDpiAwarenessContext((Native.DPI_AWARENESS_CONTEXT)context)
			};
				
			return backup;
		}

		public static DpiBehavior SetThreadDpiPerMonitorAwareV2() {

			if (!WinVer.IsWindows10v1803OrNewer) throw new PlatformNotSupportedException("This function s needs Windows 10 build 1809 or newer.");
			// https://github.com/microsoft/Windows-classic-samples/blob/master/Samples/DPIAwarenessPerWindow/client/DpiAwarenessContext.cpp
			var backup = new DpiBehavior {
				_prevBehavior = Native.SetThreadDpiHostingBehavior(Native.DPI_HOSTING_BEHAVIOR.MIXED),
				_prevDpiContext = Native.SetThreadDpiAwarenessContext(Native.DPI_AWARENESS_CONTEXT.PER_MONITOR_AWARE_V2)
			};
			return backup;
		}

		private Native.DPI_HOSTING_BEHAVIOR? _prevBehavior;
		private Native.DPI_AWARENESS_CONTEXT? _prevDpiContext;

		public void Restore() {
			if (WinVer.IsWindows10v1803OrNewer) {
				if(_prevDpiContext.HasValue) Native.SetThreadDpiAwarenessContext(_prevDpiContext.Value);
				if(_prevBehavior.HasValue) Native.SetThreadDpiHostingBehavior(_prevBehavior.Value);
			}
		}

		
		void IDisposable.Dispose() => Restore();

	}

}