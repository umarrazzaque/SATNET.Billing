using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SATNET.Domain;
using SATNET.Repository.Core;
using SATNET.Repository.Core.Interface;
using SATNET.Repository.Implementation;
using SATNET.Repository.Interface;
using SATNET.Service.Implementation;
using SATNET.Service.Interface;
using SATNET.WebApp.Areas.Identity.Data;
using SATNET.WebApp.Data;
using SATNET.WebApp.MappingProfiles;

namespace SATNET.WebApp
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
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();
                //.AddClaimsPrincipalFactory<MyUserClaimsPrincipalFactory>();  //for custom claims

            services.AddAuthorization(options =>
            {
                options.AddPolicy("UserAccessPolicy", policy => policy.RequireRole("Admin"));
            });
            //services.AddAuthorization(options => {
            //    options.AddPolicy("UserAccessPolicy", policy =>
            //    {
            //        policy.RequireClaim(ClaimTypes.Role, "Admin");
            //    });
            //});
            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddControllersWithViews();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();

            //        services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest)
            //.AddRazorPagesOptions(options =>
            //{
            //    options.AllowAreas = true;
            //    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
            //    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
            //});

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            // using Microsoft.AspNetCore.Identity.UI.Services;
            //services.AddSingleton<IEmailSender, EmailSender>();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IService<User>, UserService>();
            services.AddScoped<IService<ServicePlan>, ServicePlanService>();
            services.AddScoped<IService<ServicePlanPrice>, ServicePlanPriceService>();
            services.AddScoped<IService<Customer>, CustomerService>();
            services.AddScoped<IService<Hardware>, HardwareService>();
            services.AddScoped<IService<Site>, SiteService>();
            services.AddScoped<IService<Order>, OrderService>();
            services.AddScoped<IService<Lookup>, LookupService>();
            services.AddScoped<IService<Token>, TokenService>();
            services.AddScoped<IService<Promotion>, PromotionService>();
            services.AddScoped<IService<IP>, IPService>();

            
            services.AddScoped<IRepository<User>, UserRepository>();
            services.AddScoped<IRepository<ServicePlan>, ServicePlanRepository>();
            services.AddScoped<IRepository<ServicePlanPrice>, ServicePlanPriceRepository>();
            services.AddScoped<IRepository<Hardware>, HardwareRepository>();
            services.AddScoped<IRepository<Site>, SiteRepository>();
            services.AddScoped<IRepository<Customer>, CustomerRepository>();
            services.AddScoped<IRepository<IP>, IPRepository>();
            services.AddScoped<IRepository<Promotion>, PromotionRepository>();
            services.AddScoped<IRepository<Token>, TokenRepository>();
            services.AddScoped<IRepository<Order>, OrderRepository>();
            services.AddScoped<IRepository<Lookup>, LookupRepository>();
            
            

            //-------------Misc-----------
            //services.AddScoped<IRepository<Customer>, ResellerRepository>();
            //services.AddScoped<IService<Customer>, ResellerService>();
            //-------------Misc-----------
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

    }
}