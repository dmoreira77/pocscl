using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.AcompanhamentoLogistica.DTO
{
    public class ViagemDTO
    {
        private int codigo;
        private String placa;
        private String cpfMotorista;
        private String nomeMotorista;
        private String tipoViagem;
        private IEnumerable<ParadaDTO> paradas;

        public int Codigo { get => codigo; set => codigo = value; }
        public string Placa { get => placa; set => placa = value; }
        public string CpfMotorista { get => cpfMotorista; set => cpfMotorista = value; }
        public string NomeMotorista { get => nomeMotorista; set => nomeMotorista = value; }
        public string TipoViagem { get => tipoViagem; set => tipoViagem = value; }
        public IEnumerable<ParadaDTO> Paradas { get => paradas; set => paradas = value; }
    }
}
