namespace Makini.Src.Views.HelperPages;

public class StartPage : ContentPage
{
	public StartPage()
	{
		Content = new StackLayout
		{
			Children = {
				new Label { Text = "Welcome to .NET MAUI!" }
			}
		};
	}
}