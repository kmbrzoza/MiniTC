using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTC.Model
{
    public class DirectoryObj : AFile
    {
        public override string Name { get { return DirectoryInfo.Name; } }
        public  System.IO.DirectoryInfo DirectoryInfo;

        public DirectoryObj(string directoryPath)
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
