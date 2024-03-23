using Microsoft.AspNetCore.Mvc;

namespace MVC_Controllers_Functions.Controllers
{
    public class ArquivosController : Controller
    {
        private string caminhoServidor;

        public ArquivosController(IWebHostEnvironment sistema)
        {
            caminhoServidor = sistema.WebRootPath;
        }
        public IActionResult UploadArquivos()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile arquivo)
        {
            if (!(arquivo != null && arquivo.Length > 0))
            {
                return RedirectToAction("UploadArquivos");
            }

            string caminhoParaSalvarImagem = caminhoServidor + "\\arquivos\\";
            string novoNomeParaImagem = Guid.NewGuid().ToString() + arquivo.FileName;

            if (!Directory.Exists(caminhoParaSalvarImagem))
            {
                Directory.CreateDirectory(caminhoParaSalvarImagem);
            }

            using (var stream = System.IO.File.Create(caminhoParaSalvarImagem + novoNomeParaImagem))
            {
                arquivo.CopyToAsync(stream);
            }
            

            return RedirectToAction("UploadArquivos");
        }
    }
}
