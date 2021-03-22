using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrapeCity.Documents.Imaging;
namespace CsvProcessor
{
    public class ProductTemplateGenerator
    {
        public void Generate()
        {
            var bmp = new GcBitmap(1024, 1024, true, 96, 96);
            var graphic = bmp.CreateGraphics();
        }
    }
}
