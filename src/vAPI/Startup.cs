/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using VRP.Core.Database;
using VRP.Core.Database.Models;
using VRP.Core.Interfaces;
using VRP.Core.Repositories;
using VRP.Core.Services;
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

            UsersStorageService storageService;
            UsersWatcherService usersWatcherService;
            UsersBroadcasterService usersBroadcasterService = null;

            // singletons
            services.AddSingleton<IUsersStorageService>(storageService = new UsersStorageService());
            services.AddSingleton<IUsersWatcherService>(usersWatcherService = new UsersWatcherService(Configuration, new DebugLogger(),
                (o, e) => storageService.Login(e.Token, e.AccountId),
                (o, e) => storageService.LogOut(e.Token)));
            services.AddSingleton<IUserBroadcasterService>(usersBroadcasterService = new UsersBroadcasterService(Configuration, new DebugLogger()));

            // prevents socket exceptions
            // connect to socked after connection with game server is established
            void OnConnectionEstablished(object sender, EventArgs e)
            {
                usersBroadcasterService.Prepare();
                usersWatcherService.ConnectionEstablished -= OnConnectionEstablished;
            }

            usersWatcherService.ConnectionEstablished += OnConnectionEstablished;

            // scoped
            services.AddScoped(factory => Configuration);
            services.AddScoped<IRepository<CharacterModel>, CharactersRepository>();
            services.AddScoped<IRepository<AccountModel>, AccountsRepository>();

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
