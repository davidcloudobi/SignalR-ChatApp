using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminService.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SignalR;
using SignalR.JWT;

namespace ChatApp
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
            services.AddControllers();
            services.AddDbContext<ChatDbContent>(opt => {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddDefaultIdentity<UserModel>()
                .AddEntityFrameworkStores<ChatDbContent>()
                .AddSignInManager<SignInManager<UserModel>>();


            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
               
                options.TokenValidationParameters = new TokenValidationParameters
                {
                  
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                  

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenKey"])) //Configuration["JwtToken:SecretKey"]
                };

                //options.Events = new JwtBearerEvents
                //{
                //    OnMessageReceived = context =>
                //    {
                //        var accessToken = context.Request.Query["access_token"];

                //        // If the request is for our hub...
                //        var path = context.HttpContext.Request.Path;
                //        if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/chatHub")))
                //        {
                //            // Read the token out of the query string
                //            context.Token = accessToken;
                //        }
                //        return Task.CompletedTask;
                //    }
                //};

                //options.Events = new JwtBearerEvents
                //{
                //    OnMessageReceived = context =>
                //    {
                //        var accessToken = context.Request.Query["access_token"];


                //        // If the request is for our hub...
                //        var path = context.HttpContext.Request.Path;
                //        if (!string.IsNullOrEmpty(accessToken) &&
                //            (path.StartsWithSegments("/chathub")))
                //        {
                //            // Read the token out of the query string
                //            context.Token = accessToken;
                //        }
                //        return Task.CompletedTask;
                //    }
                //};

            });
            services.AddScoped<IJwtGenerator, JwtGenerator>();
            //services.AddAuthentication("CookieAuth")
            //    .AddCookie("CookieAuth", Config =>
            //    {
            //        Config.Cookie.Name = "User.Cookie";
            //        Config.LoginPath = "/auth/reg";
            //    });

            //#############################################################################
            //          services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //.AddCookie(options =>
            //{
            //    options.Cookie.HttpOnly = true;
            //    //options.Cookie.SecurePolicy = _environment.IsDevelopment()
            //    //  ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
            //    options.Cookie.SameSite = SameSiteMode.Lax;
            //    options.Cookie.Name = "User.AuthCookieAspNetCore";
            //    options.LoginPath = "/auth/reg";
            //    //options.LogoutPath = "/Home/Logout";
            //});

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    options.MinimumSameSitePolicy = SameSiteMode.Strict;
            //    options.HttpOnly = HttpOnlyPolicy.None;


            //    //options.Secure = _environment.IsDevelopment()
            //    //    ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
            //});
            //#############################################################################

            services.AddSignalR();
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200", "http://localhost:5000");
                });
            });

            services.AddScoped<IChatService, ChatService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();


             app.UseMiddleware<WebSocketsMiddleware>();
        
            app.UseAuthentication();
            // app.UseAuthentication();


            app.UseAuthorization();
            app.UseCors("CorsPolicy");
           

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHub<ChatHub>("/chatHub");
            });
        }
    }
}
