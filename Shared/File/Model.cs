
using Models;

namespace File
{
  public  class ListModel
    {
        public long Id { get; set; }
        public long Size { get; set; }
        public string Url { get; set; }

        public System.DateTime CreateDate { get; set; }
        public System.DateTime? LastAccessDate { get; set; }
    }
    public class AddModel
    {
        public FileDto File { get; set; }
        public string DisplayUrl { get; set; }

        public string ToDeleteFileUrl { get; set; }
    }
}
