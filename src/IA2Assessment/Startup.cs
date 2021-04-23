using IA2Assessment.Models;
using IA2Assessment.Models.Views;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
			
			//Add our tuckshop database context
			services.AddDbContext<TuckshopDbContext>(options =>
				options.UseMySQL(Configuration["ConnectionStrings:TuckshopConnection"]));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseFileServer(); 
			app.UseMvc(route =>
			{
				route.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
