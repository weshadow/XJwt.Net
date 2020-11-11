using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class ProjectLogAttachmentUploadModel
    {
        public int totalChunks { get; set; }
        public int current { get; set; }
        public string filename { get; set; }
        public int chunkSize { get; set; }
    }
}