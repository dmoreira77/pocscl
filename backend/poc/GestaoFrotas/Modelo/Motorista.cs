using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.GestaoFrotas.Modelo
{
    public class Motorista
    {
        private String cpfMotorista;
        private String nome;

        public string CpfMotorista { get => cpfMotorista; set => cpfMotorista = value; }
        public string Nome { get => nome; set => nome = value; }
    }
}
