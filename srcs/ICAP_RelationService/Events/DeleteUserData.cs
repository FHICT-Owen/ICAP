using ICAP_Infrastructure.Repositories;
using ICAP_RelationService.Entities;
using ICAP_ServiceBus;

namespace ICAP_RelationService.Events
{
    public class DeleteUserData
    {
        private readonly IRepository<Friends> _friendRepository;
        private readonly IRepository<FriendRequest> _friendRequestRepository;
        public DeleteUserData(IBusHandler busHandler, IRepository<Friends> friendRepository, IRepository<FriendRequest> friendRequestRepository)
        {
            _friendRepository = friendRepository;
            _friendRequestRepository = friendRequestRepository;
            busHandler.CreateBusHandler<string>("deleteuserdata", "DeleteUserDataFromRelationService", ProcessMessage);
        }

        private async Task ProcessMessage(string id)
        {
            await _friendRepository.RemoveAsync(id);
            var requestsToAndFromUser = await _friendRequestRepository.GetAllAsync(request => request.UserFrom == id && request.UserTo == id);
            foreach (var request in requestsToAndFromUser)
            {
                await _friendRequestRepository.RemoveAsync(request.Id);
            }
        }
    }
}
