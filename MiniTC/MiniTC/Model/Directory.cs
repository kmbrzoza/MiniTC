using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTC.Model
{
    class Directory
    {
        public string DirectoryPath { get; private set; }
        public string Name { get { return System.IO.Path.GetDirectoryName(DirectoryPath); } }
        private System.IO.DirectoryInfo fileInfo;

        public Directory(string directoryPath)
        {
            this.DirectoryPath = directoryPath;
            fileInfo = new System.IO.DirectoryInfo(directoryPath);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
