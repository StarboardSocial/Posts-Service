namespace StarboardSocial.UserService.Domain.DataInterfaces;

public interface IDataDeletionRepository
{
    Task DeleteUserData(string userId);
}