using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MiniTC.Model
{
    public class DirectoryObj : AFile
    {
        public override string Name { get { return DirectoryInfo.Name; } }
        public DirectoryInfo DirectoryInfo;

        private DirectoryObj(string directoryPath)
        {
            this.Path = directoryPath;
            DirectoryInfo = new DirectoryInfo(directoryPath);
        }

        public static DirectoryObj GetDirectoryObj(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
                return new DirectoryObj(directoryPath);
            return null;
        }

        public override string ToString()
        {
            return "<D>" + Name;
        }

        public static bool Exists(string path)
        {
            return Directory.Exists(path);
        }
    }
}
