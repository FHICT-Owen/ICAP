using ICAP_Infrastructure.MTDtos;
using ICAP_Infrastructure.Repositories;
using ICAP_MarketService.Entities;
using MassTransit;

namespace ICAP_MarketService.Events
{
    public class DeleteUserDataEvent(IRepository<MarketListing> marketRepository) : IConsumer<DeleteUserData>
    {
        public async Task Consume(ConsumeContext<DeleteUserData> ctx)
        {
            Console.WriteLine($"Removing data for user {ctx.Message.UserId}");
            await marketRepository.RemoveAsync(item => item.UserId == ctx.Message.UserId);
        }
    }
}
