using System.Windows;
using System.Windows.Media;
using NUnit.Framework;

namespace KsWare.Windows.Display.Tests {

	public class VirtualScreenTests {

		[Test]
		public void InitTest() {
			var bounds = VirtualScreen.Bounds;
		}

		[Test]
		public void ContainsTest() {
			var allDisplays = DisplayInfo.Displays;
			foreach (var di in allDisplays) {
				Assert.That(VirtualScreen.Contains(di.Bounds, false), Is.True);
			}
		}

		[Test]
		public void ContainsExactTest() {
			var allDisplays = DisplayInfo.Displays;
			foreach (var di in allDisplays) {
				Assert.That(VirtualScreen.Contains(di.Bounds, true), Is.True);
			}
		}
		
		[Test]
		public void FillContainsTest() {
			var v = new RectangleGeometry(new Rect(0, 0, 10, 10));
			var i0 = v.FillContainsWithDetail(new RectangleGeometry(new Rect(0, 0, 10, 10)));			// Intersects
			var i1 = v.FillContainsWithDetail(new RectangleGeometry(new Rect(0, 0, 9, 9)));				// FullyContains
			var i2 = v.FillContainsWithDetail(new RectangleGeometry(new Rect(1, 1, 9, 9)));				// FullyContains
			var i3 = v.FillContainsWithDetail(new RectangleGeometry(new Rect(1, 1, 10, 10)));			// Intersects
			var i4 = v.FillContainsWithDetail(new RectangleGeometry(new Rect(0, 0, 10-0.1, 10-0.1)));	// FullyContains
			var i5 = v.FillContainsWithDetail(new RectangleGeometry(new Rect(-1,-1,12,12)));			// FullyInside
			var i6 = v.FillContainsWithDetail(new RectangleGeometry(new Rect(0.1, 0.1, 9.9, 9.9)));		// FullyContains
		}

	}

}

