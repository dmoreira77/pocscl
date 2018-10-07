using poc.IntegracaoInterna.GestaoFrotas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.Coletas.DTO
{
    public class ParametrosCalculoRoteiroCompletoDTO
    {
        RoteiroColetaDTO roteiro;
        int velocidade;
        String dataHoraPartida;

        public RoteiroColetaDTO Roteiro { get => roteiro; set => roteiro = value; }
        public int Velocidade { get => velocidade; set => velocidade = value; }
        public string DataHoraPartida { get => dataHoraPartida; set => dataHoraPartida = value; }
    }
}
