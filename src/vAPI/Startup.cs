/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using VRP.Core.Database;
using VRP.Core.Database.Models;
using VRP.Core.Database.Models.Account;
using VRP.Core.Database.Models.Building;
using VRP.Core.Database.Models.Character;
using VRP.Core.Database.Models.CrimeBot;
using VRP.Core.Database.Models.Group;
using VRP.Core.Database.Models.Item;
using VRP.Core.Database.Models.Misc;
using VRP.Core.Database.Models.Telephone;
using VRP.Core.Database.Models.Vehicle;
using VRP.Core.Database.Models.Warehouse;
using VRP.Core.Interfaces;
using VRP.Core.Repositories;
using VRP.Core.Services;
using VRP.Core.Services.LogInBroadcaster;
using VRP.Core.Services.LogInWatcher;
using VRP.Core.Services.UserStorage;
using VRP.Core.Tools;

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
            //// decreases time of first usage by 2000 ms
            //ForumDatabaseHelper.CheckPasswordMatch("admin@v-santos.pl", "qweqwe", out ForumUser forumUser);

            services.AddDbContext<RoleplayContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("gameConnectionString"));
            });

            // singletons
            services.AddSingleton<IUsersStorageService>(new UsersStorageService());

            // scoped
            services.AddScoped(factory => Configuration);

            // scoped repositories
            services.AddScoped<IRepository<AccountModel>, AccountsRepository>();
            services.AddScoped<IRepository<BuildingModel>, BuildingsRepository>();
            services.AddScoped<IRepository<CharacterModel>, CharactersRepository>();
            services.AddScoped<IRepository<CrimeBotModel>, CrimeBotsRepository>();
            services.AddScoped<IRepository<GroupModel>, GroupsRepository>();
            services.AddScoped<IRepository<GroupWarehouseItemModel>, GroupWarehouseItemsRepository>();
            services.AddScoped<IRepository<GroupWarehouseOrderModel>, GroupWarehouseOrdersRepository>();
            services.AddScoped<IRepository<ItemModel>, ItemsRepository>();
            services.AddScoped<IRepository<PenaltyModel>, PenaltiesRepository>();
            services.AddScoped<IRepository<TelephoneContactModel>, TelephoneContactsRepository>();
            services.AddScoped<IRepository<TelephoneMessageModel>, TelephoneMessagesRepository>();
            services.AddScoped<IRepository<VehicleModel>, VehiclesRepository>();
            services.AddScoped<IRepository<WorkerModel>, WorkersRepository>();
            services.AddScoped<IRepository<ZoneModel>, ZonesRepository>();

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin", builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
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

            app.UseMvc();
            app.UseCors("AllowAnyOrigin");
        }
    }
}
