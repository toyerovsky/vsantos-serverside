/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using VRP.Core.Database;
using VRP.Core.Database.Forum;
using VRP.Core.Database.Models;
using VRP.Core.Interfaces;
using VRP.Core.Repositories;
using VRP.vAPI.Game.Services;

namespace VRP.vAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }
        public static IUsersWatcherService UsersWatcher { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // decreases time of first usage by 2000 ms
            ForumDatabaseHelper.CheckPasswordMatch("admin@v-santos.pl", "qweqwe", out ForumUser forumUser);

            services.AddDbContext<RoleplayContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("gameConnectionString"));
            });

            UsersStorageService storageService;

            // singleton
            services.AddSingleton<IUsersStorageService>(storageService = new UsersStorageService());

            // users watcher
            UsersWatcher = new UsersWatcherService(storageService);

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
