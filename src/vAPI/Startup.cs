/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using VRP.DAL.Database;
using VRP.DAL.Database.Models.Account;
using VRP.DAL.Database.Models.Building;
using VRP.DAL.Database.Models.Character;
using VRP.DAL.Database.Models.CrimeBot;
using VRP.DAL.Database.Models.Group;
using VRP.DAL.Database.Models.Item;
using VRP.DAL.Database.Models.Misc;
using VRP.DAL.Database.Models.Telephone;
using VRP.DAL.Database.Models.Vehicle;
using VRP.DAL.Database.Models.Warehouse;
using VRP.DAL.Interfaces;
using VRP.DAL.Repositories;

namespace VRP.vAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<RoleplayContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("gameConnectionString"));
            });

            // scoped
            services.AddScoped(factory => Configuration);

            // scoped repositories
            services.AddScoped<IJoinableRepository<AccountModel>, AccountsRepository>();
            services.AddScoped<IJoinableRepository<BuildingModel>, BuildingsRepository>();
            services.AddScoped<IJoinableRepository<CharacterModel>, CharactersRepository>();
            services.AddScoped<IJoinableRepository<CrimeBotModel>, CrimeBotsRepository>();
            services.AddScoped<IJoinableRepository<GroupModel>, GroupsRepository>();
            services.AddScoped<IJoinableRepository<GroupWarehouseItemModel>, GroupWarehouseItemsRepository>();
            services.AddScoped<IJoinableRepository<GroupWarehouseOrderModel>, GroupWarehouseOrdersRepository>();
            services.AddScoped<IJoinableRepository<ItemModel>, ItemsRepository>();
            services.AddScoped<IJoinableRepository<PenaltyModel>, PenaltiesRepository>();
            services.AddScoped<IJoinableRepository<TelephoneContactModel>, TelephoneContactsRepository>();
            services.AddScoped<IRepository<TelephoneMessageModel>, TelephoneMessagesRepository>();
            services.AddScoped<IJoinableRepository<VehicleModel>, VehiclesRepository>();
            services.AddScoped<IJoinableRepository<WorkerModel>, WorkersRepository>();
            services.AddScoped<IRepository<ZoneModel>, ZonesRepository>();

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.LoginPath = "/api/account/login";
                    options.LogoutPath = "/api/account/logout";
                    options.Cookie.Name = ".AspNet.VRP";
                    options.Cookie.Expiration = TimeSpan.FromDays(1);
                    options.Cookie.HttpOnly = true;
                });

            services.AddCors(options =>
            {
                options.AddPolicy("dev", builder =>
                {
                    builder.WithOrigins("http://localhost:4200", "https://localhost:4200", "http://localhost");
                    builder.WithHeaders("accept", "content-type", "origin");
                    builder.WithMethods("get", "post", "put", "delete");
                    builder.AllowCredentials();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            CookiePolicyOptions cookiePolicyOptions = new CookiePolicyOptions
            {
                HttpOnly = HttpOnlyPolicy.Always,
                MinimumSameSitePolicy = SameSiteMode.Strict
            };
            app.UseCookiePolicy(cookiePolicyOptions);
            app.UseMvc();

            app.UseCors("dev");
        }
    }
}
