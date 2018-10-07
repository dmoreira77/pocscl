using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.IntegracaoExterna.Rotas
{
    public class MensuracaoRota
    {
        private float distanciaTotal;
        private float[] distanciasTrechos;

        public float DistanciaTotal { get => distanciaTotal; set => distanciaTotal = value; }
        public float[] DistanciasTrechos { get => distanciasTrechos; set => distanciasTrechos = value; }
    }
}
