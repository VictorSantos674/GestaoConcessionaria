using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DesafioIntelectah.Data;
using DesafioIntelectah.Models;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioIntelectah.Controllers
{
    [Authorize(Roles = "Administrador,Gerente,Vendedor")]
    public class VeiculosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VeiculosController(ApplicationDbContext context)
        {
            _context = context;
        }
    
        // GET: Veiculos/PorFabricante/5 (AJAX)
        [HttpGet]
        [AllowAnonymous]
    public async Task<IActionResult> PorFabricante(int fabricanteID)
        {
            var veiculos = await _context.Veiculos
                .Where(v => v.FabricanteID == fabricanteID && !v.IsDeleted)
                .Select(v => new { v.VeiculoID, v.Modelo })
                .OrderBy(v => v.Modelo)
                .ToListAsync();
            return Json(veiculos);
        }

        // GET: Veiculos
        public async Task<IActionResult> Index()
        {
            var veiculos = await _context.Veiculos.Include(v => v.Fabricante).AsNoTracking().ToListAsync();
            return View(veiculos);
        }

        // GET: Veiculos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
        var veiculo = await _context.Veiculos.Include(v => v.Fabricante).FirstOrDefaultAsync(v => v.VeiculoID == id);
            if (veiculo == null) return NotFound();
            return View(veiculo);
        }

        // GET: Veiculos/Create
        [Authorize(Roles = "Administrador,Gerente")]
        public IActionResult Create()
        {
            ViewBag.Fabricantes = _context.Fabricantes.AsNoTracking().ToList();
            return View();
        }

        // POST: Veiculos/Create
        [Authorize(Roles = "Administrador,Gerente")]
        [HttpPost]
        [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Modelo,AnoFabricacao,Preco,TipoVeiculo,Descricao,FabricanteID")] Veiculo veiculo)
        {
            if (!_context.Fabricantes.Any(f => f.FabricanteID == veiculo.FabricanteID))
                ModelState.AddModelError("FabricanteID", "Selecione um fabricante válido.");
            if (ModelState.IsValid)
            {
                _context.Add(veiculo);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Veículo cadastrado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Fabricantes = _context.Fabricantes.AsNoTracking().ToList();
            return View(veiculo);
        }

        // GET: Veiculos/Edit/5
        [Authorize(Roles = "Administrador,Gerente")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null) return NotFound();
            ViewBag.Fabricantes = _context.Fabricantes.AsNoTracking().ToList();
            return View(veiculo);
        }

        // POST: Veiculos/Edit/5
        [Authorize(Roles = "Administrador,Gerente")]
        [HttpPost]
        [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("VeiculoID,Modelo,AnoFabricacao,Preco,TipoVeiculo,Descricao,FabricanteID")] Veiculo veiculo)
        {
            if (id != veiculo.VeiculoID) return NotFound();
            if (!_context.Fabricantes.Any(f => f.FabricanteID == veiculo.FabricanteID))
                ModelState.AddModelError("FabricanteID", "Selecione um fabricante válido.");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(veiculo);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Veículo atualizado com sucesso!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VeiculoExists(veiculo.VeiculoID))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Fabricantes = _context.Fabricantes.AsNoTracking().ToList();
            return View(veiculo);
        }

        // GET: Veiculos/Delete/5
        [Authorize(Roles = "Administrador,Gerente")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
        var veiculo = await _context.Veiculos.Include(v => v.Fabricante).FirstOrDefaultAsync(v => v.VeiculoID == id);
            if (veiculo == null) return NotFound();
            return View(veiculo);
        }

        // POST: Veiculos/Delete/5
        [Authorize(Roles = "Administrador,Gerente")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null) return NotFound();
            veiculo.IsDeleted = true;
            _context.Update(veiculo);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Veículo removido com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private bool VeiculoExists(int id)
        {
            return _context.Veiculos.Any(e => e.VeiculoID == id);
        }
    }
}
