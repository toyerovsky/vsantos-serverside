/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using VRP.Core.Database;
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
using VRP.Core.Services.UserStorage;

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

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

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

            app.UseAuthentication();
            app.UseCookiePolicy();
            app.UseMvc();
            app.UseCors("AllowAnyOrigin");
        }
    }
}
