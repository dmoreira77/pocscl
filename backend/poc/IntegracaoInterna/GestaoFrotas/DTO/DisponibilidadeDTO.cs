using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.IntegracaoInterna.GestaoFrotas.DTO
{
    public class DisponibilidadeDTO
    {
        private String dataHoraInicio;
        private String dataHoraFim;
        
        public string DataHoraInicio { get => dataHoraInicio; set => dataHoraInicio = value; }
        public string DataHoraFim { get => dataHoraFim; set => dataHoraFim = value; }
    }
}