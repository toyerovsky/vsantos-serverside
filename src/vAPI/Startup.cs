/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using VRP.BLL.Dto;
using VRP.BLL.Extensions;
using VRP.BLL.Interfaces;
using VRP.BLL.Mappers;
using VRP.BLL.Services;
using VRP.BLL.Services.Interfaces;
using VRP.DAL.Database;
using VRP.DAL.Database.Models.Account;
using VRP.DAL.Database.Models.Building;
using VRP.DAL.Database.Models.Character;
using VRP.DAL.Database.Models.Group;
using VRP.DAL.Database.Models.Item;
using VRP.DAL.Database.Models.Ticket;
using VRP.DAL.Database.Models.Vehicle;
using VRP.DAL.Enums;
using VRP.DAL.UnitOfWork;
using VRP.vAPI.Helpers;

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
            var t = Configuration.GetSection("Ranks:SupportRanks").Get<string[]>();

            services.AddDbContext<RoleplayContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("gameConnectionString"));
            });

            // scoped
            services.AddScoped(factory => Configuration);

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // services
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IBuildingService, BuildingService>();
            services.AddScoped<ICharacterService, CharacterService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IGroupRankService, GroupRankService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IPenaltyService, PenaltyService>();
            services.AddScoped<IVehicleService, VehicleService>();

            // scoped mappers
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AccountModel, AccountDto>()
                    .ForMember(
                        accountDto => accountDto.ServerRank,
                        opt => opt.ResolveUsing((model, dto) => model.ServerRank.GetDescription()))
                    .ForMember(accountDto => accountDto.PasswordSalt,
                        opt => opt.ResolveUsing((model, dto) => model.PasswordHash.Substring(0, 29)))
                    .ReverseMap();
                cfg.CreateMap<CharacterModel, CharacterDto>().ReverseMap();
                cfg.CreateMap<BuildingModel, BuildingDto>().ReverseMap();
                cfg.CreateMap<GroupModel, GroupDto>().ReverseMap();
                cfg.CreateMap<GroupRankModel, GroupRankDto>().ReverseMap();
                cfg.CreateMap<ItemModel, ItemDto>().ReverseMap();
                cfg.CreateMap<PenaltyModel, PenaltyDto>().ReverseMap();
                cfg.CreateMap<VehicleModel, VehicleDto>().ReverseMap();
                cfg.CreateMap<WorkerModel, WorkerDto>().ReverseMap();
                cfg.CreateMap<TicketModel, TicketDto>()
                    .ForMember(
                        ticketDto => ticketDto.Status,
                        opt => opt.ResolveUsing((model, dto) => model.Status.GetDescription()))
                    .ForMember(
                        ticketDto => ticketDto.Type,
                        opt => opt.ResolveUsing((model, dto) => model.Type.GetDescription()))
                    .ReverseMap();
                cfg.CreateMap<TicketMessageModel, TicketMessageDto>();

            });

            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            services.AddScoped<IMapper<ServerRank, long>, ServerRankMapper>();

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/login";
                    options.LogoutPath = "/logout";
                    options.Cookie.Name = ".AspNet.VRP";
                    options.Cookie.Expiration = TimeSpan.FromDays(1);
                    options.Cookie.HttpOnly = true;
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Authenticated", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
                });
                options.AddPolicy("Support", policy => { policy.RequireRole(RoleHelper.GetFromSupportRoles()); });
                options.AddPolicy("Admin", policy => { policy.RequireRole(RoleHelper.GetFromAdminRoles()); });
                options.AddPolicy("Dev", policy => { policy.RequireRole(RoleHelper.GetFromDevRoles()); });
                options.AddPolicy("Management", policy => { policy.RequireRole(RoleHelper.ManagementRoles); });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("dev", builder =>
                {
                    builder.WithOrigins("http://localhost:4200", "https://localhost:4200", "http://localhost", "http://*", "http://0.0.0.0", "http://+");
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

