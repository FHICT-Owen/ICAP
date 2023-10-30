namespace ICAP_MarketService
{
    public record ListingDto(Guid Id, string Name, string Description, bool IsActiveListing, decimal Price, 
        DateTimeOffset CreatedDate);

    public record CreateListingDto(string Name, string Description, bool IsActiveListing, decimal Price);

    public record UpdateListingDto(string Name, string Description, bool IsActiveListing, decimal Price);
}
