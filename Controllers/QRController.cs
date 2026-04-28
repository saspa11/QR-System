using Microsoft.AspNetCore.Mvc;
using StudentProfileQRSystem.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace StudentProfileQRSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QRController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public QRController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateQR([FromBody] StudentInfo info)
        {
            if (string.IsNullOrEmpty(info.StudentID))
                return BadRequest("Student ID is required");

            string qrData =
                $"Student ID: {info.StudentID}\n" +
                $"Name: {info.Name}\n" +
                $"Contact: {info.Contact}\n" +
                $"Email: {info.Email}\n" +
                $"Address: {info.Address}\n" +
                $"Course: {info.Course}\n" +
                $"Year: {info.Year}";

            string qrApiUrl = $"https://api.qrserver.com/v1/create-qr-code/?size=250x250&data={Uri.EscapeDataString(qrData)}";

            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(qrApiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return StatusCode(500, "QR API Error: " + error);
                }

                var bytes = await response.Content.ReadAsByteArrayAsync();
                return File(bytes, "image/png");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: " + ex.Message);
            }
        }
    }
}