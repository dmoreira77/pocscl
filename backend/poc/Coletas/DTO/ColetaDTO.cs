using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.Coletas.DTO
{
    public class ColetaDTO
    {
        private ItemColetaDTO[] itens;
        private RoteiroColetaDTO roteiro;
        private String placa;
        private String dataPartida;
        private String horaPartida;
        private String dataRetornoEstimada;
        private String horaRetornoEstimada;

        public ItemColetaDTO[] Itens { get => itens; set => itens = value; }
        public RoteiroColetaDTO Roteiro { get => roteiro; set => roteiro = value; }
        public string Placa { get => placa; set => placa = value; }
        public string DataPartida { get => dataPartida; set => dataPartida = value; }
        public string HoraPartida { get => horaPartida; set => horaPartida = value; }
        public string DataRetornoEstimada { get => dataRetornoEstimada; set => dataRetornoEstimada = value; }
        public string HoraRetornoEstimada { get => horaRetornoEstimada; set => horaRetornoEstimada = value; }
    }
}
