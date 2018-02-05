using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Geetest.Core.Configuration;
using Geetest.Core.Mvc.Data;
using Geetest.Core.Mvc.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Geetest.Core.Mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/Account/Manage");
                    options.Conventions.AuthorizePage("/Account/Logout");
                });

            // Register no-op EmailSender used by account confirmation and password reset during development
            // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
            services.AddSingleton<IEmailSender, EmailSender>();


            // Add Autofac
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IGeetestManager)))
                .AsImplementedInterfaces();

            builder.Populate(services);

            builder.RegisterType<GeetestConfiguration>().As<IGeetestConfiguration>().SingleInstance();

            var iocContainer = builder.Build();

            var geetestConfig = iocContainer.Resolve<IGeetestConfiguration>();
            geetestConfig.Id = "624b8993f3ff3cf5111cc95a92355da8";
            geetestConfig.Key = "4dd6927f03b8b600ca821830e66bb7f6";

            return new AutofacServiceProvider(iocContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}