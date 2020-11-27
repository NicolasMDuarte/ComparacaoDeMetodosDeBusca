using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace apCaminhosMarte
{
    class GrafoDijkstra
    {
        private Vertice[] vertices;
        private int[,] adjMatrix;
        int numVerts;

        DistOriginal[] percurso;
        int infinity = int.MaxValue;
        int verticeAtual;
        long doInicioAteAtual;
        string crit;

        public GrafoDijkstra(GrafoBacktracking grafo)
        {
            vertices = new Vertice[grafo.Matriz.GetLength(0)];
            adjMatrix = new int[grafo.Matriz.GetLength(0), grafo.Matriz.GetLength(0)];
            numVerts = 0;

            for (int j = 0; j < adjMatrix.GetLength(0); j++) // zera toda a matriz
                for (int k = 0; k < adjMatrix.GetLength(0); k++)
                    adjMatrix[j, k] = infinity; // distância tão grande que não existe

            percurso = new DistOriginal[adjMatrix.GetLength(0)];
        }

        public void NovoVertice(string rotulo)
        {
            vertices[numVerts] = new Vertice(rotulo);
            numVerts++;
        }

        public void NovaAresta(int origem, int destino, int peso)
        {
            adjMatrix[origem, destino] = peso;
        }

        public void ConstruirGrafo(string arqCid, string arqCam, string crit)
        {
            this.crit = crit;

            var cidades = new StreamReader(arqCid);
            var caminhos = new StreamReader(arqCam);

            while (!cidades.EndOfStream)
            {
                string linha = cidades.ReadLine();
                int cidade = int.Parse(linha.Substring(0, 3));

                NovoVertice(cidade.ToString());
            }
            cidades.Close();

            while (!caminhos.EndOfStream)
            {
                string linha = caminhos.ReadLine();
                int origem = int.Parse(linha.Substring(0, 3));
                int destino = int.Parse(linha.Substring(3, 3));

                int aux = 0;
                if (crit == "Distância")
                    aux = int.Parse(linha.Substring(6, 5));
                if (crit == "Custo ($)")
                    aux = int.Parse(linha.Substring(11, 4));
                if (crit == "Tempo")
                    aux = int.Parse(linha.Substring(15, 5));


                NovaAresta(origem, destino, aux);
            }
            caminhos.Close();
        }

        public PilhaLista<Movimento> Caminho(int inicioDoPercurso, int finalDoPercurso)
        {
            for (int j = 0; j < numVerts; j++)
                vertices[j].foiVisitado = false;

            vertices[inicioDoPercurso].foiVisitado = true;

            for (int j = 0; j < numVerts; j++)
            {
                // anotamos no vetor percurso a distância entre o inicioDoPercurso e cada vértice
                // se não há ligação direta, o valor da distância será infinity
                int tempDist = adjMatrix[inicioDoPercurso, j];
                percurso[j] = new DistOriginal(inicioDoPercurso, tempDist);
            }

            for (int nTree = 0; nTree < numVerts; nTree++)
            {
                // Procuramos a saída não visitada do vértice inicioDoPercurso com a menor distância
                int indiceDoMenor = ObterMenor();
                // e anotamos essa menor distância
                long distanciaMinima = percurso[indiceDoMenor].distancia;
                // o vértice com a menor distância passa a ser o vértice atual
                // para compararmos com a distância calculada em AjustarMenorCaminho()
                verticeAtual = indiceDoMenor;
                doInicioAteAtual = percurso[indiceDoMenor].distancia;
                // visitamos o vértice com a menor distância desde o inicioDoPercurso
                vertices[verticeAtual].foiVisitado = true;
                AjustarMenorCaminho();
            }

            return ExibirPercursos(inicioDoPercurso, finalDoPercurso);
        }

        public PilhaLista<Movimento> ExibirPercursos(int inicioPercurso, int fimPercurso)
        {
            int cont = 0;
            var caminho = new PilhaLista<Movimento>();
            Movimento mov;
            bool primeira = true;
            int pop = 0;
            Stack<string> pilha = new Stack<string>();

            int onde = fimPercurso;
            while (onde != inicioPercurso)
            {
                pilha.Push(vertices[onde].rotulo);
                onde = percurso[onde].verticePai;
                cont++;
            }

            LigacaoCidade lig = new LigacaoCidade(0, 0, 0);
            int seg = int.Parse(pilha.Peek());
            if (crit == "Distância")
            {
                lig.Distancia = adjMatrix[inicioPercurso, seg];
            }
            if (crit == "Custo ($)")
            {
                lig.Custo = adjMatrix[inicioPercurso, seg];
            }
            if (crit == "Tempo")
            {
                lig.Tempo = adjMatrix[inicioPercurso, seg];
            }

            if (fimPercurso != int.Parse(pilha.Peek())) // Verificação para que possamos executar o código da linha 186 sem problemas caso necessário
                caminho.Empilhar(new Movimento(inicioPercurso, seg, lig));

            mov = new Movimento(100, 200, new LigacaoCidade(0, 0, 0));
            // Enquanto ainda tiver dados para coletar
            while (pilha.Count != 0)
            {
                pop = int.Parse(pilha.Pop());

                if (primeira)
                {
                    mov.Origem = pop;
                    primeira = false;
                }
                else
                {
                    mov.Destino = pop;

                    if (crit == "Distância")
                        mov.Lc.Distancia = adjMatrix[mov.Origem, mov.Destino];
                    if (crit == "Custo ($)")
                        mov.Lc.Custo = adjMatrix[mov.Origem, mov.Destino];
                    if (crit == "Tempo")
                        mov.Lc.Tempo = adjMatrix[mov.Origem, mov.Destino];

                    caminho.Empilhar((Movimento) mov.Clone());
                    mov.Origem = pop;
                }
            }

            if ((cont == 1) && (percurso[fimPercurso].distancia == infinity))
                throw new Exception("Não há caminhos!");
            else
            {
                int prim = int.Parse(vertices[inicioPercurso].rotulo);
                seg = int.Parse(vertices[fimPercurso].rotulo);
                lig = new LigacaoCidade(0, 0, 0);
                if (caminho.GetQtd() == 0) // Caso o código anterior não funcione, colocamos à força
                {
                    if (crit == "Distância")
                    {
                        lig.Distancia = adjMatrix[prim, seg];
                        caminho.Empilhar(new Movimento(prim, seg, lig));
                    }
                    if (crit == "Custo ($)")
                    {
                        lig.Custo = adjMatrix[prim, seg];
                        caminho.Empilhar(new Movimento(prim, seg, lig));
                    }
                    if (crit == "Tempo")
                    {
                        lig.Tempo = adjMatrix[prim, seg];
                        caminho.Empilhar(new Movimento(prim, seg, lig));
                    }
                }
            }

            return caminho;
        }

        public int ObterMenor()
        {
            long distanciaMinima = infinity;
            int indiceDaMinima = 0;
            for (int j = 0; j < numVerts; j++)
                if (!(vertices[j].foiVisitado) && (percurso[j].distancia < distanciaMinima) && (percurso[j].distancia != infinity))
                {
                    distanciaMinima = percurso[j].distancia;
                    indiceDaMinima = j;
                }
            return indiceDaMinima;
        }

        public void AjustarMenorCaminho()
        {
            for (int coluna = 0; coluna < numVerts; coluna++)
                if (!vertices[coluna].foiVisitado) // para cada vértice ainda não visitado
                {
                    // acessamos a distância desde o vértice atual (pode ser infinity)
                    int atualAteMargem = adjMatrix[verticeAtual, coluna];
                    // calculamos a distância desde inicioDoPercurso passando por vertice atual até
                    // esta saída
                    long doInicioAteMargem = doInicioAteAtual + atualAteMargem;
                    // quando encontra uma distância menor, marca o vértice a partir do
                    // qual chegamos no vértice de índice coluna, e a soma da distância
                    // percorrida para nele chegar
                    long distanciaDoCaminho = percurso[coluna].distancia;
                    if (doInicioAteMargem < distanciaDoCaminho)
                    {
                        percurso[coluna].verticePai = verticeAtual;
                        percurso[coluna].distancia = doInicioAteMargem;
                    }
                }
        }
    }
}