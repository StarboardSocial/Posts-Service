using MongoDB.Driver;
using RabbitMQ.Client;
using StarboardSocial.PostsService.Data.DTOs;
using StarboardSocial.UserService.Domain.DataInterfaces;

namespace StarboardSocial.UserService.Data.Repositories;

public class DataDeletionRepository(IMongoDatabase mongoDatabase) : IDataDeletionRepository
{
    private readonly IMongoCollection<PostEntity> _postsCollection = mongoDatabase.GetCollection<PostEntity>("posts");

    public async Task DeleteUserData(string userId)
    {
        DeleteResult result = await _postsCollection.DeleteManyAsync(post => post.UserId == userId);
        if (!result.IsAcknowledged)
        {
            throw new Exception($"Failed to delete user data from user {userId}: {result}");
        }
    }
}