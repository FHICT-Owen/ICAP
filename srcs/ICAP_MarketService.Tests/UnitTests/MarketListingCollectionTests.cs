using System.Linq.Expressions;
using ICAP_Infrastructure.Repositories;
using ICAP_Infrastructure.Types;
using ICAP_MarketService.Collections;
using ICAP_MarketService.Controllers;
using ICAP_MarketService.Entities;
using MongoDB.Driver;
using Moq;

namespace ICAP_MarketService.Tests.UnitTests;

public class MarketListingCollectionTests
{
    private readonly IRepository<MarketListing> _mockRepository;

    public MarketListingCollectionTests()
    {
        var mockDatabase = new Mock<IMongoDatabase>();
        var mockRepository = new Mock<MongoRepository<MarketListing>>(mockDatabase.Object, "");

        var sampleList = new List<MarketListing>
        {
            new()
            {
                Id = "0",
                Title = "Test1",
                Category = ServiceCategory.FullReplicaBuild,
                Description = "This is the description of a full test entity",
                CreatedDateTime = DateTimeOffset.Now,
                ImageLink = "https://test.com/image",
                Price = 10.99,
                UserId = "1"
            },
            new()
            {
                Id = "1",
                Title = "Test2",
                Category = ServiceCategory.FullReplicaBuild,
                Description = "This is the description of a full test entity",
                CreatedDateTime = DateTimeOffset.Now,
                ImageLink = "https://test.com/image",
                Price = 12.99,
                UserId = "1"
            }
        };

        mockRepository.Setup(func => func.GetAllAsync()).ReturnsAsync(sampleList);
        mockRepository.Setup(func => func.GetAllAsync(It.IsAny<Expression<Func<MarketListing, bool>>>()))
            .ReturnsAsync((Expression<Func<MarketListing, bool>> filter) =>
            {
                var filteredList = sampleList.Where(filter.Compile()).ToList();
                return filteredList;
            });
        mockRepository.Setup(func => func.GetAsync(It.IsAny<string>()))
            .ReturnsAsync((string filter) =>
            {
                var item = sampleList.FirstOrDefault(item => item.Id == filter);
                return item;
            });
        mockRepository.Setup(func => func.GetAsync(It.IsAny<Expression<Func<MarketListing, bool>>>()))
            .ReturnsAsync((Expression<Func<MarketListing, bool>> filter) =>
            {
                var item = sampleList.FirstOrDefault(filter.Compile());
                return item;
            });
        mockRepository.Setup(func => func.CreateAsync(It.IsAny<MarketListing>()));
        mockRepository.Setup(func => func.UpdateAsync(It.IsAny<MarketListing>()));
        mockRepository.Setup(func => func.RemoveAsync(It.IsAny<string>()));
        mockRepository.Setup(func => func.RemoveAsync(It.IsAny<Expression<Func<MarketListing, bool>>>()));

        _mockRepository = mockRepository.Object;
    }

    private MarketListingCollection CreateMarketListingCollection()
    {
        return new MarketListingCollection(_mockRepository);
    }

    [Fact]
    public async Task GetAllListingsAsync_With2Listings_Return2Listings()
    {
        // Arrange
        var marketListingCollection = CreateMarketListingCollection();

        // Act
        var result = await marketListingCollection.GetAllListingsAsync();

        // Assert
        Assert.True(result.Count == 2);
    }

    [Fact]
    public async Task GetListingsAsync_With2Listings_ReturnOneListing()
    {
        // Arrange
        var marketListingCollection = CreateMarketListingCollection();

        // Act
        var result = await marketListingCollection.GetListingAsync("0");

        // Assert
        Assert.Equal("0", result?.Id);
    }

    [Fact]
    public async Task AddListingAsync_WithNewValidListing_ReturnsSuccessResult()
    {
        // Arrange
        var marketListingCollection = CreateMarketListingCollection();
        var newListing = new MarketListingDto("Test3", "Lorum Ipsum", 15.99, ServiceCategory.GearboxUpgrade, "Reeee");

        // Act
        var result = await marketListingCollection.AddListingAsync(newListing, "1");

        // Assert
        Assert.IsType<SuccessResult<MarketListing>>(result.Value);
    }

    [Fact]
    public async Task AddListingAsync_WithDuplicateListing_ReturnsFailureResult()
    {
        // Arrange
        var marketListingCollection = CreateMarketListingCollection();
        var newListing = new MarketListingDto("Test1", "Lorum Ipsum", 15.99, ServiceCategory.GearboxUpgrade, "Reeee");

        // Act
        var result = await marketListingCollection.AddListingAsync(newListing, "1");

        // Assert
        Assert.IsType<FailureResult>(result.Value);
    }


    [Fact]
    public async Task EditListingAsync_WhereIdExistsAndEntityIsValid_ReturnsSuccessResult()
    {
        // Arrange
        var marketListingCollection = CreateMarketListingCollection();
        var updatedListing = new MarketListingDto("Test3", "Lorum Ipsum", 15.99, ServiceCategory.GearboxUpgrade, "Reeee");

        // Act
        var result = await marketListingCollection.EditListingAsync(updatedListing, "1");

        // Assert
        Assert.IsType<SuccessResult<string>>(result.Value);
    }

    [Fact]
    public async Task EditListingAsync_WhereIdDoesNotExist_ReturnsFailureResult()
    {
        // Arrange
        var marketListingCollection = CreateMarketListingCollection();
        var updatedListing = new MarketListingDto("Test3", "Lorum Ipsum", 15.99, ServiceCategory.GearboxUpgrade, "Reeee");

        // Act
        var result = await marketListingCollection.EditListingAsync(updatedListing, "2");

        // Assert
        Assert.IsType<FailureResult>(result.Value);
    }

    [Fact]
    public async Task RemoveListingAsync_WhereItemIdExists_ReturnsTrue()
    {
        // Arrange
        var marketListingCollection = CreateMarketListingCollection();

        // Act
        var result = await marketListingCollection.RemoveListingAsync("1");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task RemoveListingAsync_WhereItemIdDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var marketListingCollection = CreateMarketListingCollection();

        // Act
        var result = await marketListingCollection.RemoveListingAsync("2");

        // Assert
        Assert.False(result);
    }
}