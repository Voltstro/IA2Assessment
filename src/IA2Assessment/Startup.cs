using IA2Assessment.Identity;
using IA2Assessment.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace IA2Assessment
{
	public class Startup
	{
		private IConfiguration Configuration { get; set; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			IConfigurationBuilder builder = new ConfigurationBuilder()   
				.AddJsonFile("appsettings.json"); 
			Configuration = builder.Build();
			
			//Setup Mvc
			services.AddMvc(options => options.EnableEndpointRouting = false);

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
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();

				if (signInManager.UserManager.FindByNameAsync("admin").Result == null)
				{
					IdentityResult result = signInManager.UserManager.CreateAsync(new User
					{
						UserName = "admin",
						UserFirstName = "Admin",
						UserLastName = "Master"
					}, "Password.1234").Result;
					dbContext.SaveChanges();
				}
			}

			app.UseSession();
			app.UseAuthentication();
			app.UseFileServer(); 
			app.UseMvc(route =>
			{
				route.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
