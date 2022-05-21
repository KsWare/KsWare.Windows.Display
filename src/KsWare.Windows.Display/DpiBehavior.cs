using System;
using static KsWare.Windows.Native;

namespace KsWare.Windows {

	internal class DpiBehavior {

		private static DpiBehavior _backup;

		private DPI_HOSTING_BEHAVIOR _prevHosting;
		private DPI_AWARENESS_CONTEXT _prevDpiContext;
			
		public static DpiBehavior SetPerMonitorAwareV2() {
			if (!IsWindows10Build1809OrNewer)
				throw new PlatformNotSupportedException("This function s needs Windows 10.18090 or newer.");
			// https://github.com/microsoft/Windows-classic-samples/blob/master/Samples/DPIAwarenessPerWindow/client/DpiAwarenessContext.cpp
			_backup = new DpiBehavior() {
				_prevHosting = SetThreadDpiHostingBehavior(DPI_HOSTING_BEHAVIOR.MIXED),
				_prevDpiContext = SetThreadDpiAwarenessContext(DPI_AWARENESS_CONTEXT.PER_MONITOR_AWARE_V2)
			};
			Console.WriteLine(NormalizeThreadDpiAwarenessContext(_backup._prevDpiContext));
				
			return _backup;

		}

		public void Restore() {
			if (IsWindows10Build1809OrNewer) {
				SetThreadDpiAwarenessContext(_prevDpiContext);
				SetThreadDpiHostingBehavior(_prevHosting);
			}
		}

		private static bool IsWindows10Build1809OrNewer => IsWindows10OrNewer && Environment.OSVersion.Version.Build >= 17763;

		private static bool IsWindows10OrNewer => Environment.OSVersion.Version >= new Version(10, 0);
	}
}