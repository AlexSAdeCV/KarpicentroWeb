namespace KarpicentroWeb.Providers
{
    public enum Folders
    {
        Products = 0, Images = 1, Documents = 2, Temp = 3
    }

    public class PathProvider
    {
        private IWebHostEnvironment hostEnvironment;

        public PathProvider(IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        public string MapPath(string fileName, Folders folder)
        {
            string path = "";
            if (folder == Folders.Products)
            {
                string carpeta = "Images/Products";
                path = Path.Combine(this.hostEnvironment.WebRootPath, carpeta, fileName);
            }
            else if (folder == Folders.Images)
            {
                string carpeta = "Images/Users";
                path = Path.Combine(this.hostEnvironment.WebRootPath, carpeta, fileName);
            }

            return path;
        }
    }
}
