using StarboardSocial.PostsService.Data.DTOs;
using StarboardSocial.PostsService.Domain.Models;

namespace StarboardSocial.PostsService.Data.Mappers;

public static class PostMapper
{
    public static Post ToPost(this PostEntity postEntity)
    {
        Guid? imageId = null;
        try
        {
            imageId = Guid.Parse(postEntity.ImageUrl.Split("/").Last().Split(".").First());    
        } catch (Exception e)
        {
            imageId = Guid.NewGuid();
        }
        
        return new Post
        {
            Id = Guid.Parse(postEntity.Id),
            UserId = postEntity.UserId,
            Title = postEntity.Title,
            Description = postEntity.Description,
            Image = new()
            {
                Id = imageId!.Value,
                Url =  postEntity.ImageUrl
            },
            PostedAt = postEntity.PostedAt
        };
    }
    
    public static PostEntity ToPostEntity(this Post post)
    {
        return new PostEntity
        {
            Id = post.Id.ToString(),
            UserId = post.UserId,
            Title = post.Title,
            Description = post.Description,
            ImageUrl = post.Image.Url!,
            PostedAt = post.PostedAt
        };
    }
}