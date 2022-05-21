using NUnit.Framework;

namespace KsWare.Windows.Display.Tests;

public class Tests {
	[SetUp]
	public void Setup() {
	}

	[Test, Ignore("manual test")]
	public void Test1() {
		var bac=DpiBehavior.SetPerMonitorAwareV2();
		bac.Restore();
	}
}