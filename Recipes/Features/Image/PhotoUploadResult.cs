using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Features.Image
{
    public class PhotoUploadResult
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public long Size { get; set; }
    }
}
