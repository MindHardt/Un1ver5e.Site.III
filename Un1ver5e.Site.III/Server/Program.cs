using Serilog;

Host.CreateDefaultBuilder(args)
	.ConfigureAppConfiguration(config =>
	{
		config
		.AddJsonFile("appsettings.json", optional: true)
		.AddJsonFile("appsettings.development.json", optional: true)
		.AddUserSecrets<Program>(optional: true)
		.AddEnvironmentVariables();
	})
	.UseSerilog((ctx, logger) =>
	{
		logger
		.ReadFrom.Configuration(ctx.Configuration);
	})
	.ConfigureWebHostDefaults(webBuilder =>
	{
		webBuilder.UseStartup<Un1ver5e.Site.III.Server.Startup>();
	})
	.Build()
	.Run();
