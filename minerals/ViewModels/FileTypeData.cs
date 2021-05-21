using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.ViewModels
{
    public class FileTypeData
    {
        private List<FileTypeIcon> FileTypeIcons = new List<FileTypeIcon>{
            new FileTypeIcon{ FileExtension = ".pdf", Icon = "file-pdf-solid-gray.png"},
            new FileTypeIcon{ FileExtension = ".xlsx", Icon = "file-excel-solid-gray.png"},
            new FileTypeIcon{ FileExtension = ".xls", Icon = "file-excel-solid-gray.png"},
            new FileTypeIcon{ FileExtension = ".xlt", Icon = "file-excel-solid-gray.png"},
            new FileTypeIcon{ FileExtension = ".csv", Icon = "file-csv-solid-gray.png"},
            new FileTypeIcon{ FileExtension = ".png", Icon = "file-image-solid-gray.png"},
            new FileTypeIcon{ FileExtension = ".jpg", Icon = "file-image-solid-gray.png"},
            new FileTypeIcon{ FileExtension = ".jpeg", Icon = "file-image-solid-gray.png"},
            new FileTypeIcon{ FileExtension = ".bmp", Icon = "file-image-solid-gray.png"},
            new FileTypeIcon{ FileExtension = ".doc", Icon = "file-word-solid-gray.png"},
            new FileTypeIcon{ FileExtension = ".docx", Icon = "file-word-solid-gray.png"},
            new FileTypeIcon{ FileExtension = ".docm", Icon = "file-word-solid-gray.png"},
            new FileTypeIcon{ FileExtension = ".txt", Icon = "file-alt-solid-gray.png"},
        };
        public List<FileTypeIcon> fileTypeIcons => FileTypeIcons;
    }
 
}
