using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.DesafioIntelectah.Data;
using src.DesafioIntelectah.Models;
using System.Linq;
using System.Threading.Tasks;

namespace src.DesafioIntelectah.Controllers
{
    [Authorize(Roles = "Administrador,Gerente")]
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clientes.AsNoTracking().ToListAsync());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.ClienteId == id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,CPF,Telefone")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                // Validação de unicidade de CPF
                if (await _context.Clientes.AnyAsync(c => c.CPF == cliente.CPF))
                {
                    ModelState.AddModelError("CPF", "Já existe um cliente com este CPF.");
                    return View(cliente);
                }
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cliente cadastrado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClienteId,Nome,CPF,Telefone")] Cliente cliente)
        {
            if (id != cliente.ClienteId) return NotFound();
            if (ModelState.IsValid)
            {
                // Validação de unicidade de CPF (exceto o próprio)
                if (await _context.Clientes.AnyAsync(c => c.CPF == cliente.CPF && c.ClienteId != cliente.ClienteId))
                {
                    ModelState.AddModelError("CPF", "Já existe um cliente com este CPF.");
                    return View(cliente);
                }
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cliente atualizado com sucesso!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.ClienteId))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.ClienteId == id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            cliente.IsDeleted = true;
            _context.Update(cliente);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Cliente removido com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.ClienteId == id);
        }
    }
}
