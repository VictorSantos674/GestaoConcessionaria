using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using DesafioIntelectah.Data;
using DesafioIntelectah.ViewModels;

namespace DesafioIntelectah.Controllers
{
    [Authorize(Roles = "Administrador,Gerente")]
    public class ExportacaoController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ExportacaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> RelatorioMensalExcel(int? ano, int? mes)
        {
            int anoSelecionado = ano ?? DateTime.Now.Year;
            int mesSelecionado = mes ?? DateTime.Now.Month;
            var vendas = await _context.Vendas
                .Include(v => v.Veiculo).ThenInclude(v => v.Fabricante)
                .Include(v => v.Concessionaria)
                .Where(v => !v.IsDeleted && v.DataVenda.Year == anoSelecionado && v.DataVenda.Month == mesSelecionado)
                .ToListAsync();

            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("Relatório Mensal");
                ws.Cells[1, 1].Value = "Tipo de Veículo";
                ws.Cells[1, 2].Value = "Quantidade";
                ws.Cells[1, 3].Value = "Faturamento";
                int row = 2;
                var porTipo = vendas.GroupBy(v => v.Veiculo.TipoVeiculo.ToString())
                    .Select(g => new { Tipo = g.Key, Qtd = g.Count(), Fat = g.Sum(x => x.PrecoVenda) });
                foreach (var tipo in porTipo)
                {
                    ws.Cells[row, 1].Value = tipo.Tipo;
                    ws.Cells[row, 2].Value = tipo.Qtd;
                    ws.Cells[row, 3].Value = tipo.Fat;
                    row++;
                }
                row++;
                ws.Cells[row, 1].Value = "Fabricante";
                ws.Cells[row, 2].Value = "Quantidade";
                ws.Cells[row, 3].Value = "Faturamento";
                row++;
                var porFab = vendas.GroupBy(v => v.Veiculo.Fabricante.Nome)
                    .Select(g => new { Fab = g.Key, Qtd = g.Count(), Fat = g.Sum(x => x.PrecoVenda) });
                foreach (var fab in porFab)
                {
                    ws.Cells[row, 1].Value = fab.Fab;
                    ws.Cells[row, 2].Value = fab.Qtd;
                    ws.Cells[row, 3].Value = fab.Fat;
                    row++;
                }
                row++;
                ws.Cells[row, 1].Value = "Concessionária";
                ws.Cells[row, 2].Value = "Quantidade";
                ws.Cells[row, 3].Value = "Faturamento";
                row++;
                var porConc = vendas.GroupBy(v => v.Concessionaria.Nome)
                    .Select(g => new { Conc = g.Key, Qtd = g.Count(), Fat = g.Sum(x => x.PrecoVenda) });
                foreach (var conc in porConc)
                {
                    ws.Cells[row, 1].Value = conc.Conc;
                    ws.Cells[row, 2].Value = conc.Qtd;
                    ws.Cells[row, 3].Value = conc.Fat;
                    row++;
                }
                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                var bytes = package.GetAsByteArray();
                string fileName = $"RelatorioMensal_{mesSelecionado:D2}_{anoSelecionado}.xlsx";
                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
        [HttpGet]
        public async Task<IActionResult> RelatorioMensalPDF(int? ano, int? mes)
        {
            int anoSelecionado = ano ?? DateTime.Now.Year;
            int mesSelecionado = mes ?? DateTime.Now.Month;
            var vendas = await _context.Vendas
                .Include(v => v.Veiculo).ThenInclude(v => v.Fabricante)
                .Include(v => v.Concessionaria)
                .Where(v => !v.IsDeleted && v.DataVenda.Year == anoSelecionado && v.DataVenda.Month == mesSelecionado)
                .ToListAsync();

            var viewModel = new ViewModels.RelatorioVendasViewModel
            {
                Ano = anoSelecionado,
                Mes = mesSelecionado,
                TotalVendas = vendas.Count,
                TotalFaturado = vendas.Sum(v => v.PrecoVenda),
                VendasPorTipo = vendas
                    .GroupBy(v => v.Veiculo.TipoVeiculo.ToString())
                    .Select(g => new ViewModels.VendasPorTipoViewModel
                    {
                        TipoVeiculo = g.Key,
                        Quantidade = g.Count(),
                        Faturamento = g.Sum(x => x.PrecoVenda)
                    }).ToList(),
                VendasPorFabricante = vendas
                    .GroupBy(v => v.Veiculo.Fabricante.Nome)
                    .Select(g => new ViewModels.VendasPorFabricanteViewModel
                    {
                        Fabricante = g.Key,
                        Quantidade = g.Count(),
                        Faturamento = g.Sum(x => x.PrecoVenda)
                    }).ToList(),
                VendasPorConcessionaria = vendas
                    .GroupBy(v => v.Concessionaria.Nome)
                    .Select(g => new ViewModels.VendasPorConcessionariaViewModel
                    {
                        Concessionaria = g.Key,
                        Quantidade = g.Count(),
                        Faturamento = g.Sum(x => x.PrecoVenda)
                    }).ToList()
            };

            return new Rotativa.AspNetCore.ViewAsPdf("RelatorioMensal", viewModel)
            {
                FileName = $"RelatorioMensal_{mesSelecionado:D2}_{anoSelecionado}.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
    }
}
