using ICAP_Infrastructure.Repositories;
using ICAP_RelationService.Entities;
using MassTransit;


namespace ICAP_RelationService.Events
{
    public class DeleteUserData : IConsumer<string>
    {
        private readonly IRepository<Friends> _friendRepository;
        private readonly IRepository<FriendRequest> _friendRequestRepository;
        public DeleteUserData(IRepository<Friends> friendRepository, IRepository<FriendRequest> friendRequestRepository)
        {
            _friendRepository = friendRepository;
            _friendRequestRepository = friendRequestRepository;
        }

        public async Task Consume(ConsumeContext<string> ctx)
        {
            Console.WriteLine($"Removing data for user {ctx.Message}");
            await _friendRepository.RemoveAsync(ctx.Message);
            var requestsToAndFromUser = await _friendRequestRepository.GetAllAsync(request => request.UserFrom == ctx.Message && request.UserTo == ctx.Message);
            foreach (var request in requestsToAndFromUser)
            {
                await _friendRequestRepository.RemoveAsync(request.Id);
            }
        }
    }
}
