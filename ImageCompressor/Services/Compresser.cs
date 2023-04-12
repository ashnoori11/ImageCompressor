namespace ImageCompressor.Services;

public sealed class Compresser : IDisposable
{
    #region methods
    public void ProccessImage(Action<Compresser> options)
    {
        ArgumentNullException.ThrowIfNull(options, $"{nameof(options)} can not be null or empty");

        Compresser myInstance = new Compresser();
        options(myInstance);

        if (string.IsNullOrEmpty(myInstance.Path))
            throw new ArgumentNullException("path can not be null or empty");

        if (string.IsNullOrEmpty(myInstance.ImageName))
            throw new ArgumentNullException("imageName can not be null or empty");

        if (myInstance.Quality == 0 || myInstance.Quality > 100)
            throw new ArgumentOutOfRangeException("invalid value for quality - quality must be between 1 and 100");

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
        if (this.Quality == 0 || this.Quality > 100)
            throw new ArgumentOutOfRangeException("invalid quality");

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
    public int Quality { get; set; }
    public string NewPath { get; set; } = string.Empty;
    #endregion

    #region dispose
    private bool disposedValue;
    private void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
            }

            this.disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion
}
