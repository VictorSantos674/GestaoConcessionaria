using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DesafioIntelectah.Data;
using DesafioIntelectah.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioIntelectah.Controllers
{
    [Authorize(Roles = "Administrador,Gerente,Vendedor")]
    public class VendasController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IDistributedCache _cache;
        public VendasController(ApplicationDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: Vendas/RelatorioMensal
        [Authorize(Roles = "Administrador,Gerente")]
        public async Task<IActionResult> RelatorioMensal(int? ano, int? mes)
        {
            int anoSelecionado = ano ?? DateTime.Now.Year;
            int mesSelecionado = mes ?? DateTime.Now.Month;
            string cacheKey = $"relatorio_{anoSelecionado}_{mesSelecionado}";
            ViewModels.RelatorioVendasViewModel? viewModel = null;
            var cached = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cached))
            {
                viewModel = System.Text.Json.JsonSerializer.Deserialize<ViewModels.RelatorioVendasViewModel>(cached);
            }
            else
            {

                var vendas = await _context.Vendas
                    .Include(v => v.Veiculo).ThenInclude(v => v.Fabricante)
                    .Include(v => v.Concessionaria)
                    .Where(v => !v.IsDeleted && v.DataVenda.Year == anoSelecionado && v.DataVenda.Month == mesSelecionado)
                    .ToListAsync();

                // Protege contra possíveis nulos
                var vendasValidas = vendas.Where(v => v.Veiculo != null && v.Concessionaria != null && v.Veiculo.Fabricante != null).ToList();

                viewModel = new ViewModels.RelatorioVendasViewModel
                {
                    Ano = anoSelecionado,
                    Mes = mesSelecionado,
                    TotalVendas = vendasValidas.Count,
                    TotalFaturado = vendasValidas.Sum(v => v.PrecoVenda),
                    VendasPorTipo = vendasValidas
                        .GroupBy(v => v.Veiculo!.TipoVeiculo.ToString())
                        .Select(g => new ViewModels.VendasPorTipoViewModel
                        {
                            TipoVeiculo = g.Key,
                            Quantidade = g.Count(),
                            Faturamento = g.Sum(x => x.PrecoVenda)
                        }).ToList(),
                    VendasPorFabricante = vendasValidas
                        .GroupBy(v => v.Veiculo!.Fabricante!.Nome)
                        .Select(g => new ViewModels.VendasPorFabricanteViewModel
                        {
                            Fabricante = g.Key,
                            Quantidade = g.Count(),
                            Faturamento = g.Sum(x => x.PrecoVenda)
                        }).ToList(),
                    VendasPorConcessionaria = vendasValidas
                        .GroupBy(v => v.Concessionaria!.Nome)
                        .Select(g => new ViewModels.VendasPorConcessionariaViewModel
                        {
                            Concessionaria = g.Key,
                            Quantidade = g.Count(),
                            Faturamento = g.Sum(x => x.PrecoVenda)
                        }).ToList()
                };
                var options = new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };
                await _cache.SetStringAsync(cacheKey, System.Text.Json.JsonSerializer.Serialize(viewModel), options);
            }
            return View(viewModel);
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
            ViewBag.Veiculos = _context.Veiculos.AsNoTracking().ToList();
            ViewBag.Concessionarias = _context.Concessionarias.AsNoTracking().ToList();
            ViewBag.Clientes = _context.Clientes.AsNoTracking().ToList();
            return View();
        }

        // POST: Vendas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("DataVenda,PrecoVenda,VeiculoID,ConcessionariaID,ClienteID")] Venda venda)
        {
            // Geração de protocolo único
            venda.ProtocoloVenda = Guid.NewGuid().ToString().Substring(0, 20);

            var veiculo = await _context.Veiculos.FindAsync(venda.VeiculoID);
            if (veiculo == null)
                ModelState.AddModelError("VeiculoID", "Selecione um veículo válido.");
            else if (venda.PrecoVenda > veiculo.Preco)
                ModelState.AddModelError("PrecoVenda", "O preço de venda não pode ser maior que o preço do veículo.");

            if (await _context.Vendas.AnyAsync(v => v.VeiculoID == venda.VeiculoID && !v.IsDeleted))
                ModelState.AddModelError("VeiculoID", "Este veículo já foi vendido e não pode ser vendido novamente.");

            if (await _context.Vendas.AnyAsync(v => v.ProtocoloVenda == venda.ProtocoloVenda))
                ModelState.AddModelError("ProtocoloVenda", "Já existe uma venda com este protocolo. Tente novamente.");

            if (ModelState.IsValid)
            {
                _context.Add(venda);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Venda registrada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Veiculos = _context.Veiculos.AsNoTracking().ToList();
            ViewBag.Concessionarias = _context.Concessionarias.AsNoTracking().ToList();
            ViewBag.Clientes = _context.Clientes.AsNoTracking().ToList();
            return View(venda);
        }

        // GET: Vendas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var venda = await _context.Vendas.FindAsync(id);
            if (venda == null) return NotFound();
            ViewBag.Veiculos = _context.Veiculos.AsNoTracking().ToList();
            ViewBag.Concessionarias = _context.Concessionarias.AsNoTracking().ToList();
            ViewBag.Clientes = _context.Clientes.AsNoTracking().ToList();
            return View(venda);
        }

        // POST: Vendas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("VendaId,DataVenda,PrecoVenda,ProtocoloVenda,VeiculoID,ConcessionariaID,ClienteID")] Venda venda)
        {
            if (id != venda.VendaId) return NotFound();
            var veiculo = await _context.Veiculos.FindAsync(venda.VeiculoID);
            if (veiculo == null)
                ModelState.AddModelError("VeiculoID", "Selecione um veículo válido.");
            else if (venda.PrecoVenda > veiculo.Preco)
                ModelState.AddModelError("PrecoVenda", "O preço de venda não pode ser maior que o preço do veículo.");

            if (await _context.Vendas.AnyAsync(v => v.VeiculoID == venda.VeiculoID && v.VendaId != venda.VendaId && !v.IsDeleted))
                ModelState.AddModelError("VeiculoID", "Este veículo já foi vendido e não pode ser vendido novamente.");

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
            ViewBag.Veiculos = _context.Veiculos.AsNoTracking().ToList();
            ViewBag.Concessionarias = _context.Concessionarias.AsNoTracking().ToList();
            ViewBag.Clientes = _context.Clientes.AsNoTracking().ToList();
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