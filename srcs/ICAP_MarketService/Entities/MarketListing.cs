using ICAP_Infrastructure.Entities;
using System.ComponentModel;

namespace ICAP_MarketService.Entities
{
    public class MarketListing : IEntity
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public ServiceCategory Category { get; set; }
        public string ImageLink { get; set; }
        public DateTimeOffset CreatedDateTime { get; set; }
    }

    public enum ServiceCategory
    {
        [Description("Gearbox Upgrade")] GearboxUpgrade,
        [Description("Full Replica Build")] FullReplicaBuild
    }
}
