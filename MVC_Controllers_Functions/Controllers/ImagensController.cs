using Microsoft.AspNetCore.Mvc;

namespace MVC_Controllers_Functions.Controllers
{
    public class ImagensController : Controller
    {
        private readonly string _caminhoServidor;

        public ImagensController(IWebHostEnvironment sistema)
        {
            _caminhoServidor = Path.Combine(sistema.WebRootPath, "imagem");
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile foto)
        {
            if (!(foto != null && foto.Length > 0))
            {
                return RedirectToAction("Upload");
            }

            string caminhoParaSalvarImagem = _caminhoServidor + "\\imagem\\";
            string novoNomeParaImagem = Guid.NewGuid().ToString() + foto.FileName;

            if (!Directory.Exists(caminhoParaSalvarImagem))
            {
                Directory.CreateDirectory(caminhoParaSalvarImagem);
            }

            string[] tiposDeImagemPermitidos = { ".jpg", ".jpeg", ".png", ".gif" };
            string extensao = Path.GetExtension(foto.FileName).ToLower();

            if (tiposDeImagemPermitidos.Contains(extensao))
            {
                using (var stream = System.IO.File.Create(caminhoParaSalvarImagem + novoNomeParaImagem))
                {
                    foto.CopyToAsync(stream);
                }
            }

            return RedirectToAction("Upload");
        }
    }
    
}
