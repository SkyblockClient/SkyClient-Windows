namespace SkyblockClient.Options
{
	public class OptionPreview
	{
		public string Content;
		public Option Option;

		public OptionPreview(Option option, string text)
		{
			this.Option = option;
			this.Content = text;
		}
	}
}
