using ICAP_Infrastructure.Repositories;
using ICAP_RelationService.Entities;
using ICAP_ServiceBus;

namespace ICAP_RelationService.Events
{
    public class DeleteUserData
    {
        private readonly IRepository<Friends> _friendRepository;
        public DeleteUserData(IBusHandler busHandler, IRepository<Friends> friendRepository)
        {
            _friendRepository = friendRepository;
            busHandler.CreateBusHandler<string>("deleteuserdata", "DeleteUserDataFromRelationService", ProcessMessage);
        }

        private async Task ProcessMessage(string id)
        {
            await _friendRepository.RemoveAsync(id);
        }
    }
}
