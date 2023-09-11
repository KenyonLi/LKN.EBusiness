using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Minio;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace YDT.EBusiness
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
            services.AddControllersWithViews();
            services.AddHttpClient();
            services.AddSingleton(new MinioClient("localhost:9000", "minioadmin", "minioadmin"));

            // 如何做成模块
            // 1、扩展方法
            // 2、需要程序集

            //配置Cookie决策
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                options.OnAppendCookie = cookieContext =>
                    SetSameSite(cookieContext.Context, cookieContext.CookieOptions);
                options.OnDeleteCookie = cookieContext =>
                    SetSameSite(cookieContext.Context, cookieContext.CookieOptions);
            });


            // 1、添加身份认证信息
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            // 1.1 添加可以处理cookie的处理程序
            .AddCookie("Cookies")
            // 1.2 重定向到IdentityServer4登录页面
            .AddOpenIdConnect("oidc", options =>
            {
                //认证服务器
                options.Authority = "http://localhost:5005";    // 受信任令牌服务地址
                options.RequireHttpsMetadata = false;
                options.ClientId = "client-code";
                options.ClientSecret = "secret";
                options.ResponseType = "code";
                options.SaveTokens = true;  // 用于将来自IdentityServer的令牌保留在cookie中 cookie 和 httpContext
                // 1、添加授权访问api的支持
                options.Scope.Add("offline_access");
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("OrderService");
            });
        }

        public void SetSameSite(HttpContext httpContext, CookieOptions options)
        {
            if (options.SameSite == SameSiteMode.None)
            {
                //检测如果不是HTTPS，则移除SameSite属性
                if (httpContext.Request.Scheme != "https")
                {
                    //设置为Unspecified，则不会添加SameSite属性
                    options.SameSite = SameSiteMode.Unspecified;
                }
            }
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
             //使用Cookie决策
            app.UseCookiePolicy();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            // 2、开启身份验证
            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
