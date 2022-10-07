namespace Lab2Solution;

/**
Name: Paul Hwang
Date: October 7th, 2022
Description: Lab 3 - Building upon our Lab 2 and having the database be an actual running database
					 on bit.io. I use an API key to write and fetch data to/from the database.
Bugs: 
Reflection: 
*/

public static class MauiProgram
{
    public static IBusinessLogic ibl = new BusinessLogic();

    public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		return builder.Build();
	}
}

