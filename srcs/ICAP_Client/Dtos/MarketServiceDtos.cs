using System.ComponentModel;

namespace ICAP_Client.Dtos
{
    public record MarketListingDto(string Title, string Description, double Price, ServiceCategory Category, string ImageLink);
    public enum ServiceCategory
    {
        [Description("Gearbox Upgrade")] GearboxUpgrade,
        [Description("Full Replica Build")] FullReplicaBuild
    }
}
