using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CsvProcessor.Controllers
{
    public class ProductCatalogController : Controller
    {
        [HttpPost(nameof(ProductCatalog))]
        public async Task<IActionResult> ProductCatalog(IFormFile file)
        {
            var filestream = file.OpenReadStream();
            var processor = new Processor();
         
            return Ok(await processor.Process(filestream));
            }
        }
    }
