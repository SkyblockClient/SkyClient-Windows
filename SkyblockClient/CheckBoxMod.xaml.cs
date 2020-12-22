using System.Windows;
using System.Windows.Controls;
using SkyblockClient.Options;

namespace SkyblockClient
{
	/// <summary>
	/// Interaktionslogik für CheckBoxMod.xaml
	/// </summary>
	public partial class CheckBoxMod : UserControl
	{
		public bool HasGuide
		{
			get => _hasGuide;
			set
			{
				_hasGuide = value;

				Visibility visibility;
				if (HasGuide)
					visibility = Visibility.Visible;
				else
					visibility = Visibility.Hidden;

				buttonOpenGuide.Visibility = visibility;
				rectInfoBorder.Visibility = visibility;
			}
		}
		private bool _hasGuide;

		public event ClickEventHandler Click;
		public delegate void ClickEventHandler(object sender, RoutedEventArgs e);

		public new object Content
		{
			get => checkBox.Content;
			set => checkBox.Content = value;
		}
		public bool IsChecked
		{
			get => checkBox.IsChecked ?? false;
			set => checkBox.IsChecked = value;
		}

		public CheckBoxMod()
		{
			InitializeComponent();
		}

		private void CheckBox_Click(object sender, RoutedEventArgs e)
		{
			this.Click?.Invoke(this, e);
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var option = this.Tag as Option;
			option.OpenGuide();
		}

        private void image_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
			 
        }
    }
}
