using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitTifImages
{
    public static class NCZImage
    {
        public static void SplitImage(string pstrInputFilePath, string pstrOutputPath)
        {
            Image tiffImage = Image.FromFile(pstrInputFilePath);
            Guid objGuid = tiffImage.FrameDimensionsList[0];
            FrameDimension dimension = new FrameDimension(objGuid);
            int noOfPages = tiffImage.GetFrameCount(dimension);

            ImageCodecInfo encodeInfo = null;
            ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
            for (int j = 0; j < imageEncoders.Length; j++)
            {
                if (imageEncoders[j].MimeType == "image/tiff")
                {
                    encodeInfo = imageEncoders[j];
                    break;
                }
            }

            if (!Directory.Exists(pstrOutputPath))
                Directory.CreateDirectory(pstrOutputPath);

            foreach (Guid guid in tiffImage.FrameDimensionsList)
            {
                for (int index = 0; index < noOfPages; index++)
                {
                    FrameDimension currentFrame = new FrameDimension(guid);
                    tiffImage.SelectActiveFrame(currentFrame, index);
                    tiffImage.Save(string.Concat(pstrOutputPath, @"\", index, ".TIF"), encodeInfo, null);
                }
            }

            tiffImage.Dispose();
            tiffImage = null;
        }
    }
}
