using ICAP_Infrastructure.Entities;

namespace ICAP_RelationService.Entities
{
    public class FriendRequest : IEntity
    {
        public string Id { get; set; }
        public string UserFrom { get; set; }
        public string UserTo { get; set; }
        public FriendRequestStatus RequestStatus { get; set; }
        public DateTimeOffset CreatedDateTime { get; set; }
    }

    public enum FriendRequestStatus
    {
        Pending, 
        Accepted,
        Declined
    }
}
