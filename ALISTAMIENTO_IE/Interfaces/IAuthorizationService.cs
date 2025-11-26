namespace ALISTAMIENTO_IE.Interfaces
{
    public interface IAuthorizationService
    {
        Task<int> CreateAuthorizationAsync(int areaId, int roleId, string plainPassword);

        Task<bool> UpdatePasswordAsync(int areaId, int roleId, string newPlainPassword);

        Task<bool> ValidatePasswordAsync(int areaId, int roleId, string inputPassword);
    }

}
