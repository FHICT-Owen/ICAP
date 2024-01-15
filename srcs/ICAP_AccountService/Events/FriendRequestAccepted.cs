using ICAP_AccountService.Entities;
using ICAP_Infrastructure.Repositories;
using ICAP_ServiceBus;

namespace ICAP_AccountService.Events
{
    public class FriendRequestAccepted
    {
        private record FriendRequestAcceptedData (string UserId, string UserToAdd);
        private readonly IRepository<User> _usersRepository;
        public FriendRequestAccepted(IBusHandler busHandler, IRepository<User> usersRepository)
        {
            _usersRepository = usersRepository;
            busHandler.CreateBusHandler<FriendRequestAcceptedData>("friendrequesttopic", "friendrequestsub", ProcessMessage);
        }

        private async Task ProcessMessage(FriendRequestAcceptedData data)
        {
            var user = await _usersRepository.GetAsync(data.UserId);
            await _usersRepository.UpdateAsync(user);
        }
    }
}
