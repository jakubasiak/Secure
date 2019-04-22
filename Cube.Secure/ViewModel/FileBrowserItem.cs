using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube.Secure.ViewModel
{
    public class FileBrowserItem
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public bool IsDirectory { get; set; }
        public long Size { get; set; }
    }
}
