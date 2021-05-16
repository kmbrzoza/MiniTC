using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTC.Model
{
    class Directory : AFile
    {
        public override string Name { get { return System.IO.Path.GetDirectoryName(Path); } }
        public  System.IO.DirectoryInfo DirectoryInfo;

        public Directory(string directoryPath)
        {
            this.Path = directoryPath;
            DirectoryInfo = new System.IO.DirectoryInfo(directoryPath);
        }

        public override string ToString()
        {
            return "<D>" + Name;
        }
    }
}
