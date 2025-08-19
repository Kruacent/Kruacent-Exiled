using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Utils
{
    public static class ImageUtils
    {
        private static HashSet<Image> images = null;

        public static string Path => Paths.Configs + "/";


        public static IEnumerable<Image> GetAllImages()
        {
            if(images is not null)
            {
                return images;
            }


        }

    }
}
