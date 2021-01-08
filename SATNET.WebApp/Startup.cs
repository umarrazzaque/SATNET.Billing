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
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using SATNET.Domain;
using SATNET.Repository.Core;
using SATNET.Repository.Core.Interface;
using SATNET.Repository.Implementation;
using SATNET.Repository.Interface;
using SATNET.Service.Implementation;
using SATNET.Service.Interface;
using SATNET.WebApp.Areas.Identity.Data;
using SATNET.WebApp.BackgroundTasks;
using SATNET.WebApp.BackgroundTasks.Jobs;
using SATNET.WebApp.Data;
using SATNET.WebApp.Helpers;
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
                options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
                options.AddPolicy("ManageServiceOrderPolicy", policy=>policy.RequireRole("Admin", "Reseller Accounting", "Reseller Operations"));
                options.AddPolicy("ReadOnlyServiceOrderPolicy", policy => policy.RequireRole("Admin", "Reseller Accounting", "Reseller Operations", "Management", "Accounting", "NOC"));
                options.AddPolicy("ReadOnlySOInvoicePolicy", policy => policy.RequireRole("Admin", "Reseller Accounting", "Reseller Operations", "Management", "Accounting"));
                options.AddPolicy("ReadOnlySitePolicy", policy => policy.RequireRole("Admin", "Reseller Accounting", "Reseller Operations", "Management", "Accounting", "NOC"));
                options.AddPolicy("ReadOnlyLogisticsPolicy", policy => policy.RequireRole("Admin", "Management", "Accounting", "Logistics"));
                options.AddPolicy("ManageLogisticsPolicy", policy => policy.RequireRole("Admin", "Accounting", "Logistics"));
            });

            //Session

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(5);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
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
            services.AddScoped<IService<City>, CityService>();
            services.AddScoped<IService<HardwareComponent>, HardwareComponentService>();
            services.AddScoped<IService<HardwareKit>, HardwareKitService>();
            services.AddScoped<IService<SOInvoice>, SOInvoiceService>();
            services.AddScoped<IService<MRCInvoice>, MRCInvoiceService>();
            services.AddScoped<IService<IP>, IPService>();
            services.AddScoped<IService<IPPrice>, IPPriceService>();
            services.AddScoped<IService<TokenPrice>, TokenPriceService>();
            services.AddScoped<IService<CreditNote>, CreditNoteService>();
            services.AddScoped<IService<HardwareComponentPrice>, HardwareComponentPriceService>();
            services.AddScoped<IService<HardwareComponentRegistration>, HardwareComponentRegistrationService>();
            services.AddScoped <IAPIService, APIService>();

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
            services.AddScoped<IRepository<City>, CityRepository>();
            services.AddScoped<IRepository<HardwareComponent>, HardwareComponentRepository>();
            services.AddScoped<IRepository<HardwareKit>, HardwareKitRepository>();


            services.AddScoped<IRepository<SOInvoice>, SOInvoiceRepository>();
            services.AddScoped<IRepository<MRCInvoice>, MRCInvoiceRepository>();
            services.AddScoped<IRepository<IP>, IPRepository>();
            services.AddScoped<IRepository<IPPrice>, IPPriceRepository>();
            services.AddScoped<IRepository<TokenPrice>, TokenPriceRepository>();
            services.AddScoped<IRepository<CreditNote>, CreditNoteRepository>();
            services.AddScoped<IRepository<HardwareComponentPrice>, HardwareComponentPriceRepository>();
            services.AddScoped<IRepository<HardwareComponentRegistration>, HardwareComponentRegistrationRepository>();
            //-------------Misc-----------
            //services.AddScoped<IRepository<Customer>, ResellerRepository>();
            //services.AddScoped<IService<Customer>, ResellerService>();
            //-------------Misc-----------

            //------Background Services-----------//

            //services.AddSingleton<NotificationJob>();
            //services.AddSingleton(new JobMetadata(Guid.NewGuid(), typeof(NotificationJob), "Notification Job", "0/10 * * * * ?"));
            //services.AddSingleton(new JobMetadata(Guid.NewGuid(), typeof(InvoiceJob), "Invoice Job", "0 */1 * ? * *"));// every 2 mins
            //services.AddSingleton(new JobMetadata(Guid.NewGuid(), typeof(InvoiceJob), "Invoice Job", "0 */1 * ? * *"));// every hour
            //services.AddSingleton(new JobMetadata(Guid.NewGuid(), typeof(InvoiceJobMonthly), "Invoice Job Monthly", "0 */1 * ? * *"));// At 00:00:00am, on the 1st day, every month

            services.AddHostedService<CustomQuartzHostedService>();
            services.AddSingleton<IJobFactory, CustomQuartzJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            //jobs
            services.AddSingleton<InvoiceJob>(); 
            services.AddSingleton<MRCJob>();
            services.AddSingleton<EndOfMonthJob>();

            services.AddSingleton<IBackgroundTaskService, BackgroundTaskService>();
            services.AddSingleton<IBackgroundTaskRepository, BackgroundTaskRepository>();
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
            app.UseSession();
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