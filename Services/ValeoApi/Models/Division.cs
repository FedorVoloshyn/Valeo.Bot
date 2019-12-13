namespace Valeo.Bot.Services.ValeoApi.Models
{
    public class Division
    {
        public string StructureId { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public Addresses Addresses { get; set; }
        public string PropertyType { get; set; }    
    }
}