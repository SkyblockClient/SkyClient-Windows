using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using SkyblockClient.Options;

namespace SkyblockClient
{
	/// <summary>
	/// Interaktionslogik für CheckBoxMod.xaml
	/// </summary>
	public partial class CheckBoxMod : UserControl
	{

        public event ClickEventHandler Click;
		public delegate void ClickEventHandler(object sender, RoutedEventArgs e);

		public new object Content
		{
			get => chkEnabled.Content;
			set => chkEnabled.Content = value;
		}
		public bool IsChecked
		{
			get => chkEnabled.IsChecked ?? false;
			set => chkEnabled.IsChecked = value;
		}
		public List<OptionAction> Actions
        {
			get => _actions;
			set
            {
                _actions = value;

                cmbActions.Items.Clear();
                btnAction.Content = null;

                if (Utils.IsPropSet(Actions))
                {
                    var buttonActions = Actions.Where(a => a.Method == "click").ToList();

                    if (buttonActions.Count == 0)
                    {
                        cmbActions.Visibility = Visibility.Hidden;
                        btnAction.Visibility = Visibility.Hidden;
                        lblActionButtonDivider.Visibility = Visibility.Hidden;
                    }
                    else if (buttonActions.Count == 1)
                    {
                        btnAction.Content = buttonActions[0];

                        cmbActions.Visibility = Visibility.Hidden;
                        btnAction.Visibility = Visibility.Visible;
                        lblActionButtonDivider.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        cmbActions.ItemsSource = buttonActions.Select(a => a.Item);

                        cmbActions.Visibility = Visibility.Visible;
                        btnAction.Visibility = Visibility.Hidden;
                        lblActionButtonDivider.Visibility = Visibility.Hidden;
                    }
                }
            }
        }

        public string Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                if (Utils.IsPropSet(Icon))
                {
                    Utils.SetImage(btnIcon, Icon);
                }
                else
                {
                    if (!Globals.Settings.appendMissingOptionIcon)
                    {
                        gridCol0.Width = new GridLength(0);
                        gridCol1.Width = new GridLength(0);
                    }
                }
            }
        }
        private string _icon;

        private List<OptionAction> _actions;

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

        private void imgIcon_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
			Utils.Debug("mouse down");
			this.Click?.Invoke(this, new RoutedEventArgs());
		}

        private void cmbActions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var optionAction = cmbActions.SelectedItem as OptionAction;
            cmbActions.SelectedIndex = -1;
            if (optionAction != null)
                optionAction.Act();
        }

        private void btnAction_Click(object sender, RoutedEventArgs e)
        {
            var optionAction = btnAction.Content as OptionAction;
            if (optionAction != null)
                optionAction.Act();
        }

        private void btnIcon_Click(object sender, RoutedEventArgs e)
        {
            IsChecked = !IsChecked;
            this.Click?.Invoke(this, new RoutedEventArgs());
        }
    }
}
