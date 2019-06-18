using System.IO;

namespace GenericSiteCrawler.Services
{
    public class FileSaver
    {
        public void Save(string path, string content)
        {
            File.WriteAllText(path, content);
        }
    }
}
