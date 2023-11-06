using ICAP_Infrastructure.Entities;

namespace RelationService.Entities
{
    public class Friends : IEntity
    {
        public string Id { get; set; }
        public List<string> FriendIds { get; set; }
    }
}
