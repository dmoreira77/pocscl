using poc.GestaoFrotas.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.IntegracaoInterna.GestaoFrotas
{
    public class FacadeGestaoFrotas
    {
        public IEnumerable<VeiculoDisponivelDTO> obterVeiculosDisponiveis(String dataLimite, float distancia, int qtdParadas, float pesoTotal, float volumeTotal)
        {
            return (new ConectorGestaoFrotas()).obterVeiculosDisponiveis(dataLimite, distancia, qtdParadas, pesoTotal, volumeTotal);
        }

        public object atualizarDisponibilidade(String placa, DateTime dataHoraInicio, DateTime dataHoraFimPrevisto)
        {
            return (new ConectorGestaoFrotas()).atualizarDisponibilidade(placa, dataHoraInicio, dataHoraFimPrevisto);
        }
    }
}
