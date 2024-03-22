using Microsoft.AspNetCore.Mvc;

namespace MVC_Controllers_Functions.Controllers
{
    public class ImagensController : Controller
    {
        private string caminhoServidor;

        public ImagensController(IWebHostEnvironment sistema) 
        {
            caminhoServidor = sistema.WebRootPath;
        }
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile foto)
        {
            string caminhoParaSalvarImagem = caminhoServidor + "\\imagem\\";
            string novoNomeParaImagem = Guid.NewGuid().ToString() + foto.FileName;

            if (!Directory.Exists(caminhoParaSalvarImagem))
            {
                Directory.CreateDirectory(caminhoParaSalvarImagem);
            }

            using (var stream = System.IO.File.Create(caminhoParaSalvarImagem + novoNomeParaImagem))
            {
                foto.CopyToAsync(stream);
            }

            return RedirectToAction("Upload");
        }
    }
}
