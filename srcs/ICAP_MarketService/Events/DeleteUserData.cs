using ICAP_Infrastructure.MTDtos;
using ICAP_Infrastructure.Repositories;
using ICAP_MarketService.Entities;
using MassTransit;

namespace ICAP_MarketService.Events
{
    public class DeleteUserDataEvent(IRepository<MarketListing> marketRepository, ILogger<DeleteUserDataEvent> logger) : IConsumer<DeleteUserData>
    {
        public async Task Consume(ConsumeContext<DeleteUserData> ctx)
        {
            logger.LogInformation($"Removing data for user {ctx.Message.UserId}");
            await marketRepository.RemoveAsync(item => item.UserId == ctx.Message.UserId);
        }
    }
}
