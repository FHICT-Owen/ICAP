namespace ICAP_AccountService
{
    public record AccountDto();

    public record CreateAccountDto(string Name, string Description, bool IsActiveListing, decimal Price);

    public record UpdateAccountDto(string Name, string Description, bool IsActiveListing, decimal Price);
}
