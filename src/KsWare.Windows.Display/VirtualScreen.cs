using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using KsWare.Windows.Internal;
using Microsoft.Win32;
using Brushes = System.Windows.Media.Brushes;
using Pen = System.Windows.Media.Pen;

namespace KsWare.Windows {

	// https://docs.microsoft.com/en-us/windows/win32/gdi/the-virtual-screen
	// https://docs.microsoft.com/en-us/windows/win32/gdi/about-multiple-display-monitors

	internal class VirtualScreen {

		private static Geometry _geometry;

		public static Geometry Geometry => _geometry.Clone();

		static VirtualScreen() {
			Update();
		}

		private static void Update() {
			var allDisplays = Helper.GetAllDisplays();
			var r = new Rectangle();
			Geometry g = null;
			foreach (var d in allDisplays) {
				r = Rectangle.Union(r, d.Bounds);
				g = g == null ? d.Bounds.ToGeometry() : g.Union(d.Bounds.ToGeometry());
			}
			Bounds = r;
			_geometry = g.GetOutlinedPathGeometry();
			HasUncoveredArea = g.GetArea() < g.Bounds.Width * g.Bounds.Height;
		}

		/// <summary>
		/// Gets the bounds.
		/// </summary>
		/// <value>The bounds.</value>
		/// <remarks>This API does not participate in DPI virtualization. The values are always in terms of physical pixels, and is not related to the calling context.</remarks>
		public static Rectangle Bounds { get; private set; }


		/// <summary>Gets a value indicating whether the virtual screen as areas which are not covered by a monitor.</summary>
		/// <value><c>true</c> if virtual screen has uncovered area; otherwise, <c>false</c>.</value>
		public static bool HasUncoveredArea { get; private set; }

		/// <summary>
		/// Determines if this rectangle intersects with rect.
		/// </summary>
		/// <remarks>This API does not participate in DPI virtualization. The values are always in terms of physical pixels, and is not related to the calling context.</remarks>
		public static bool IntersectsWith(Rectangle rect, bool checkExactMonitorCoveredArea = false) {
			if(checkExactMonitorCoveredArea==false) return Bounds.IntersectsWith(rect);
			return _geometry.FillContainsWithDetail(new Rectangle(rect.X, rect.Y, rect.Width - 1, rect.Height - 1).ToGeometry())==IntersectionDetail.Intersects;
		}

		/// <summary>
		/// Determines if the rectangular region represented by <paramref name="rect"/> is entirely contained within the
		/// rectangular region represented by this <see cref='VirtualScreen.Bounds'/>.
		/// </summary>
		/// <remarks>This API does not participate in DPI virtualization. The values are always in terms of physical pixels, and is not related to the calling context.</remarks>
		public static bool Contains(Rectangle rect, bool checkExactMonitorCoveredArea = false) {
			if(checkExactMonitorCoveredArea==false) return Bounds.Contains(rect);
			return _geometry.FillContains(new Rectangle(rect.X, rect.Y, rect.Width - 1, rect.Height - 1).ToGeometry());
		}

	}

}

