using IA2Assessment.Identity;
using IA2Assessment.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace IA2Assessment
{
	/// <summary>
	///		Class for setting our ASP.NET Core app
	/// </summary>
	public class Startup
	{
		private IConfiguration Configuration { get; set; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			//Setup our configuration
			IConfigurationBuilder builder = new ConfigurationBuilder()   
				.AddJsonFile("appsettings.json"); 
			Configuration = builder.Build();
			
			//Setup Mvc
			services.AddMvc(options => options.EnableEndpointRouting = false);
			
			//Setup Web Optimizer
			services.AddWebOptimizer(pipeline =>
			{
				pipeline.MinifyJsFiles("js/*.js");
				pipeline.MinifyCssFiles("css/*.css");
				pipeline.MinifyHtmlFiles();
			});

			//Setup temp data
			services.AddSession();

			//Add our tuckshop database context
			services.AddDbContext<TuckshopDbContext>(options =>
				options.UseMySQL(Configuration["ConnectionStrings:TuckshopConnection"]));

			//Setup our custom identity
			services.AddIdentity<User, UserRole>()
				.AddDefaultTokenProviders();
			services.AddTransient<IUserStore<User>, UserStore>();
			services.AddTransient<IRoleStore<UserRole>, RoleStore>();
			services.ConfigureApplicationCookie(options =>
			{
				options.Cookie.HttpOnly = true;
				options.LoginPath = "/Account/Login";
				options.LogoutPath = "/Account/Logout";
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TuckshopDbContext dbContext, SignInManager<User> signInManager)
		{
			//If we are in a development environment, enable the exception page
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();

				if (signInManager.UserManager.FindByNameAsync("admin").Result == null)
				{
					IdentityResult _ = signInManager.UserManager.CreateAsync(new User
					{
						UserName = "admin",
						UserFirstName = "Admin",
						UserLastName = "Master"
					}, "Password.1234").Result;
					dbContext.SaveChanges();
				}
			}

			//Configure our app
			app.UseSession();
			app.UseAuthentication();
			app.UseWebOptimizer();
			app.UseStaticFiles();
			app.UseFileServer(); 
			app.UseMvc(route =>
			{
				route.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
