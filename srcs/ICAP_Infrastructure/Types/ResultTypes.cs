namespace ICAP_Infrastructure.Types
{
    public record SuccessResult<T>(T Entity);
    public record FailureResult(string HelperText);
}
