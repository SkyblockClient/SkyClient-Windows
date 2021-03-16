using SkyblockClient.Options;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;

namespace SkyblockClient
{
	/// <summary>
	/// Interaktionslogik für MarkdownDocument.xaml
	/// </summary>
	public partial class MarkdownDocument : UserControl
	{
		private static PropertyInfo[] stringProperties = typeof(Option).GetProperties()
			.ToList().Where(prop => prop.PropertyType == typeof(string)).ToArray();

		public OptionPreview Document
		{
			get => _document;
			set
			{
				_document = value;
				if (!string.IsNullOrEmpty(value.Content))
				{
					this.editSource.Text = value.Content;
				}
			}
		}
		private OptionPreview _document;

		public MarkdownDocument()
        {
            InitializeComponent();
		}
    }
}
