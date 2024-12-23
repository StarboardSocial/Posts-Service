using FluentResults;
using StarboardSocial.UserService.Domain.DataInterfaces;


namespace StarboardSocial.UserService.Domain.Services;

public interface IDataDeletionService
{
    Task DeleteUserData(string userId);
}

public class DataDeletionService(IDataDeletionRepository dataDeletionRepository) : IDataDeletionService
{
    private readonly IDataDeletionRepository _dataDeletionRepository = dataDeletionRepository;
    
    public async Task DeleteUserData(string userId) => await _dataDeletionRepository.DeleteUserData(userId);
    
}