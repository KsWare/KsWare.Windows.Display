using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KsWare.Windows;
using DispatcherPriority = System.Windows.Threading.DispatcherPriority;

namespace WpfApp1;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {

	public MainWindow() {
		InitializeComponent();
		UpdateUI();
	}

	private void UpdateUI() {
		switch (Native.GetThreadDpiHostingBehavior()) {
			case Native.DPI_HOSTING_BEHAVIOR.DEFAULT: DPI_HOSTING_BEHAVIOR_DEFAULT_RadioButton.IsChecked = true; break;
			case Native.DPI_HOSTING_BEHAVIOR.MIXED: DPI_HOSTING_BEHAVIOR_MIXED_RadioButton.IsChecked = true; break;
		}

		switch (Native.GetThreadDpiAwarenessContext(true)) {
			case Native.DPI_AWARENESS_CONTEXT.UNAWARE: DPI_AWARENESS_CONTEXT_UNAWARE_RadioButton.IsChecked=true; break;
			case Native.DPI_AWARENESS_CONTEXT.SYSTEM_AWARE: DPI_AWARENESS_CONTEXT_SYSTEM_AWARE_RadioButton.IsChecked=true; break;
			case Native.DPI_AWARENESS_CONTEXT.PER_MONITOR_AWARE: DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_RadioButton.IsChecked = true; break;
			case Native.DPI_AWARENESS_CONTEXT.PER_MONITOR_AWARE_V2: DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2_RadioButton.IsChecked = true; break;
			case Native.DPI_AWARENESS_CONTEXT.UNAWARE_GDI_SCALED: DPI_AWARENESS_CONTEXT_UNAWARE_GDI_SCALED_RadioButton.IsChecked=true; break;
		}
	}

	private void DPI_HOSTING_BEHAVIOR_RadioButton_OnClick(object sender, RoutedEventArgs e) {
		if (sender == DPI_HOSTING_BEHAVIOR_MIXED_RadioButton)
			Native.SetThreadDpiHostingBehavior(Native.DPI_HOSTING_BEHAVIOR.MIXED);
		else
			Native.SetThreadDpiHostingBehavior(Native.DPI_HOSTING_BEHAVIOR.DEFAULT);
		Dispatcher.BeginInvoke(DispatcherPriority.Normal, UpdateUI);
	}

	private void DPI_AWARENESS_CONTEXT_RadioButton_OnClick(object sender, RoutedEventArgs e) {
		var value = Enum.Parse<Native.DPI_AWARENESS_CONTEXT>($"{((RadioButton)sender).Content}");
		Native.SetThreadDpiAwarenessContext(value);
		Dispatcher.BeginInvoke(DispatcherPriority.Normal, UpdateUI);
	}

	private void NewWindowButton_OnClick(object sender, RoutedEventArgs e) {
		new TestWindow().Show();
	}
}
