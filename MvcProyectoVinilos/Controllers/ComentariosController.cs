using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcProyectoVinilos.Models;
using MvcProyectoVinilos.Repositories;

namespace MvcProyectoVinilos.Controllers
{
    public class ComentariosController : Controller
    {
        private ComentarioRepository repo;
        public ComentariosController(ComentarioRepository repo)
        {
            this.repo = repo;
        }

        public IActionResult Blog(int page = 1, int pageSize = 5)
        {
            List<Comentario> comentarios = this.repo.GetComentarios();
            var paginatedComentarios = comentarios.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.PageNumber = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = Math.Ceiling((double)comentarios.Count / pageSize);

            return View(paginatedComentarios);
        }
        [Authorize]
        [HttpPost]
        public IActionResult Blog(string comentario, string nombre, int usuario)
        {
            this.repo.InsertComentario(comentario, nombre, usuario);
            return RedirectToAction("Blog");

        }
       public IActionResult List()
        {
            List<Comentario> comentarios = this.repo.GetComentarios();

            return View(comentarios);
        }
        public IActionResult Delete(int id) {
        this.repo.EliminarComentario(id);
            return RedirectToAction("List");
        }
    }
}
