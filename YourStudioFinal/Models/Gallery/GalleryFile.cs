namespace YourStudioFinal.Models.Gallery;

public class GalleryFile
{
    public string Id { get; set; }
    public string fileName { get; set; }
    public string category { get; set; }


    public string GalleryId { get; set; } = null!;
    public Gallery Gallery { get; set; } = null!;
}