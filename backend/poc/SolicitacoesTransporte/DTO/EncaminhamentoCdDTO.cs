using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.SolicitacoesTransporte.DTO
{
    public class EncaminhamentoCdDTO
    {
        private int codigoItem;
        private int codigoCD;

        public int CodigoItem { get => codigoItem; set => codigoItem = value; }
        public int CodigoCD { get => codigoCD; set => codigoCD = value; }
    }
}
