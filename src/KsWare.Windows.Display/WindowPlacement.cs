using System;
using System.Drawing;
using System.Linq;
using System.Windows;

namespace KsWare.Windows {

	/// <summary>
	/// Contains information about the placement of a window on the screen.
	/// </summary>
	/// <remarks>WINDOWPLACEMENT</remarks>
	public class WindowPlacement {

		/// <summary>
		/// The window's coordinates when the window is in the restored position.
		/// </summary>
		/// <remarks>WINDOWPLACEMENT.NormalPosition</remarks>
		public Rectangle Bounds { get; set; }

		/// <summary>
		/// The current show state of the window.
		/// </summary>
		/// <remarks> WINDOWPLACEMENT.ShowCmd</remarks>
		public WindowState State { get; set; }

		internal static WindowPlacement From(Native.WINDOWPLACEMENT windowPlacement) {
			return new WindowPlacement {
				Bounds = windowPlacement.NormalPosition.ToRectangle(),
				State = From(windowPlacement.ShowCmd)
			};
		}

		private static WindowState From(Native.ShowCmd showCmd) {
			switch (showCmd) {
				case Native.ShowCmd.SW_NORMAL: return WindowState.Normal;
				case Native.ShowCmd.SW_MAXIMIZE: return WindowState.Maximized;
				case Native.ShowCmd.SW_MINIMIZE: return WindowState.Minimized;
				default: return WindowState.Normal;
			}
		}

		public override string ToString() {
			return $"{Bounds.X},{Bounds.Y},{Bounds.Width},{Bounds.Height}, {State}";
		}

		public static WindowPlacement Parse(string s) {
			var parts = s.Split(',').Select(o=>o.Trim());
			var bounds = parts.Take(4).Select(int.Parse).ToArray();
			var state = parts.Skip(4).Take(1).Select(x=>(WindowState)Enum.Parse(typeof(WindowState), x)).First();
			return new WindowPlacement {
				Bounds = new Rectangle(bounds[0], bounds[1], bounds[3], bounds[3]),
				State = state
			};
		}
	}
}
