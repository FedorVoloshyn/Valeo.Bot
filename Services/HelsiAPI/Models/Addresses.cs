using System;
using System.Collections.Generic;

namespace Valeo.Bot.Services.HelsiAPI.Models
{
    public class Addresses
    {
        public string StructureId { get; set; }
        public Address Address { get; set; }
        public string ShortName { get; set; }
        public string Parent { get; set; }
        public List<string> Integrations { get; set; }
        public string Area { get; set; }
        public Rating rating { get; set; }
        public List<int> AllowedSystems { get; set; }
        public string PropertyType { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
        public int Sort { get; set; }
        public bool Active { get; set; }
        public bool Draft { get; set; }
        public bool Delete { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class Address
    {
        public string AddressId { get; set; }
        public string TypeEntity { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public long AddressIdAPI { get; set; }
        public string AddressText { get; set; }
    }
}