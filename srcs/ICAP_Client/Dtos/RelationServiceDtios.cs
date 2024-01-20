namespace ICAP_Client.Dtos
{
    public record FriendRequestDto(string UserFrom, string UserTo, FriendRequestStatus RequestStatus);
    public record FriendsDto(List<string> FriendIds);
    public enum FriendRequestStatus
    {
        Pending,
        Accepted,
        Declined
    }
}
