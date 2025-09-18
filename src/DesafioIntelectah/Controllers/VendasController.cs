using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.DesafioIntelectah.Data;
using src.DesafioIntelectah.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace src.DesafioIntelectah.Controllers
{
    [Authorize(Roles = "Administrador,Gerente,Vendedor")]
    public class VendasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VendasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vendas
        public async Task<IActionResult> Index()
        {
            var vendas = await _context.Vendas
                .Include(v => v.Veiculo)
                .Include(v => v.Concessionaria)
                .Include(v => v.Cliente)
                .AsNoTracking().ToListAsync();
            return View(vendas);
        }

        // GET: Vendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var venda = await _context.Vendas
                .Include(v => v.Veiculo)
                .Include(v => v.Concessionaria)
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(v => v.VendaId == id);
            if (venda == null) return NotFound();
            return View(venda);
        }

        // GET: Vendas/Create
        public IActionResult Create()
        {
            ViewBag.Veiculos = _context.Veiculos.ToList();
            ViewBag.Concessionarias = _context.Concessionarias.ToList();
            ViewBag.Clientes = _context.Clientes.ToList();
            return View();
        }

        // POST: Vendas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DataVenda,PrecoVenda,VeiculoId,ConcessionariaId,ClienteId")] Venda venda)
        {
            // Geração de protocolo único
            venda.ProtocoloVenda = Guid.NewGuid().ToString().Substring(0, 20);

            // Validação: preço de venda não pode ser maior que o preço do veículo
            var veiculo = await _context.Veiculos.FindAsync(venda.VeiculoId);
            if (veiculo == null)
                ModelState.AddModelError("VeiculoId", "Selecione um veículo válido.");
            else if (venda.PrecoVenda > veiculo.Preco)
                ModelState.AddModelError("PrecoVenda", "O preço de venda não pode ser maior que o preço do veículo.");

            // Validação: veículo já vendido
            if (await _context.Vendas.AnyAsync(v => v.VeiculoId == venda.VeiculoId && !v.IsDeleted))
                ModelState.AddModelError("VeiculoId", "Este veículo já foi vendido e não pode ser vendido novamente.");

            // Validação de unicidade do protocolo
            if (await _context.Vendas.AnyAsync(v => v.ProtocoloVenda == venda.ProtocoloVenda))
                ModelState.AddModelError("ProtocoloVenda", "Já existe uma venda com este protocolo. Tente novamente.");

            if (ModelState.IsValid)
            {
                _context.Add(venda);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Venda registrada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Veiculos = _context.Veiculos.ToList();
            ViewBag.Concessionarias = _context.Concessionarias.ToList();
            ViewBag.Clientes = _context.Clientes.ToList();
            return View(venda);
        }

        // GET: Vendas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var venda = await _context.Vendas.FindAsync(id);
            if (venda == null) return NotFound();
            ViewBag.Veiculos = _context.Veiculos.ToList();
            ViewBag.Concessionarias = _context.Concessionarias.ToList();
            ViewBag.Clientes = _context.Clientes.ToList();
            return View(venda);
        }

        // POST: Vendas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VendaId,DataVenda,PrecoVenda,ProtocoloVenda,VeiculoId,ConcessionariaId,ClienteId")] Venda venda)
        {
            if (id != venda.VendaId) return NotFound();
            var veiculo = await _context.Veiculos.FindAsync(venda.VeiculoId);
            if (veiculo == null)
                ModelState.AddModelError("VeiculoId", "Selecione um veículo válido.");
            else if (venda.PrecoVenda > veiculo.Preco)
                ModelState.AddModelError("PrecoVenda", "O preço de venda não pode ser maior que o preço do veículo.");

            // Validação: veículo já vendido (exceto a própria venda)
            if (await _context.Vendas.AnyAsync(v => v.VeiculoId == venda.VeiculoId && v.VendaId != venda.VendaId && !v.IsDeleted))
                ModelState.AddModelError("VeiculoId", "Este veículo já foi vendido e não pode ser vendido novamente.");

            // Validação de unicidade do protocolo (exceto o próprio)
            if (await _context.Vendas.AnyAsync(v => v.ProtocoloVenda == venda.ProtocoloVenda && v.VendaId != venda.VendaId))
                ModelState.AddModelError("ProtocoloVenda", "Já existe uma venda com este protocolo.");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venda);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Venda atualizada com sucesso!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendaExists(venda.VendaId))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Veiculos = _context.Veiculos.ToList();
            ViewBag.Concessionarias = _context.Concessionarias.ToList();
            ViewBag.Clientes = _context.Clientes.ToList();
            return View(venda);
        }

        // GET: Vendas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var venda = await _context.Vendas
                .Include(v => v.Veiculo)
                .Include(v => v.Concessionaria)
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(v => v.VendaId == id);
            if (venda == null) return NotFound();
            return View(venda);
        }

        // POST: Vendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venda = await _context.Vendas.FindAsync(id);
            if (venda == null) return NotFound();
            venda.IsDeleted = true;
            _context.Update(venda);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Venda removida com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private bool VendaExists(int id)
        {
            return _context.Vendas.Any(e => e.VendaId == id);
        }
    }
}
