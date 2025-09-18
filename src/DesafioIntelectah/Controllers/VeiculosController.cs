using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.DesafioIntelectah.Data;
using src.DesafioIntelectah.Models;
using System.Linq;
using System.Threading.Tasks;

namespace src.DesafioIntelectah.Controllers
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
        public async Task<IActionResult> PorFabricante(int fabricanteId)
        {
            var veiculos = await _context.Veiculos
                .Where(v => v.FabricanteId == fabricanteId && !v.IsDeleted)
                .Select(v => new { v.VeiculoId, v.Modelo })
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
            var veiculo = await _context.Veiculos.Include(v => v.Fabricante).FirstOrDefaultAsync(v => v.VeiculoId == id);
            if (veiculo == null) return NotFound();
            return View(veiculo);
        }

        // GET: Veiculos/Create
        [Authorize(Roles = "Administrador,Gerente")]
        public IActionResult Create()
        {
            ViewBag.Fabricantes = _context.Fabricantes.ToList();
            return View();
        }

        // POST: Veiculos/Create
        [Authorize(Roles = "Administrador,Gerente")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Modelo,AnoFabricacao,Preco,TipoVeiculo,Descricao,FabricanteId")] Veiculo veiculo)
        {
            if (!_context.Fabricantes.Any(f => f.FabricanteId == veiculo.FabricanteId))
                ModelState.AddModelError("FabricanteId", "Selecione um fabricante válido.");
            if (ModelState.IsValid)
            {
                _context.Add(veiculo);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Veículo cadastrado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Fabricantes = _context.Fabricantes.ToList();
            return View(veiculo);
        }

        // GET: Veiculos/Edit/5
        [Authorize(Roles = "Administrador,Gerente")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null) return NotFound();
            ViewBag.Fabricantes = _context.Fabricantes.ToList();
            return View(veiculo);
        }

        // POST: Veiculos/Edit/5
        [Authorize(Roles = "Administrador,Gerente")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VeiculoId,Modelo,AnoFabricacao,Preco,TipoVeiculo,Descricao,FabricanteId")] Veiculo veiculo)
        {
            if (id != veiculo.VeiculoId) return NotFound();
            if (!_context.Fabricantes.Any(f => f.FabricanteId == veiculo.FabricanteId))
                ModelState.AddModelError("FabricanteId", "Selecione um fabricante válido.");
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
                    if (!VeiculoExists(veiculo.VeiculoId))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Fabricantes = _context.Fabricantes.ToList();
            return View(veiculo);
        }

        // GET: Veiculos/Delete/5
        [Authorize(Roles = "Administrador,Gerente")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var veiculo = await _context.Veiculos.Include(v => v.Fabricante).FirstOrDefaultAsync(v => v.VeiculoId == id);
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
            return _context.Veiculos.Any(e => e.VeiculoId == id);
        }
    }
}
