using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTC.Model
{
    class File
    {
        public string FilePath { get; private set; }
        public string Name { get { return System.IO.Path.GetFileName(FilePath); } }
        private System.IO.FileInfo fileInfo;

        public File(string filePath)
        {
            this.FilePath = filePath;
            fileInfo = new System.IO.FileInfo(filePath);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
