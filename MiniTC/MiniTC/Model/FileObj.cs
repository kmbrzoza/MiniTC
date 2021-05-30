using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MiniTC.Model
{
    public class FileObj: AFile
    {
        public override string Name { get { return FileInfo.Name; } }
        public System.IO.FileInfo FileInfo;

        private FileObj(string filePath)
        {
            this.Path = filePath;
            FileInfo = new System.IO.FileInfo(filePath);
        }

        public static FileObj GetFileObj(string filePath)
        {
            if (File.Exists(filePath))
                return new FileObj(filePath);
            return null;
        }

        public override string ToString()
        {
            return Name;
        }

        public static bool Exists(string path)
        {
            return File.Exists(path);
        }
    }
}
