using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SkyblockClient.Options;
using SkyblockClient.Options.Events;

namespace SkyblockClient
{
	/// <summary>
	/// Interaktionslogik für CheckBoxMod.xaml
	/// </summary>
	public partial class CheckBoxMod : UserControl
	{
		public string[] Categories { get; set; }

        public event ClickEventHandler Click;
		public delegate void ClickEventHandler(object sender, RoutedEventArgs e);
       
        public event TextClickEventHandler HoverEnter;
        public event TextClickEventHandler HoverLeave;
        public delegate void TextClickEventHandler(object sender, TextMouseEventArgs e);

        public event GuideRequestEventHandler GuideRequest;
        public delegate void GuideRequestEventHandler(object sender, GuideRequestEventArgs e);

        public string DocumentText;
        private bool HasDocument => Utils.IsPropSet(DocumentText) && DocumentText != "invalid";

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
                    var hoverActions = Actions.Where(a => a.Method == "hover").ToList();

                    if (hoverActions.Count > 0)
                    {
                        var hoverAction = hoverActions[0];
                        this.GuideRequest?.Invoke(this, new GuideRequestEventArgs(this, hoverAction));
                        if (GuideRequest is null)
                            Utils.Debug("GuideRequest is null");
                    }

                    if (buttonActions.Count == 0)
                    {
                        cmbActions.Visibility = Visibility.Hidden;
                        btnAction.Visibility = Visibility.Hidden;
                        lblActionButtonDivider.Visibility = Visibility.Hidden;
                    }
                    else if (buttonActions.Count == 1)
                    {
						Utils.SetImage(btnAction, buttonActions[0].Icon);
                        btnAction.Tag = buttonActions[0];

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
            var optionAction = btnAction.Tag as OptionAction;
            if (optionAction != null)
                optionAction.Act();
        }

        private void btnIcon_Click(object sender, RoutedEventArgs e)
        {
            IsChecked = !IsChecked;
            this.Click?.Invoke(this, new RoutedEventArgs());
        }

        private void ActionMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (HasDocument)
                this.HoverEnter?.Invoke(this, new TextMouseEventArgs(DocumentText, e));
        }

        private void ActionMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (HasDocument)
                this.HoverLeave?.Invoke(this, new TextMouseEventArgs("", e));
        }
    }
}
