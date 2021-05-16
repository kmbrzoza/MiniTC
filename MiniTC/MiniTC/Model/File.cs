using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTC.Model
{
    class File: AFile
    {
        public override string Name { get { return System.IO.Path.GetFileName(Path); } }
        public System.IO.FileInfo FileInfo;

        public File(string filePath)
        {
            this.Path = filePath;
            FileInfo = new System.IO.FileInfo(filePath);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
