using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DesafioIntelectah.Data;
using DesafioIntelectah.Models;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioIntelectah.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ConcessionariasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConcessionariasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Concessionarias
        public async Task<IActionResult> Index()
        {
            return View(await _context.Concessionarias.AsNoTracking().ToListAsync());
        }

        // GET: Concessionarias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var concessionaria = await _context.Concessionarias.FirstOrDefaultAsync(c => c.ConcessionariaID == id);
            if (concessionaria == null) return NotFound();
            return View(concessionaria);
        }

        // GET: Concessionarias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Concessionarias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nome,Endereco,Cidade,Estado,CEP,Telefone,Email,CapacidadeMaximaVeiculos")] Concessionaria concessionaria)
        {
            if (ModelState.IsValid)
            {
                // Validação de unicidade
                if (await _context.Concessionarias.AnyAsync(c => c.Nome == concessionaria.Nome))
                {
                    ModelState.AddModelError("Nome", "Já existe uma concessionária com este nome.");
                    return View(concessionaria);
                }
                _context.Add(concessionaria);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Concessionária cadastrada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(concessionaria);
        }

        // GET: Concessionarias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var concessionaria = await _context.Concessionarias.FindAsync(id);
            if (concessionaria == null) return NotFound();
            return View(concessionaria);
        }

        // POST: Concessionarias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ConcessionariaID,Nome,Endereco,Cidade,Estado,CEP,Telefone,Email,CapacidadeMaximaVeiculos")] Concessionaria concessionaria)
        {
            if (id != concessionaria.ConcessionariaID) return NotFound();
            if (ModelState.IsValid)
            {
                // Validação de unicidade (exceto o próprio)
                if (await _context.Concessionarias.AnyAsync(c => c.Nome == concessionaria.Nome && c.ConcessionariaID != concessionaria.ConcessionariaID))
                {
                    ModelState.AddModelError("Nome", "Já existe uma concessionária com este nome.");
                    return View(concessionaria);
                }
                try
                {
                    _context.Update(concessionaria);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Concessionária atualizada com sucesso!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConcessionariaExists(concessionaria.ConcessionariaID))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(concessionaria);
        }

        // GET: Concessionarias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var concessionaria = await _context.Concessionarias.FirstOrDefaultAsync(c => c.ConcessionariaID == id);
            if (concessionaria == null) return NotFound();
            return View(concessionaria);
        }

        // POST: Concessionarias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var concessionaria = await _context.Concessionarias.FindAsync(id);
            if (concessionaria == null) return NotFound();
            concessionaria.IsDeleted = true;
            _context.Update(concessionaria);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Concessionária removida com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private bool ConcessionariaExists(int id)
        {
            return _context.Concessionarias.Any(e => e.ConcessionariaID == id);
        }
    }
}
