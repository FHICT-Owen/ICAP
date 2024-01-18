using ICAP_Infrastructure.Repositories;
using ICAP_Infrastructure.Types;
using ICAP_MarketService.Controllers;
using ICAP_MarketService.Entities;
using OneOf;

namespace ICAP_MarketService.Collections
{
    public class MarketListingCollection(IRepository<MarketListing> listingRepository)
    {
        public async Task<IReadOnlyCollection<MarketListing>> GetAllListingsAsync()
        {
            return await listingRepository.GetAllAsync();
        }

        public async Task<MarketListing?> GetListingAsync(string id)
        {
            return await listingRepository.GetAsync(id);
        }

        public async Task<OneOf<SuccessResult<MarketListing>, FailureResult>> AddListingAsync(MarketListingDto reqData, string userId)
        {
            var data = new MarketListing
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                Title = reqData.Title,
                Description = reqData.Description,
                CreatedDateTime = DateTimeOffset.Now,
                Price = reqData.Price,
                Category = reqData.Category,
                ImageLink = reqData.ImageLink
            };
            var userResult = await listingRepository.GetAllAsync(listing => listing.Title == reqData.Title && listing.UserId == userId);
            if (userResult.Count > 0) return new FailureResult("A listing for your user with the same title already exists.");
            await listingRepository.CreateAsync(data);
            return new SuccessResult<MarketListing>(data);
        }

        public async Task<OneOf<SuccessResult<string>, FailureResult>> EditListingAsync(MarketListingDto reqData, string id)
        {
            var existingItem = await listingRepository.GetAsync(id);
            if (existingItem is null) return new FailureResult("Could not find listing to edit");
            existingItem.Title = reqData.Title;
            existingItem.Description = reqData.Description;
            existingItem.Price = reqData.Price;
            existingItem.Category = reqData.Category;
            existingItem.ImageLink = reqData.ImageLink;

            await listingRepository.UpdateAsync(existingItem);
            return new SuccessResult<string>("Edited successfully!");
        }

        public async Task<bool> RemoveListingAsync(string id)
        {
            var existingItem = await listingRepository.GetAsync(id);
            if (existingItem is null) return false;
            await listingRepository.RemoveAsync(id);
            return true;
        }
    }
}
