using Markdig;
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

		MarkdownPipeline pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

		public OptionPreview Document
		{
			set
			{
				_document = value;
				if (!string.IsNullOrEmpty(value.Content))
				{
					var html = Globals.PreviewHtmlBase;
					html = ReplacePladeholder(html, "CONTENT", Markdown.ToHtml(value.Content, pipeline));
					var option = value.Option;
					foreach (var prop in stringProperties)
					{
						var val = prop.GetValue(option).ToString();
						html = ReplacePladeholder(html, "OPTION." + prop.Name.ToUpper(), val);
					}
					browser.NavigateToString(html);
					Utils.Debug(html);
				}
			}
		}
		private OptionPreview _document;

		public MarkdownDocument()
        {
            InitializeComponent();
			browser.NavigateToString("Loading...");
		}

		private string ReplacePladeholder(string baseText, string key, string value)
		{
			return baseText.Replace("{{{" + key + "}}}", value);
		}
    }
}
