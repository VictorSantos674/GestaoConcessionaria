using System;
using System.Collections.Generic;

namespace DesafioIntelectah.ViewModels
{
    public class RelatorioVendasViewModel
    {
        public int Ano { get; set; }
        public int Mes { get; set; }
        public int TotalVendas { get; set; }
        public decimal TotalFaturado { get; set; }
        public List<VendasPorTipoViewModel> VendasPorTipo { get; set; }
        public List<VendasPorFabricanteViewModel> VendasPorFabricante { get; set; }
        public List<VendasPorConcessionariaViewModel> VendasPorConcessionaria { get; set; }
    }

    public class VendasPorTipoViewModel
    {
        public string TipoVeiculo { get; set; }
        public int Quantidade { get; set; }
        public decimal Faturamento { get; set; }
    }

    public class VendasPorFabricanteViewModel
    {
        public string Fabricante { get; set; }
        public int Quantidade { get; set; }
        public decimal Faturamento { get; set; }
    }

    public class VendasPorConcessionariaViewModel
    {
        public string Concessionaria { get; set; }
        public int Quantidade { get; set; }
        public decimal Faturamento { get; set; }
    }
}
