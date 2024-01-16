using ICAP_Infrastructure.Entities;

namespace ICAP_MarketService.Entities
{
    public class MarketListing : IEntity
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedDateTime { get; set; }
    }
}
