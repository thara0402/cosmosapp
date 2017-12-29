using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cosmosapp.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace cosmosapp
{
    public class Startup
    {
        private const string EndpointUri = "https://xxx1216.documents.azure.com:443/";
        private const string PrimaryKey = "dG4VVoVXPVWIj1x67SV3m6ntBgPwxxxMP459eqNoqaLgSFI3c4VFfp8w4fwK2kc3OxOd36gYKmoxYrnaV2YE5w==";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(provider =>
            {
                return new DocumentClient(new Uri(EndpointUri), PrimaryKey);
            });
            services.AddSingleton<PersonRepository>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
