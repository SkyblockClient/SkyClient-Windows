using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Controls;

namespace SkyblockClient
{
	/// <summary>
	/// Interaktionslogik für TabControlItems.xaml
	/// </summary>
	public partial class TabControlOptions : UserControl
	{
		private int LastSelectedIndex = 0;
		public List<CheckBoxMod> ItemSource
		{
			get => Items.ToList();
			set => Items = new ObservableCollection<CheckBoxMod>(value);
		}

		public ObservableCollection<CheckBoxMod> Items
		{
			get => _items;
			set
			{
				if (_items != null)
					_items.CollectionChanged -= CollectionChanged;
				_items = value;
				_items.CollectionChanged += CollectionChanged;

				if (_items.Count > 0)
				{
					RedrawContent();
					tabControl.SelectedIndex = LastSelectedIndex;
				}
			}
		}

		private ObservableCollection<CheckBoxMod> _items;

		public bool HeadersAreDrawn = false;

		public TabControlOptions()
		{
			InitializeComponent();
			Items = new ObservableCollection<CheckBoxMod>();
		}

		private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			RedrawContent();
		}

		private string[] GetAllHeaders()
		{
			List<string> headers = new List<string>();
			if (Items != null)
			{
				foreach (var item in Items)
				{
					foreach (var category in item.Categories)
					{
						if (!headers.Contains(category))
						{
							headers.Add(category);
						}
					}
				}
			}
			headers.Sort();
			return headers.ToArray();
		}

		private void RedrawContent()
		{
			EnsureHeadersAreDrawn();

			var index = tabControl.SelectedIndex;
			if (index == -1)
				return;

			if (index == 0)
			{
				var contentList = Items;
				var content = new ListBox();
				content.ItemsSource = contentList;
				(tabControl.Items[index] as TabItem).Content = content;
			}
			else
			{
				var contentList = GetAllContent(index - 1);
				var content = new ListBox();
				content.ItemsSource = contentList;
				(tabControl.Items[index] as TabItem).Content = content;
			}
		}

		private void EnsureHeadersAreDrawn()
		{
			if (!HeadersAreDrawn)
			{
				var tabItems = new List<TabItem>();
				var headers = GetAllHeaders();

				var allTabItem = new TabItem();
				allTabItem.Header = "All";
				allTabItem.Content = new ListBox();
				tabItems.Add(allTabItem);

				foreach (var header in headers)
				{
					var tabItem = new TabItem();
					tabItem.Header = GetDisplayPartCategory(header);
					tabItem.Content = new ListBox();
					tabItems.Add(tabItem);
				}

				tabControl.ItemsSource = tabItems;

				HeadersAreDrawn = true;
			}
		}

		private List<CheckBoxMod> GetAllContent(int index)
		{
			var result = new List<CheckBoxMod>();
			var header = GetAllHeaders()[index];

			foreach (var item in Items)
			{
				foreach (var cat in item.Categories)
				{
					if (cat != header)
						continue;
					result.Add(item);
				}
			}

			return result;
		}

		private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			LastSelectedIndex = tabControl.SelectedIndex;
			RedrawContent();
		}

		private string GetDisplayPartCategory(string category)
		{
			return category.Split(';')[1];
		}
	}
}
