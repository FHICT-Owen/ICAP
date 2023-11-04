using ICAP_AccountService.Repositories;
using ICAP_ServiceBus;

namespace ICAP_AccountService.Events
{
    public class FriendRequestAccepted
    {
        private record FriendRequestAcceptedData (string UserId, string UserToAdd);
        private readonly MongoRepository _usersRepository = new();
        public FriendRequestAccepted(IBusHandler busHandler)
        {
            busHandler.CreateBusHandler<FriendRequestAcceptedData>("friendrequesttopic", "friendrequestsub", ProcessMessage);
        }

        private async Task ProcessMessage(FriendRequestAcceptedData data)
        {
            var user = await _usersRepository.GetAsync(data.UserId);
            if (user == null) return;
            user.FriendIds.Add(data.UserToAdd);
            await _usersRepository.UpdateAsync(user);
        }
    }
}
