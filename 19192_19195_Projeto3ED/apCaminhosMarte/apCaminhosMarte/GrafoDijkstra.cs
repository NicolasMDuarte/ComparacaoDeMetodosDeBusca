using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace apCaminhosMarte
{
    class GrafoDijkstra
    {
        private const int NUM_VERTICES = 23;
        private Vertice[] vertices;
        private int[,] adjMatrix;
        int numVerts;
        DataGridView dgv; // para exibir a matriz de adjacência num formulário
        /// DIJKSTRA
        DistOriginal[] percurso;
        int infinity = int.MaxValue;
        int verticeAtual; // global usada para indicar o vértice atualmente sendo visitado
        int doInicioAteAtual; // global usada para ajustar menor caminho com Djikstra
        int nTree;

        public GrafoDijkstra(DataGridView dgv)
        {
            this.dgv = dgv;
            vertices = new Vertice[NUM_VERTICES];
            adjMatrix = new int[NUM_VERTICES, NUM_VERTICES];

            numVerts = 0;
            nTree = 0;
            for (int j = 0; j < NUM_VERTICES; j++) // zera toda a matriz
                for (int k = 0; k < NUM_VERTICES; k++)
                    adjMatrix[j, k] = infinity; // distância tão grande que não existe
            percurso = new DistOriginal[NUM_VERTICES];
        }

        public void ConstruirGrafo(string arquivoCidades, string arquivoLigacoes, string metodo)
        {
            if (arquivoCidades == null || arquivoCidades.Equals(""))
                throw new Exception("Nome de arquivo invalido");

            var arquivo = new StreamReader(arquivoCidades);
            while (!arquivo.EndOfStream)
            {
                string linha = arquivo.ReadLine();
                int id = int.Parse(linha.Substring(0, 3));
                string nomeCidade = linha.Substring(3, 15);
                int x = int.Parse(linha.Substring(18, 5));
                int y = int.Parse(linha.Substring(23, 5));

                var cidadeMarte = new CidadeMarte(id, nomeCidade, x, y);
                this.NovoVertice(cidadeMarte.Id.ToString());
            }

            arquivo.Close();

            arquivo = new StreamReader(arquivoLigacoes);
            while (!arquivo.EndOfStream)
            {
                string linha = arquivo.ReadLine();
                int origem = int.Parse(linha.Substring(0, 3));
                int destino = int.Parse(linha.Substring(3, 3));
                int distancia = int.Parse(linha.Substring(6, 5));
                int tempo = int.Parse(linha.Substring(11, 4));
                int custo = int.Parse(linha.Substring(15, 5));

                if (metodo == "Distância")
                    this.NovaAresta(origem, destino, distancia);
                if (metodo == "Custo ($)")
                    this.NovaAresta(origem, destino, custo);
                if (metodo == "Tempo")
                    this.NovaAresta(origem, destino, tempo);
            }

            arquivo.Close();
        }

        public void NovoVertice(string label)
        {
            vertices[numVerts] = new Vertice(label);
            numVerts++;
            if (dgv != null) // se foi passado como parâmetro um dataGridView para exibição
            { // se realiza o seu ajuste para a quantidade de vértices
                dgv.RowCount = numVerts + 1;
                dgv.ColumnCount = numVerts + 1;
                dgv.Columns[numVerts].Width = 45;
            }
        }

        public void NovaAresta(int origem, int destino, int peso)
        {
            adjMatrix[origem, destino] = peso; // sobrecarga do método anterior
        }
        // demais métodos anteriores : ordenação topológica, árvore geradora mínima,
        // percursos – ajustar para comparar com infinity e não com zero

        public string Caminho(int inicioDoPercurso, int finalDoPercurso, ListBox lista)
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
                int distanciaMinima = percurso[indiceDoMenor].distancia;
                // o vértice com a menor distância passa a ser o vértice atual
                // para compararmos com a distância calculada em AjustarMenorCaminho()
                verticeAtual = indiceDoMenor;
                doInicioAteAtual = percurso[indiceDoMenor].distancia;

                // visitamos o vértice com a menor distância desde o inicioDoPercurso
                vertices[verticeAtual].foiVisitado = true;
                AjustarMenorCaminho(lista);
            }
            return ExibirPercursos(inicioDoPercurso, finalDoPercurso, lista);
        }
        public int ObterMenor()
        {
            int distanciaMinima = infinity;
            int indiceDaMinima = 0;
            for (int j = 0; j < numVerts; j++)
                if (!(vertices[j].foiVisitado) && (percurso[j].distancia < distanciaMinima))
                {
                    distanciaMinima = percurso[j].distancia;
                    indiceDaMinima = j;
                }
            return indiceDaMinima;
        }
        public void AjustarMenorCaminho(ListBox lista)
        {
            for (int coluna = 0; coluna < numVerts; coluna++)
                if (!vertices[coluna].foiVisitado) // para cada vértice ainda não visitado
                {
                    // acessamos a distância desde o vértice atual (pode ser infinity)
                    int atualAteMargem = adjMatrix[verticeAtual, coluna];
                    // calculamos a distância desde inicioDoPercurso passando por vertice atual até
                    // esta saída
                    int doInicioAteMargem = doInicioAteAtual + atualAteMargem;
                    // quando encontra uma distância menor, marca o vértice a partir do
                    // qual chegamos no vértice de índice coluna, e a soma da distância
                    // percorrida para nele chegar
                    int distanciaDoCaminho = percurso[coluna].distancia;
                    if (doInicioAteMargem < distanciaDoCaminho)
                    {
                        percurso[coluna].verticePai = verticeAtual;
                        percurso[coluna].distancia = doInicioAteMargem;
                        ExibirTabela(lista);
                    }
                }
            lista.Items.Add("==================Caminho ajustado==============");
            lista.Items.Add(" ");
        }

        public void ExibirTabela(ListBox lista)
        {
            string dist = "";
            lista.Items.Add("Vértice\tVisitado?\tPeso\tVindo de");
            for (int i = 0; i < numVerts; i++)
            {
                if (percurso[i].distancia == infinity)
                    dist = "inf";
                else
                    dist = Convert.ToString(percurso[i].distancia);
                lista.Items.Add(vertices[i].rotulo + "\t" + vertices[i].foiVisitado +
                "\t\t" + dist + "\t" + vertices[percurso[i].verticePai].rotulo);
            }
            lista.Items.Add("-----------------------------------------------------");
        }
        public string ExibirPercursos(int inicioDoPercurso, int finalDoPercurso,
        ListBox lista)
        {
            string resultado = "";
            for (int j = 0; j < numVerts; j++)
            {
                resultado += vertices[j].rotulo + "=";
                if (percurso[j].distancia == infinity)
                    resultado += "inf";
                else
                    resultado += percurso[j].distancia + " ";
                string pai = vertices[percurso[j].verticePai].rotulo;
                resultado += "(" + pai + ") ";
            }
            lista.Items.Add(resultado);
            lista.Items.Add(" ");
            lista.Items.Add(" ");
            lista.Items.Add("Caminho entre " + vertices[inicioDoPercurso].rotulo +
            " e " + vertices[finalDoPercurso].rotulo);
            lista.Items.Add(" ");
            int onde = finalDoPercurso;
            Stack<string> pilha = new Stack<string>();
            int cont = 0;
            while (onde != inicioDoPercurso)
            {
                onde = percurso[onde].verticePai;
                pilha.Push(vertices[onde].rotulo);
                cont++;
            }
            resultado = "";
            while (pilha.Count != 0)
            {
                resultado += pilha.Pop();
                if (pilha.Count != 0)
                    resultado += " --> ";
            }

            if ((cont == 1) && (percurso[finalDoPercurso].distancia == infinity))
                resultado = "Não há caminho";
            else
                resultado += " --> " + vertices[finalDoPercurso].rotulo;
            return resultado;
        }
    }
}
