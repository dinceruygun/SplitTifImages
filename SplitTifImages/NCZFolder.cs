using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitTifImages
{
    public class NCZFolder
    {
        string _path;
        NCZFiles _files;

        public string FullPath
        {
            get
            {
                return _path;
            }
        }

        public NCZFolder(string path)
        {
            _path = path;
        }

        public NCZFiles LoadFiles(string fileName)
        {
            _files = new NCZFiles();
            _files.AddRange(Directory.GetFiles(FullPath, fileName, SearchOption.AllDirectories).Select(f => { return new NCZFile(System.IO.Path.GetFileName(f), f); }));

            return _files;
        }
    }
}
