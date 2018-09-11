/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using AutoMapper;
using AutoMapper.EquivalencyExpression;
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
using System;
using System.Threading.Tasks;
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
            services.AddScoped<IWorkerService, WorkerService>();

            // scoped mappers
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddCollectionMappers();

                #region Account

                cfg.CreateMap<AccountModel, AccountDto>()
                    .PreserveReferences()
                    .EqualityComparison((model, dto) => model.Id == dto.Id)
                    .ForMember(
                        accountDto => accountDto.ServerRank,
                        opt => opt.ResolveUsing((model, dto) => model.ServerRank.GetDescription()))
                    .ForMember(
                        accountDto => accountDto.PasswordSalt,
                        opt => opt.ResolveUsing((model, dto) => model.PasswordHash.Substring(0, 29)))
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                cfg.CreateMap<AccountDto, AccountModel>()
                    .PreserveReferences()
                    .EqualityComparison((dto, model) => model.Id == dto.Id)
                    .ForMember(
                        accountModel => accountModel.ServerRank,
                        opt => opt.ResolveUsing(dto => dto.ServerRank.FromDescription<ServerRank>()))
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                #endregion

                #region Character

                cfg.CreateMap<CharacterModel, CharacterDto>()
                    .PreserveReferences()
                    .EqualityComparison((model, dto) => model.Id == dto.Id)
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                cfg.CreateMap<CharacterDto, CharacterModel>()
                    .PreserveReferences()
                    .EqualityComparison((dto, model) => model.Id == dto.Id)
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                #endregion

                #region Building

                cfg.CreateMap<BuildingModel, BuildingDto>()
                    .PreserveReferences()
                    .EqualityComparison((model, dto) => model.Id == dto.Id)
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                cfg.CreateMap<BuildingDto, BuildingModel>()
                    .PreserveReferences()
                    .EqualityComparison((dto, model) => model.Id == dto.Id)
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                #endregion

                #region Group

                cfg.CreateMap<GroupModel, GroupDto>()
                    .PreserveReferences()
                    .EqualityComparison((model, dto) => model.Id == dto.Id)
                    .ForMember(
                        groupDto => groupDto.GroupType,
                        opt => opt.ResolveUsing((model, dto) => model.GroupType.GetDescription()))
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                cfg.CreateMap<GroupDto, GroupModel>()
                    .PreserveReferences()
                    .EqualityComparison((dto, model) => model.Id == dto.Id)
                    .ForMember(
                        groupModel => groupModel.GroupType,
                        opt => opt.MapFrom(dto => dto.GroupType.FromDescription<GroupType>()))
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                #endregion

                #region Group Rank

                cfg.CreateMap<GroupRankModel, GroupRankDto>()
                    .PreserveReferences()
                    .EqualityComparison((model, dto) => model.Id == dto.Id)
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                cfg.CreateMap<GroupRankDto, GroupRankModel>()
                    .PreserveReferences()
                    .EqualityComparison((dto, model) => model.Id == dto.Id)
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                #endregion

                #region Item

                cfg.CreateMap<ItemModel, ItemDto>()
                    .PreserveReferences()
                    .EqualityComparison((model, dto) => model.Id == dto.Id)
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                cfg.CreateMap<ItemDto, ItemModel>()
                    .PreserveReferences()
                    .EqualityComparison((dto, model) => model.Id == dto.Id)
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                #endregion

                #region Penalty

                cfg.CreateMap<PenaltyModel, PenaltyDto>()
                    .PreserveReferences()
                    .EqualityComparison((model, dto) => model.Id == dto.Id)
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                cfg.CreateMap<PenaltyDto, PenaltyModel>()
                    .PreserveReferences()
                    .EqualityComparison((dto, model) => model.Id == dto.Id)
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                #endregion

                #region Vehicle

                cfg.CreateMap<VehicleModel, VehicleDto>()
                    .PreserveReferences()
                    .EqualityComparison((model, dto) => model.Id == dto.Id)
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                cfg.CreateMap<VehicleDto, VehicleModel>()
                    .PreserveReferences()
                    .EqualityComparison((dto, model) => model.Id == dto.Id)
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                #endregion

                #region Worker

                cfg.CreateMap<WorkerModel, WorkerDto>()
                    .PreserveReferences()
                    .EqualityComparison((model, dto) => model.Id == dto.Id)
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                cfg.CreateMap<WorkerDto, WorkerModel>()
                    .PreserveReferences()
                    .EqualityComparison((dto, model) => model.Id == dto.Id)
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                #endregion

                #region Ticket

                cfg.CreateMap<TicketModel, TicketDto>()
                    .PreserveReferences()
                    .EqualityComparison((model, dto) => model.Id == dto.Id)
                    .ForMember(
                        ticketDto => ticketDto.Status,
                        opt => opt.ResolveUsing((model, dto) => model.Status.GetDescription()))
                    .ForMember(
                        ticketDto => ticketDto.Type,
                        opt => opt.ResolveUsing((model, dto) => model.Type.GetDescription()))
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                cfg.CreateMap<TicketDto, TicketModel>()
                    .PreserveReferences()
                    .EqualityComparison((dto, model) => model.Id == dto.Id)
                    .ForMember(
                        ticketDto => ticketDto.Status,
                        opt => opt.ResolveUsing((model, dto) => model.Status.FromDescription<TicketStatusType>()))
                    .ForMember(
                        ticketDto => ticketDto.Type,
                        opt => opt.ResolveUsing((model, dto) => model.Type.FromDescription<TicketType>()))
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                #endregion

                #region Ticket Message

                cfg.CreateMap<TicketMessageModel, TicketMessageDto>()
                    .PreserveReferences()
                    .EqualityComparison((model, dto) => model.Id == dto.Id)
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                cfg.CreateMap<TicketMessageDto, TicketMessageModel>()
                    .PreserveReferences()
                    .EqualityComparison((dto, model) => model.Id == dto.Id)
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                #endregion
            });

            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            services.AddScoped<IMapper<ServerRank, long>, ServerRankMapper>();

            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Events.OnRedirectToLogin = ctx =>
                    {
                        ctx.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    };
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

