namespace YourStudioFinal.Models.Gallery;

public class Gallery
{
    // TODO:
    //public User associatedUser { get; set; }
    public string Id { get; set; }

    public List<GalleryFile> GalleryFiles { get; set; } = new();
}