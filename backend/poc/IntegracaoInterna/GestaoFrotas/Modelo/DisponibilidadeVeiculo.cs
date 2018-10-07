using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.IntegracaoInterna.GestaoFrotas.Modelo
{
    public class DisponibilidadeVeiculo
    {
        private int codigo;
        private Veiculo veiculo;
        private DateTime dataHoraInicio;
        private DateTime? dataHoraFim;
        private String observacao;
        
        public int Codigo { get => codigo; set => codigo = value; }
        public Veiculo Veiculo { get => veiculo; set => veiculo = value; }
        public DateTime DataHoraInicio { get => dataHoraInicio; set => dataHoraInicio = value; }
        public DateTime? DataHoraFim { get => dataHoraFim; set => dataHoraFim = value; }
        public string Observacao { get => observacao; set => observacao = value; }
    }
}
