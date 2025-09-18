namespace DesafioIntelectah.ViewModels
{
    public class VendaViewModel
    {
        public int VendaID { get; set; }
        public int VeiculoID { get; set; }
        public int ConcessionariaID { get; set; }
        public string VeiculoNome { get; set; }
        public string ClienteNome { get; set; }
        public string ClienteCPF { get; set; }
        public string ClienteTelefone { get; set; }
        public string ConcessionariaNome { get; set; }
        public decimal PrecoVenda { get; set; }
        public DateTime DataVenda { get; set; }
        public string Protocolo { get; set; }
    }
}
