using ICAP_Infrastructure.Entities;

namespace ICAP_RelationService.Entities
{
    public class Friends : IEntity
    {
        public string Id { get; set; }
        public List<string> FriendIds { get; set; }
    }
}
