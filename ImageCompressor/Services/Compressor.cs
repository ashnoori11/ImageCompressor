namespace ImageCompressor.Services;

public sealed class Compressor
{
    private byte quality;
    #region methods
    public void ProcessImage(Action<Compressor> options)
    {
        ArgumentNullException.ThrowIfNull(options, nameof(options));

        Compressor myInstance = new Compressor();
        options(myInstance);

        if (string.IsNullOrEmpty(myInstance.Path))
            throw new ArgumentNullException(nameof(myInstance.Path));

        if (string.IsNullOrEmpty(myInstance.ImageName))
            throw new ArgumentNullException(nameof(myInstance.ImageName));

        this.Path = myInstance.Path;
        this.ImageName = myInstance.ImageName;
        this.Width = myInstance.Width;
        this.Height = myInstance.Height;
        this.Quality = myInstance.Quality;
        this.NewPath = string.IsNullOrWhiteSpace(myInstance.NewPath) ? myInstance.Path : myInstance.NewPath;
        this.ResizeImage();
    }
    private bool ResizeImage()
    {
        try
        {
            string newPath = string.IsNullOrWhiteSpace(this.NewPath) ? this.NewPath : this.NewPath;
            using var image = new MagickImage($"{this.Path}/{this.ImageName}");

            if (this.Width != 0 && this.Height != 0)
                image.Resize(this.Width, this.Height);

            image.Quality = this.Quality;

            string newImageFullPath = $"{newPath}/{Guid.NewGuid()}-Resized.jpg";
            image.Write(newImageFullPath);
            CompressImage(newImageFullPath);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    private void CompressImage(string path)
    {
        var image = new FileInfo(path);
        var optimizer = new ImageOptimizer();
        optimizer.Compress(image);
        image.Refresh();
    }
    #endregion

    #region properties
    public string Path { get; set; } = string.Empty;
    public string ImageName { get; set; } = string.Empty;
    public int Width { get; set; } = 0;
    public int Height { get; set; } = 0;
    public byte Quality
    {
        get => quality; set
        {
            if (value is 0 or > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(Quality));
            }
            quality = value;
        }
    }
    public string NewPath { get; set; } = string.Empty;
    #endregion
}
