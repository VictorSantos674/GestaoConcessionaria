using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.DesafioIntelectah.Data;
using src.DesafioIntelectah.Models;
using System.Linq;
using System.Threading.Tasks;

namespace src.DesafioIntelectah.Controllers
{
    public class FabricantesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FabricantesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Fabricantes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Fabricantes.AsNoTracking().ToListAsync());
        }

        // GET: Fabricantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var fabricante = await _context.Fabricantes.FirstOrDefaultAsync(f => f.FabricanteId == id);
            if (fabricante == null) return NotFound();
            return View(fabricante);
        }

        // GET: Fabricantes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fabricantes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,PaisOrigem,AnoFundacao,Website")] Fabricante fabricante)
        {
            if (ModelState.IsValid)
            {
                // Validação de unicidade
                if (await _context.Fabricantes.AnyAsync(f => f.Nome == fabricante.Nome))
                {
                    ModelState.AddModelError("Nome", "Já existe um fabricante com este nome.");
                    return View(fabricante);
                }
                _context.Add(fabricante);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Fabricante cadastrado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(fabricante);
        }

        // GET: Fabricantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var fabricante = await _context.Fabricantes.FindAsync(id);
            if (fabricante == null) return NotFound();
            return View(fabricante);
        }

        // POST: Fabricantes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FabricanteId,Nome,PaisOrigem,AnoFundacao,Website")] Fabricante fabricante)
        {
            if (id != fabricante.FabricanteId) return NotFound();
            if (ModelState.IsValid)
            {
                // Validação de unicidade (exceto o próprio)
                if (await _context.Fabricantes.AnyAsync(f => f.Nome == fabricante.Nome && f.FabricanteId != fabricante.FabricanteId))
                {
                    ModelState.AddModelError("Nome", "Já existe um fabricante com este nome.");
                    return View(fabricante);
                }
                try
                {
                    _context.Update(fabricante);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Fabricante atualizado com sucesso!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FabricanteExists(fabricante.FabricanteId))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(fabricante);
        }

        // GET: Fabricantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var fabricante = await _context.Fabricantes.FirstOrDefaultAsync(f => f.FabricanteId == id);
            if (fabricante == null) return NotFound();
            return View(fabricante);
        }

        // POST: Fabricantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fabricante = await _context.Fabricantes.FindAsync(id);
            if (fabricante == null) return NotFound();
            fabricante.IsDeleted = true;
            _context.Update(fabricante);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Fabricante removido com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private bool FabricanteExists(int id)
        {
            return _context.Fabricantes.Any(e => e.FabricanteId == id);
        }
    }
}
