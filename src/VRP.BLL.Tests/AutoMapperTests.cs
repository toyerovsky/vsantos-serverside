using AutoMapper;
using AutoMapper.EquivalencyExpression;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using VRP.BLL.Dto;
using VRP.BLL.Extensions;
using VRP.DAL.Database.Models.Account;
using VRP.DAL.Database.Models.Building;
using VRP.DAL.Database.Models.Character;
using VRP.DAL.Database.Models.Group;
using VRP.DAL.Database.Models.Item;
using VRP.DAL.Database.Models.Ticket;
using VRP.DAL.Database.Models.Vehicle;
using VRP.DAL.Enums;

namespace VRP.BLL.Tests
{
    [TestFixture]
    public class AutoMapperTests
    {
        private readonly IMapper _mapper;

        public AutoMapperTests()
        {
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

            _mapper = config.CreateMapper();
        }

        [Test]
        public void ShouldNot_MapNullProperties()
        {
            var characterModel = new CharacterModel()
            {
                Id = 1,
                Name = "Test"
            };

            var workerModel = new WorkerModel()
            {
                Id = 1,
                Character = characterModel
            };

            var groupRank = new GroupRankModel()
            {
                Id = 1,
                Workers = new HashSet<WorkerModel>()
                {
                    workerModel
                }
            };

            GroupModel groupModel = new GroupModel()
            {
                GroupRanks = new HashSet<GroupRankModel>()
                {
                    groupRank
                }
            };

            workerModel.Group = groupModel;
            workerModel.GroupRank = groupRank;

            GroupDto groupDto = new GroupDto()
            {
                GroupRanks = new List<GroupRankDto>()
                {
                    new GroupRankDto()
                    {
                        Id = 1,
                        Workers = new List<WorkerDto>()
                        {
                            new WorkerDto()
                            {
                                Id = 1
                            }
                        }
                    }
                }
            };

            _mapper.Map(groupDto, groupModel);

            var final = groupModel.GroupRanks.ElementAt(0).Workers.ElementAt(0);
            Assert.That(final.Character != null && final.Group != null);
        }
    }
}
