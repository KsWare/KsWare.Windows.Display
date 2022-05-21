﻿using System;
using System.Runtime.InteropServices;

namespace KsWare.Windows {

	internal static partial class Native {

		/// <summary>
		/// Contains information about the placement of a window on the screen.
		/// </summary>
		[Serializable]
		[StructLayout(LayoutKind.Sequential)]
		// ReSharper disable once InconsistentNaming
		// ReSharper disable once IdentifierTypo
		internal struct WINDOWPLACEMENT {

			/// <summary>
			/// The length of the structure, in bytes. Before calling the GetWindowPlacement or SetWindowPlacement functions, set this member to sizeof(WINDOWPLACEMENT).
			/// <para>
			/// GetWindowPlacement and SetWindowPlacement fail if this member is not set correctly.
			/// </para>
			/// </summary>
			public int Length;

			/// <summary>
			/// Specifies flags that control the position of the minimized window and the method by which the window is restored.
			/// </summary>
			public int Flags;

			/// <summary>
			/// The current show state of the window.
			/// </summary>
			public ShowCmd ShowCmd;

			/// <summary>
			/// The coordinates of the window's upper-left corner when the window is minimized.
			/// </summary>
			public POINT MinPosition;

			/// <summary>
			/// The coordinates of the window's upper-left corner when the window is maximized.
			/// </summary>
			public POINT MaxPosition;

			/// <summary>
			/// The window's coordinates when the window is in the restored position.
			/// </summary>
			public RECT NormalPosition;

			/// <summary>
			/// Gets the default (empty) value.
			/// </summary>
			public static WINDOWPLACEMENT Default {
				get {
					var result = new WINDOWPLACEMENT();
					result.Length = Marshal.SizeOf(result);
					return result;
				}
			}
		}
	}
}