using ICAP_Infrastructure.MTDtos;
using ICAP_Infrastructure.Repositories;
using ICAP_RelationService.Entities;
using MassTransit;


namespace ICAP_RelationService.Events
{
    public class DeleteUserDataEvent(IRepository<Friends> friendRepository, IRepository<FriendRequest> friendRequestRepository) : IConsumer<DeleteUserData>
    {
        public async Task Consume(ConsumeContext<DeleteUserData> ctx)
        {
            Console.WriteLine($"Removing data for user {ctx.Message}");
            await friendRepository.RemoveAsync(ctx.Message.UserId);
            await friendRequestRepository.RemoveAsync(filter =>
                filter.UserFrom == ctx.Message.UserId || filter.UserTo == ctx.Message.UserId);
        }
    }
}
