using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace KsWare.Windows.Display.Tests {

	public  class DpiBehaviorTests {

		[Test]
		public void GetSetThreadDpiHostingBehaviorTest() {
			var first=DpiBehavior.GetThreadDpiHostingBehavior();
			var firstCopy=DpiBehavior.SetThreadDpiHostingBehavior(DpiHostingBehavior.Mixed);
			var second=DpiBehavior.GetThreadDpiHostingBehavior();
			var secondCopy = DpiBehavior.SetThreadDpiHostingBehavior(DpiHostingBehavior.Default);
			var third=DpiBehavior.GetThreadDpiHostingBehavior();

			Assert.That(first, Is.EqualTo(DpiHostingBehavior.Default));
			Assert.That(firstCopy, Is.EqualTo(DpiHostingBehavior.Default));
			Assert.That(second,Is.EqualTo(DpiHostingBehavior.Mixed));
			Assert.That(secondCopy, Is.EqualTo(DpiHostingBehavior.Mixed));
			Assert.That(third,Is.EqualTo(DpiHostingBehavior.Default));
		}

		[Test]
		public void SetPerMonitorAwareV2AndRestoreTest() {
			var behavior0=DpiBehavior.GetThreadDpiHostingBehavior();
			var context0 = DpiBehavior.GetThreadDpiAwarenessContext();
			var used = DpiBehavior.SetThreadDpiPerMonitorAwareV2();
			var behavior1=DpiBehavior.GetThreadDpiHostingBehavior();
			var context1 = DpiBehavior.GetThreadDpiAwarenessContext();
			used.Restore();
			var behavior2=DpiBehavior.GetThreadDpiHostingBehavior();
			var context2 = DpiBehavior.GetThreadDpiAwarenessContext();

			//
			Assert.That(behavior1,Is.EqualTo(DpiHostingBehavior.Mixed));
			Assert.That(context1,Is.EqualTo(DpiAwarenessContext.PerMonitorAwareV2));

			//restore
			Assert.That(behavior2,Is.EqualTo(behavior0));
			Assert.That(context2,Is.EqualTo(context0));
		}

		[Test]
		public void InformationalTest() {
			var monitorInfos0 = Native.GetDisplayMonitors(true);
			using(var _ = DpiBehavior.SetThreadDpiAwarenessContext(DpiAwarenessContext.Unaware)) {
				Console.WriteLine(DpiBehavior.GetThreadDpiAwarenessContext());
				var monitorInfos1 = Native.GetDisplayMonitors(true);
				foreach (var mi in monitorInfos1) {
					Console.WriteLine(mi.Monitor);
				}
			}
			using(var _ = DpiBehavior.SetThreadDpiAwarenessContext(DpiAwarenessContext.PerMonitorAwareV2)) {
				Console.WriteLine(DpiBehavior.GetThreadDpiAwarenessContext());
				var monitorInfos1 = Native.GetDisplayMonitors(true);
				foreach (var mi in monitorInfos1) {
					Console.WriteLine(mi.Monitor);
				}
			}
			using(var _ = DpiBehavior.SetThreadDpiAwarenessContext(DpiAwarenessContext.UnawareGdiScaled)){
				Console.WriteLine(DpiBehavior.GetThreadDpiAwarenessContext());
				var monitorInfos1 = Native.GetDisplayMonitors(true);
				foreach (var mi in monitorInfos1) {
					Console.WriteLine(mi.Monitor);
				}
			}
		}
	}


}