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

        public IActionResult Blog()
        {
            List<Comentario> comentarios=this.repo.GetComentarios();
            return View(comentarios);

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
