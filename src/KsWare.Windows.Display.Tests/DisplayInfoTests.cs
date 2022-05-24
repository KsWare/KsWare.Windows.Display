using NUnit.Framework;

namespace KsWare.Windows.Display.Tests {

	public class DisplayInfoTests {

		[SetUp]
		public void Setup() { }

		[Test]
		public void GetAllDisplaysTest() {
			Assert.That(DisplayInfo.Displays, Is.Not.Null);
			Assert.That(DisplayInfo.Displays?.Length, Is.GreaterThanOrEqualTo(1));
		}

		[Test]
		public void AllFromRect() {
			var allDisplays = (DisplayInfo[]) DisplayInfo.Displays.Clone();
			foreach (var di in allDisplays) {
				var rectDIs = DisplayInfo.AllFromRect(di.Bounds);
				Assert.That(rectDIs.Length, Is.EqualTo(1));
				Assert.That(rectDIs[0].Bounds, Is.EqualTo(di.Bounds));
			}

		}

	}

}