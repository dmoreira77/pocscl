using Newtonsoft.Json.Linq;
using poc.Cadastro;
using poc.Coletas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace poc.IntegracaoExterna.Rotas
{
    public class ConectorMapQuest : ConectorRotas
    {
        String key = "SwXQ0fVkucfXyCeWWeBArYPkrHUOLjy7";
        const float coeficienteConversãoKm = 1.60934f;

        public async override Task<OtimizacaoRota> obterOtimizacaoRota(Local[] locais)
        {
            List<String> enderecos = new List<string>();
            foreach (var local in locais)
            {
                enderecos.Add(prepararEndereco(local));
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://www.mapquestapi.com/directions/v2/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsJsonAsync("optimizedroute?key=" + key, new { locations = enderecos });

                if (response.IsSuccessStatusCode)
                {
                    dynamic resultado = await response.Content.ReadAsAsync<dynamic>();

                    float distanciaTotal = ((float) resultado.route.distance) * coeficienteConversãoKm;

                    JArray sequencia = resultado.route.locationSequence;
                    int[] sequenciaReordenada = new int[sequencia.Count];
                    int i = 0;
                    foreach (var indice in sequencia)
                    {
                        sequenciaReordenada[i] = (int)indice;
                        i++;
                    }

                    JArray trechos = resultado.route.legs;
                    float[] distanciasTrechos = new float[trechos.Count];

                    for (int t = 0; t < trechos.Count; t++)
                    {
                        float distanciaTrecho;
                        dynamic trecho = trechos.ElementAt(t);
                        distanciaTrecho = ((float)trecho.distance) * coeficienteConversãoKm;
                        distanciasTrechos[t] = distanciaTrecho;
                    }

                    OtimizacaoRota o = new OtimizacaoRota()
                    {
                        DistanciasTrechos = distanciasTrechos,
                        DistanciaTotal = distanciaTotal,
                        SequenciaLocais = sequenciaReordenada
                    };

                    return o;
                }
            }

            return null;
        }

        public async override Task<MensuracaoRota> obterMensuracaoRota(Local[] locais)
        {
            MensuracaoRota m = null;

            List<String> enderecos = new List<string>();
            foreach (var local in locais)
            {
                enderecos.Add(prepararEndereco(local));
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://www.mapquestapi.com/directions/v2/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsJsonAsync("routematrix?key=" + key, new { locations = enderecos, options = new { allToAll = true } });

                if (response.IsSuccessStatusCode)
                {
                    float distanciaTotal = 0;
                    float[] distanciasTrechos = new float[locais.Count() - 1];

                    dynamic resultado = await response.Content.ReadAsAsync<dynamic>();
                    JArray matrizDistancias = resultado.distance;

                    int i = 0;
                    foreach (var vetor in matrizDistancias)
                    {
                        if (i < (vetor.Count() - 1))
                        {
                            float distanciaTrecho = (float)vetor.SelectToken("[" + (i + 1) + "]");
                            distanciaTrecho *= coeficienteConversãoKm;
                            distanciaTotal += distanciaTrecho;
                            distanciasTrechos[i] = distanciaTrecho;
                            i++;
                        }
                    }

                    m = new MensuracaoRota()
                    {
                        DistanciasTrechos = distanciasTrechos,
                        DistanciaTotal = distanciaTotal
                    };
                }
            }

            return m;
        }

        private String prepararEndereco(Local local)
        {
            String enderecoRes = local.Endereco + " - " + local.Municipio.ToString() + " - Brasil";
            enderecoRes = enderecoRes.ToUpper();

            String de   = "ÂÃÁÀÊÉÈÎÍÌÔÕÓÒÚÙÛÇ";
            String para = "AAAAEEEIIIOOOOUUUC";

            for(int i=0; i < de.Length; i++)
            {
                enderecoRes = enderecoRes.Replace(de[i], para[i]);
            }

            return enderecoRes;
        }

        public override async Task<int> encontrarLocalMaisProximo(Local referencia, Local[] locais)
        {
            if (locais.Length == 1) return 0;

            List<String> enderecos = new List<string>();

            enderecos.Add(prepararEndereco(referencia));
            foreach (var local in locais)
            {
                enderecos.Add(prepararEndereco(local));
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://www.mapquestapi.com/directions/v2/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsJsonAsync("routematrix?key=" + key, new { locations = enderecos, options = new { manyToOne = true } });

                if (response.IsSuccessStatusCode)
                {
                    dynamic resultado = await response.Content.ReadAsAsync<dynamic>();
                    JArray vetorDistancias = resultado.distance;

                    int iMenorDistancia = 1;
                    for(var i = 2; i < vetorDistancias.Count; i++)
                    {
                        if(((float)vetorDistancias[iMenorDistancia]) > ((float)vetorDistancias[i]))
                        {
                            iMenorDistancia = i;
                        }
                    }

                    return iMenorDistancia - 1;
                }
            }

            return -1;
        }
    }
}
