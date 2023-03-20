using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Un1ver5e.Site.III.Server.Data;
using Un1ver5e.Site.III.Server.Models;

namespace Un1ver5e.Site.III.Server;

public class Startup
{
	private IConfiguration _cfg;
	private IWebHostEnvironment _env;

	public Startup(IConfiguration cfg, IWebHostEnvironment env)
	{
		_cfg = cfg;
		_env = env;
	}

	public void ConfigureServices(IServiceCollection services)
	{
		var connectionString = _cfg.GetConnectionString("DefaultConnection") 
			?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
		
		services.AddDbContext<ApplicationDbContext>(options =>
			options.UseNpgsql(connectionString));

		services.AddDatabaseDeveloperPageExceptionFilter();

		services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
			.AddEntityFrameworkStores<ApplicationDbContext>();

		services.AddIdentityServer()
			.AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

		services.AddAuthentication()
			.AddIdentityServerJwt();

		services.AddControllersWithViews();
		services.AddRazorPages();
	}
	public void Configure(IApplicationBuilder app)
	{
		if (_env.IsDevelopment())
		{
			app.UseMigrationsEndPoint();
			app.UseWebAssemblyDebugging();
		}
		else
		{
			app.UseExceptionHandler("/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}

		app.UseHttpsRedirection();

		app.UseBlazorFrameworkFiles();
		app.UseStaticFiles();

		app.UseRouting();

		app.UseIdentityServer();
		app.UseAuthorization();

		app.UseEndpoints(ep =>
		{
			ep.MapRazorPages();
			ep.MapControllers();
			ep.MapFallbackToFile("index.html");
		});
	}
}
