using poc.Coletas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.IntegracaoExterna.Rotas
{
    public abstract class ConectorRotas
    {
        public abstract Task<MensuracaoRota> obterMensuracaoRota(Local[] locais);
        public abstract Task<OtimizacaoRota> obterOtimizacaoRota(Local[] locais);
        public abstract Task<int> encontrarLocalMaisProximo(Local referencia, Local[] locais);
    }
}
