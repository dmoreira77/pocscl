using poc.Cadastro;
using poc.Cadastro.DAO;
using poc.Cadastro.Modelo;
using poc.Coletas;
using poc.Coletas.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.IntegracaoExterna.Rotas
{
    public class FacadeRotas
    {
        private static Local[] derivarLocais(ParadaRoteiroColetaDTO[] paradas, CentroDistribuicao centroDistribuicao)
        {
            List<Local> locais = new List<Local>();
            locais.Add(
                new Local()
                {
                    Endereco = centroDistribuicao.Endereco,
                    Municipio = centroDistribuicao.Municipio.Nome,
                    Uf = centroDistribuicao.Municipio.Uf.Sigla
                }
            );

            foreach (var parada in paradas)
            {
                locais.Add(
                        new Local()
                        {
                            Endereco = parada.Endereco,
                            Municipio = parada.Municipio,
                            Uf = parada.Uf
                        }
                    );
            }

            locais.Add(
                new Local()
                {
                    Endereco = centroDistribuicao.Endereco,
                    Municipio = centroDistribuicao.Municipio.Nome,
                    Uf = centroDistribuicao.Municipio.Uf.Sigla
                }
            );

            return locais.ToArray();
        }

        private static ConectorRotas obterConectorRotas()
        {
            return new ConectorMapQuest();
        }

        public static RoteiroColetaDTO gerarRoteiroAutomatico(ParadaRoteiroColetaDTO[] paradas, CentroDistribuicao centroDistribuicao)
        {
            ConectorRotas conector = obterConectorRotas();
            RoteiroColetaDTO roteiro = null;

            Local[] locais = derivarLocais(paradas, centroDistribuicao);

            OtimizacaoRota o = conector.obterOtimizacaoRota(locais).Result;

            float distanciaTotal = o.DistanciaTotal;
            int[] indicesReordenados = o.SequenciaLocais;
            float[] distanciasTrechos = o.DistanciasTrechos;

            List<ParadaRoteiroColetaDTO> paradasReordenadas = new List<ParadaRoteiroColetaDTO>();

            foreach (var indice in indicesReordenados)
            {
                int i = indice;
                if ((i != 0) && (i != (indicesReordenados.Count() - 1)))
                {
                    paradasReordenadas.Add(paradas[i - 1]);
                }
            }

            for (int p = 0; p < paradasReordenadas.Count; p++)
            {
                float distanciaTrecho = distanciasTrechos[p];
                paradasReordenadas[p].Distancia = distanciaTrecho;
            }

            roteiro = new RoteiroColetaDTO();
            roteiro.Distancia = distanciaTotal;
            roteiro.Paradas = paradasReordenadas.ToArray();
                
            return roteiro;
        }

        public static RoteiroColetaDTO gerarRoteiroManual(ParadaRoteiroColetaDTO[] paradas, CentroDistribuicao centroDistribuicao)
        {
            ConectorRotas conector = new ConectorMapQuest();
            RoteiroColetaDTO roteiro = null;

            Local[] locais = derivarLocais(paradas, centroDistribuicao);

            MensuracaoRota m = conector.obterMensuracaoRota(locais).Result;

            float distanciaTotal = m.DistanciaTotal;
            float[] distanciasTrechos = m.DistanciasTrechos;

            for (int p = 0; p < paradas.Length; p++)
            {
                paradas[p].Distancia = distanciasTrechos[p];
            }

            roteiro = new RoteiroColetaDTO();
            roteiro.Distancia = distanciaTotal;
            roteiro.Paradas = paradas;

            return roteiro;
        }

        public static CentroDistribuicao[] centrosDistribuicaoMaisProximos(Local[] locais)
        {
            CentroDistribuicao[] cds = (new DAOCentroDistribuicao()).obterTodos().ToArray();
            

            Dictionary<String, CentroDistribuicao> d = new Dictionary<string, CentroDistribuicao>();
            List<CentroDistribuicao> resultado = new List<CentroDistribuicao>();
            
            foreach (var local in locais)
            {
                CentroDistribuicao cd;
                if (!d.TryGetValue(local.enderecoCompleto(), out cd))
                {
                    cd = centroDistribuicaoMaisProximo(local, cds);
                    d.Add(local.enderecoCompleto(), cd);
                }
                resultado.Add(cd);
            }

            return resultado.ToArray();
        }

        public static CentroDistribuicao centroDistribuicaoMaisProximo(Local referencia, CentroDistribuicao[] cds)
        {
            List<Local> locaisComparacao = new List<Local>();

            foreach (var cd in cds)
            {
                locaisComparacao.Add(new Local()
                {
                    Endereco = cd.Endereco,
                    Municipio = cd.Municipio.Nome,
                    Uf = cd.Municipio.Uf.Sigla
                });
            }

            int posicaoDoMaior = (int) obterConectorRotas().encontrarLocalMaisProximo(referencia, locaisComparacao.ToArray()).Result;
            if(posicaoDoMaior != -1)
            {
                return cds[posicaoDoMaior];
            }
            else
            {
                return null;
            }
        }
    }
}
