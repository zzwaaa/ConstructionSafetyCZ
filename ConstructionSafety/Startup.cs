using Common.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NSwag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using WisdomDbCore.WisdomModels;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Models.Caching;

namespace ConstructionSafety
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
            // 配置跨域
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSameDomain", build =>
                {
                    build.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });

            // OssFileSettings
            services.Configure<OssFileSetting>(Configuration.GetSection("OssFileSettings"));
            OssFileSetting ossFileSet = new OssFileSetting();
            Configuration.Bind("OssFileSettings", ossFileSet);
            // jwt
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            JwtSettings setting = new JwtSettings();
            Configuration.Bind("JwtSettings", setting);

            services.AddHttpClient();
            services
                .AddSwaggerDocument(config =>
                {
                    config.Title = "常州人保 接口";
                    config.Description = "常州人保业务接口";
                    config.Version = "2.0.0";
                    config.PostProcess = document =>
                    {
                        document.Info.Contact = new OpenApiContact
                        {
                            //Name = "My Company",
                            //Email = "info@mycompany.com",
                            //Url = "https://www.mycompany.com"
                        };
                    };
                });

            #region PostgreSql
            //services.AddDbContext<InterfaceSysDBContext>(
            //    option => option.UseNpgsql(Configuration.GetConnectionString("InterfaceSysDBContext")));
            //智慧监管数据库
            //services.AddDbContext<WisdomPlatDBContext>(
            //    option => option.UseNpgsql(Configuration.GetConnectionString("WisdomPlatDBContext")));

            #endregion

            #region Sql Server
            services.AddDbContext<ProjLiefInsDBContext>(
                option => option.UseSqlServer(Configuration.GetConnectionString("CZLifeInsurance"),
                o=>o.MigrationsAssembly("ConstructionSafety")
                ));
            //services.AddDbContext<sanyo_acsContext>(
            //    option => option.UseSqlServer(Configuration.GetConnectionString("SanYoACS")));
            #endregion

            // services.AddSingleton<IQRCode, RaffQRCode>();
            // 开启目录浏览
            services.AddDirectoryBrowser();

            services.AddSingleton(typeof(ICacheService), new RedisCacheService(Configuration.GetSection("Redis:Configuration").Value));
            //redis缓存
            var csredis = new CSRedis.CSRedisClient(Configuration.GetSection("Redis:Configuration").Value);
            RedisHelper.Initialization(csredis);

            // 定时器
            //services.AddHangfire(x => x.UsePostgreSqlStorage(Configuration.GetConnectionString("InterfaceSysDBContext")));
            //services.AddHangfireServer();

            services.AddSignalR();

            services.AddControllers(option => {
                
                //统一格式化日期
                //option.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
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
            //开启目录浏览
            //app.UseDirectoryBrowser();
            app.UseStaticFiles();

            app.UseCors("AllowSameDomain");

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapHub<DustHub>("/dustHub");
                //endpoints.MapHub<PersonHub>("/personHub");
                //endpoints.MapHub<AlarmHub>("/alarmHub");
                //endpoints.MapHub<HatHub>("/hatHub");
                //endpoints.MapHub<PeopleHub>("/peopleHub");
            });
        }
    }
}
