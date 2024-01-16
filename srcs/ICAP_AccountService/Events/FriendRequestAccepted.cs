using ICAP_AccountService.Entities;
using ICAP_Infrastructure.Repositories;
using MassTransit;

namespace ICAP_AccountService.Events
{
    public record FriendRequestAcceptedData(string UserId, string UserToAdd);
    public class FriendRequestAccepted(IRepository<User> usersRepository) : IConsumer<FriendRequestAcceptedData>
    {
        public async Task Consume(ConsumeContext<FriendRequestAcceptedData> ctx)
        {
            var user = await usersRepository.GetAsync(ctx.Message.UserId);
            if (user == null) return;
            await usersRepository.UpdateAsync(user);
        }
    }
}
