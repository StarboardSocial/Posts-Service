using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FluentResults;
using Microsoft.Extensions.Configuration;
using StarboardSocial.PostsService.Domain.Models;
using StarboardSocial.UserService.Domain.DataInterfaces;

namespace StarboardSocial.PostsService.Domain.Services;

public interface IPrivateService
{
    Task<Result<List<Post>>> GetPosts(string userId);
    Task<Result<Post>> GetPost(string userId, string postId);
    Task<Result> DeletePost(string userId, string postId);
    Task<Result<Post>> CreatePost(Post post);
    Task<Result<Post>> UpdatePost(Post post);
}

public class PrivateService(IConfiguration config, IPrivateRepository privateRepository, BlobContainerClient blobContainerClient) : IPrivateService
{
    private readonly IPrivateRepository _privateRepository = privateRepository;
    private readonly BlobContainerClient _blobContainerClient = blobContainerClient;
    private readonly string _imageBaseUrl = config["AzureBlobStorage:ImageBaseUrl"]!;

    public async Task<Result<List<Post>>> GetPosts(string userId) => await _privateRepository.GetPosts(userId);

    public async Task<Result<Post>> GetPost(string userId, string postId) => await _privateRepository.GetPost(userId, postId);

    public async Task<Result> DeletePost(string userId, string postId) => await _privateRepository.DeletePost(userId, postId);

    public async Task<Result<Post>> CreatePost(Post post)
    {
        Result<string> imageResult = await UploadImageToBlob(post.Image);
        if (imageResult.IsFailed) return Result.Fail<Post>(imageResult.Errors);
        post.Image.Url = imageResult.Value;
        return await _privateRepository.CreatePost(post);  
    }

    public async Task<Result<Post>> UpdatePost(Post post) => await _privateRepository.UpdatePost(post);

    private async Task<Result<string>> UploadImageToBlob(PostImage image)
    {
        Response<BlobContentInfo>? response = await _blobContainerClient.UploadBlobAsync($"{image.Id.ToString()}.{image.ImageExtension}", image.Image);
        return response != null ? Result.Ok($"{_imageBaseUrl}{image.Id}.{image.ImageExtension}") : Result.Fail("Failed to upload image");
    }
}