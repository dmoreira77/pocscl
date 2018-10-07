using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.IntegracaoExterna.Rotas
{
    public class OtimizacaoRota: MensuracaoRota
    {
        private int[] sequenciaLocais;

        public int[] SequenciaLocais { get => sequenciaLocais; set => sequenciaLocais = value; }
    }
}
