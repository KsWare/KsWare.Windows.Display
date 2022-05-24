namespace KsWare.Windows {

	/// <summary>
	/// Identifies the awareness context for a window.
	/// </summary>
	/// <remarks><para>Native API: <see href="https://docs.microsoft.com/en-us/windows/win32/hidpi/dpi-awareness-context">DPI_AWARENESS_CONTEXT</see>. </para>
	/// </remarks>
	public enum DpiAwarenessContext {

		/// <summary>
		/// DPI unaware. This window does not scale for DPI changes and is always assumed to have a scale factor of 100% (96 DPI).
		/// It will be automatically scaled by the system on any other DPI setting.
		/// </summary>
		/// <remarks>Native API: DPI_AWARENESS_CONTEXT_UNAWARE</remarks>
		Unaware = -1,

		/// <summary>
		/// System DPI aware. This window does not scale for DPI changes. It will query for the DPI once and use that value
		/// for the lifetime of the process. If the DPI changes, the process will not adjust to the new DPI value.
		/// It will be automatically scaled up or down by the system when the DPI changes from the system value.
		/// </summary>
		/// <remarks>Native API: DPI_AWARENESS_CONTEXT_SYSTEM_AWARE</remarks>
		SystemAware = -2,

		/// <summary>
		/// Per monitor DPI aware. This window checks for the DPI when it is created and adjusts the scale factor whenever the
		/// DPI changes. These processes are not automatically scaled by the system.
		/// </summary>
		/// <remarks>Native API: DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE</remarks>
		PerMonitorAware = -3,

		/// <summary>
		/// Also known as Per Monitor v2. An advancement over the original per-monitor DPI awareness mode, which enables applications
		/// to access new DPI-related scaling behaviors on a per top-level window basis.
		/// </summary>
		///		<remarks>
		///		<para>Native API equivalent: DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2</para>
		///		<para>Per Monitor v2 was made available in the Creators Update of Windows 10, and is not available on earlier versions of the operating system.</para>
		///		<para><see href="https://docs.microsoft.com/en-us/windows/win32/hidpi/dpi-awareness-context">Read more...</see></para>
		/// </remarks>
		PerMonitorAwareV2 = -4,

		/// <summary>
		/// DPI unaware with improved quality of GDI-based content. This mode behaves similarly to <see cref="DpiAwarenessContext.Unaware"/>,
		/// but also enables the system to automatically improve the rendering quality of text and other GDI-based primitives
		/// when the window is displayed on a high-DPI monitor.
		/// </summary>
		/// <remarks>
		///	  <para>Native API equivalent: DPI_AWARENESS_CONTEXT_UNAWARE_GDISCALED</para>
		///	  <para>DPI_AWARENESS_CONTEXT_UNAWARE_GDISCALED was introduced in the October 2018 update of Windows 10 (also known as version 1809).</para>
		/// </remarks>
		UnawareGdiScaled = -5,
	}

}