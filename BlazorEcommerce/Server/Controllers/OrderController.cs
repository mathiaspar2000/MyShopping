using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using System.Globalization;
using System.Text;

namespace BlazorEcommerce.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        public class Order
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public decimal Price { get; set; }
        }

        [HttpPost("generate-csv")]
        public IActionResult GenerateCsv([FromBody] List<Order> orders)
        {
            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream))
            using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                csvWriter.WriteRecords(orders);
                streamWriter.Flush();
                var result = Encoding.UTF8.GetString(memoryStream.ToArray());
                return File(Encoding.UTF8.GetBytes(result), "text/csv", "order.csv");
            }
        }
    }
}
