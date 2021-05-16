using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTC.Model
{
    abstract class AFile
    {
        public string Path;
        public virtual string Name { get; set; }
        public abstract override string ToString();

    }
}
