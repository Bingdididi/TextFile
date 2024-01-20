using Microsoft.AspNetCore.Mvc;

namespace TextFilesMvc;


public class FileController : Controller
{
      private readonly ILogger<FileController> _logger;
      private readonly IWebHostEnvironment _env;

    public FileController(ILogger<FileController> logger, IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

  public IActionResult Index()
{
    var path = Path.Combine(_env.ContentRootPath, "TextFiles");
    var files = Directory.GetFiles(path).Select(f => Path.GetFileNameWithoutExtension(f)).ToArray();
    ViewBag.Files = files;

    return View();
}

[HttpGet("Content/{fileName}")]
   public IActionResult Content(string fileName)
    {
        try
        {
            var filePath = Path.Combine(_env.ContentRootPath, "TextFiles", fileName + ".txt");
            if (!System.IO.File.Exists(filePath))
            {
                _logger.LogInformation($"Attempting to access file: {fileName}.txt");

                return NotFound($"File {fileName}.txt not found.");
            }

            var content = System.IO.File.ReadAllText(filePath);
            return View(model: content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading file");
            return StatusCode(500, "An error occurred while reading the file.");
        }
    }


}
