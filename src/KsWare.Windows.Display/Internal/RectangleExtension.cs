using System.Drawing;
using System.Windows;
using System.Windows.Media;

namespace KsWare.Windows.Internal {

	internal static class RectangleExtension {

		public static Rect ToRect(this Rectangle r) {
			return new Rect(new System.Windows.Point(r.X, r.Y), new System.Windows.Size(r.Width, r.Height));
		}

		public static Geometry ToGeometry(this Rectangle r) {
			return new RectangleGeometry(r.ToRect());
		}

	}

}