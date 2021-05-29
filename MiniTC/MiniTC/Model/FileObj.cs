using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTC.Model
{
    public class FileObj: AFile
    {
        public override string Name { get { return FileInfo.Name; } }
        public System.IO.FileInfo FileInfo;

        public FileObj(string filePath)
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
