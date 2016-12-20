using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitTifImages
{
    public class NCZFile
    {
        string _fileName;
        string _filePath;

        public string FileName
        {
            get
            {
                return _fileName;
            }
        }

        public string FilePath
        {
            get
            {
                return _filePath;
            }
        }

        public NCZFile(string fileName, string filePath)
        {
            _fileName = fileName;
            _filePath = filePath;
        }
    }
}
