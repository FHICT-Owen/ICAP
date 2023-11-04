namespace ICAP_AccountService.Entities
{
    public class User : IStringEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string> FriendIds { get; set; }
        public DateTimeOffset CreatedDateTime { get; set; }
    }
}
