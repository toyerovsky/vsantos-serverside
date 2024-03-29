﻿using System;
using System.Collections.Generic;
using VRP.DAL.Database.Models.Group;

namespace VRP.BLL.Dto
{
    public class CharacterDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool Online { get; set; }
        public float LastPositionX { get; set; }
        public float LastPositionY { get; set; }
        public float LastPositionZ { get; set; }
        public float LastRotationX { get; set; }
        public float LastRotationY { get; set; }
        public float LastRotationZ { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime BornDate { get; set; }
        public bool IsAlive { get; set; }
        public byte Health { get; set; }
        public bool Gender { get; set; }
        public decimal Money { get; set; }
        public decimal? BankMoney { get; set; }
        public AccountDto Account { get; set; }
        public int AccountId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime ImageUploadDate { get; set; }
        public string TodayPlayedTime { get; set; }
        public string PlayedTime { get; set; }

        public ICollection<VehicleDto> Vehicles { get; set; }
        public ICollection<ItemDto> Items { get; set; }
        public ICollection<BuildingDto> Buildings { get; set; }
        public ICollection<WorkerModel> Workers { get; set; }
    }
}