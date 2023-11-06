using ICAP_Infrastructure.Entities;

namespace ICAP_AccountService.Entities
{
    public class User : IEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTimeOffset CreatedDateTime { get; set; }
    }
}
