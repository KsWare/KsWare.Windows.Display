using System.Windows.Media;

namespace KsWare.Windows.Internal {

	internal static class GeometryExtensions {

		public static Geometry Union(this Geometry g0, Geometry g1) {
			return new CombinedGeometry {
				GeometryCombineMode = GeometryCombineMode.Union,
				Geometry1 = g0,
				Geometry2 = g1
			};
		}
	}

}
