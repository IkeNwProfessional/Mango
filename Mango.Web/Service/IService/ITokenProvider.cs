namespace Mango.Web.Service.IService
{
    public interface ITokenProvider
    {
        void SetTokenInCookies(string token);
        string? GetTokenFromCookies();
        void ClearTokenFromCookies();
    }
}
