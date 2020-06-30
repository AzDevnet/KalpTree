using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KalpTree.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace KalpTree
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                    builder.AllowAnyOrigin();
                    builder.AllowCredentials();
                });
            });
            var GoogleSearchApiURL = Configuration["Appconfig:GoogleSearchApiURL"].ToString();
            var GoogleSearchApiKey = Configuration["Appconfig:GoogleSearchApiKey"].ToString();
            var GoogleSearchApiCX = Configuration["Appconfig:GoogleSearchApiCX"].ToString();
            var ChatBoardApi = Configuration["Appconfig:ChatBoardApi"].ToString();

            var LoginApiUrl = Configuration["LoginApi:LoginApiUrl"].ToString();

            services.Configure<GoogleSearchAPI>(config => config.url = GoogleSearchApiURL);
            services.Configure<GoogleSearchAPI>(config => config.Key = GoogleSearchApiKey);
            services.Configure<GoogleSearchAPI>(config => config.CX = GoogleSearchApiCX);
            services.Configure<GoogleSearchAPI>(config => config.ChatBoardApi = ChatBoardApi);
            services.Configure<KalpTreeAPI>(config => config.LoginApiUrl = LoginApiUrl);

            services.AddDistributedMemoryCache();
            services.AddSession( options => {
                options.IdleTimeout = TimeSpan.FromMinutes(10);//You can set Time   
            });
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
            });
            services.AddSession(opts =>
            {
                opts.Cookie.IsEssential = true; // make the session cookie Essential
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors("CorsPolicy");
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "Login",
                    template: "{controller=Home}/{action=index}/{id?}");
            });
        }
    }
}
