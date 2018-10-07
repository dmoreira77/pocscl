using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.Coletas.DTO
{
    public class RoteiroColetaDTO
    {
        private ParadaRoteiroColetaDTO[] paradas;
        private float distancia;
        private String dataHoraRetorno;

        public ParadaRoteiroColetaDTO[] Paradas { get => paradas; set => paradas = value; }
        public float Distancia { get => distancia; set => distancia = value; }
        public string DataHoraRetorno { get => dataHoraRetorno; set => dataHoraRetorno = value; }
    }
}
