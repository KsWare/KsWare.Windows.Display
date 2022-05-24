using System.Windows;
using KsWare.Windows;

namespace WpfApp1 {

	/// <summary>
	/// Interaction logic for VirtualScreenWindow.xaml
	/// </summary>
	public partial class VirtualScreenWindow : Window {

		public VirtualScreenWindow() {
			InitializeComponent();
			Path.Data = VirtualScreen.Geometry;
		}

	}

}

