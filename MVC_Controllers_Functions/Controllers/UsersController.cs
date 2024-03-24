using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Controllers_Functions.Data;
using MVC_Controllers_Functions.Models;

namespace MVC_Controllers_Functions.Controllers
{
    public class UsersController : Controller
    {
        private readonly MVC_Controllers_FunctionsContext _context;
        private readonly string _caminhoServidor;

        public UsersController(MVC_Controllers_FunctionsContext context, IWebHostEnvironment sistema)
        {
            _context = context;
            _caminhoServidor = Path.Combine(sistema.WebRootPath, "imagem");
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            if (user.UserImage.StartsWith("/Users/Details"))
            {
                user.UserImage = "/Users" + user.UserImage;
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Email,UserImage")] User user, IFormFile foto)
        {

            if (!(foto != null && foto.Length > 0))
            {
                return RedirectToAction("Users");
            }

            if (ModelState.IsValid)
            {
                string caminhoParaSalvarImagem = _caminhoServidor + "\\Userimagem\\";
                string novoNomeParaImagem = Guid.NewGuid().ToString() + foto.FileName;
                string CaminhoDaImagem = caminhoParaSalvarImagem + novoNomeParaImagem;

                if (!Directory.Exists(caminhoParaSalvarImagem))
                {
                    Directory.CreateDirectory(caminhoParaSalvarImagem);
                }

                string[] tiposDeImagemPermitidos = { ".jpg", ".jpeg", ".png", ".gif" };
                string extensao = Path.GetExtension(foto.FileName).ToLower();

                if (tiposDeImagemPermitidos.Contains(extensao))
                {
                    using (var stream = System.IO.File.Create(CaminhoDaImagem))
                    {
                        foto.CopyTo(stream);
                    }
                    await _context.SaveChangesAsync();
                }

                user.UserImage = Path.Combine("/imagem/Userimagem", novoNomeParaImagem); // Caminho absoluto relativo a wwwroot

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Email,UserImage")] User user)
        {
            if (id != user.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.ID == id);
        }
    }
}
