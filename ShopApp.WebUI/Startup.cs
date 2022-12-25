using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShopApp.Business.Abstract;
using ShopApp.Business.Concrete;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Concrete.EfCore;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationIdentityDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));

            services.AddIdentity<ApplicationUser,IdentityRole>()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // PASSWORD

                options.Password.RequireDigit=true; // Sifre icerisinde say�sal de�er ister.
                options.Password.RequireLowercase=true; // Sifre icerisinde k�c�k karakter ister.
                options.Password.RequiredLength = 6; // Sifre uzunlu�unun min de�erini belirleme.
                options.Password.RequireNonAlphanumeric=true; // Alfa numerik de�er isteyip istememe.
                options.Password.RequireUppercase=true; // Sifre icerisinde buyuk karakter ister.

                options.Lockout.MaxFailedAccessAttempts = 5; // Kullan�c� �st �ste kac yanl�s sifre girerse blocklan�r.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Bloklanma s�resi
                options.Lockout.AllowedForNewUsers = true; // Lockout i�lemi yeni kullan�c�da ge�erli olucak.

                options.User.RequireUniqueEmail = true; // �nceden mail adresiyle olu�turulmu� hesap olmas�n� �nler.

                options.SignIn.RequireConfirmedEmail = false; // Email do�rulamas� yapmas� gerekir.
                options.SignIn.RequireConfirmedPhoneNumber = false; // Telefon Do�rulamas�.

            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
                options.ExpireTimeSpan=TimeSpan.FromMinutes(60); // Cookie S�re verme
                options.SlidingExpiration = true; // Tekrar Login islemi isteme
                options.Cookie = new CookieBuilder()
                {
                    HttpOnly = true, //Scriptler cookielere ula�amaz
                    Name =".ShopApp.Security.Cookie"
                };
            });


            services.AddScoped<IProductDal, EfProductDal>();
            services.AddScoped<ICategoryDal, EfCoreCategoryDal>();
            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddMvc(options => options.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                SeedDatabase.Seed();
            }
            app.UseStaticFiles();
            app.CustomStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                 name: "adminProducts",
                 template: "admin/products",
                 defaults: new { controller = "Admin", action = "ProductList" }
               );

                routes.MapRoute(
                    name: "adminProductsEdit",
                    template: "admin/products/{id?}",
                    defaults: new { controller = "Admin", action = "EditProduct" }
                );

                routes.MapRoute(
                  name: "products",
                  template: "products/{category?}",
                  defaults: new { controller = "Shop", action = "List" }
                );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                );

            });

        }
    }
}
