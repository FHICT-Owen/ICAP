using ICAP_AccountService.Entities;
using ICAP_Infrastructure.Repositories;
using MassTransit;

namespace ICAP_AccountService.Events
{
    public record FriendRequestAcceptedData(string UserId, string UserToAdd);
    public class FriendRequestAccepted : IConsumer<FriendRequestAcceptedData>
    {
        private readonly IRepository<User> _usersRepository;
        public FriendRequestAccepted( IRepository<User> usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task Consume(ConsumeContext<FriendRequestAcceptedData> ctx)
        {
            var user = await _usersRepository.GetAsync(ctx.Message.UserId);
            if (user == null) return;
            await _usersRepository.UpdateAsync(user);
        }
    }
}
