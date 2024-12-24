namespace StarboardSocial.PostsService.Domain.Models;

public class PostImage
{
    public required Guid Id { get; init; }
    public string? Url { get; set; }
    public Stream? Image { get; set; }
    public string? ImageExtension { get; set; }
}