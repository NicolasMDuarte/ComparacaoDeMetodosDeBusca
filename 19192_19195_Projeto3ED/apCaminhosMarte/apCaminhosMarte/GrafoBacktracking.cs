﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    // Nome: Pedro Go Iqueda RA: 19195
    // Nome: Nícolas Maisonnette Duarte RA: 19192
    class GrafoBacktracking
    {
        // Atributo que representa um grafo
        private LigacaoCidade[,] matriz;
        // Atributo que representa o nome de um arquivo texto
        private string nomeArquivo;
        // Atributo utilizado no método de busca de caminhos e indica a cidade atual da busca
        private int cidadeAtual;
        // Atributo utilizado no método de busca de caminhos e contém todos os movimentos realizados durante a busca
        private PilhaLista<Movimento> pilha;
        // Atributo utilizado no método de busca de caminhos e contém todos os caminhos encontrados
        private PilhaLista<PilhaLista<Movimento>> caminhos;

        // Construtor da classe
        public GrafoBacktracking(string nomeArquivo)
        {
            NomeArquivo = nomeArquivo;
            Matriz = new LigacaoCidade[23, 23];
            ConstruirGrafo();
        }

        // Propriedade do atributo nomeArquivo
        public string NomeArquivo
        {
            get => nomeArquivo;
            set
            {
                if (value == null || value.Equals(""))
                    throw new Exception("Nome de arquivo invalido");

                nomeArquivo = value;
            }
        }

        // Propriedade do atributo matriz
        public LigacaoCidade[,] Matriz
        {
            get => matriz;
            set
            {
                matriz = value ?? throw new Exception("Matriz invalida");
            }
        }

        // Método que constroi um grafo a partir de um arquivo texto
        private void ConstruirGrafo()
        {
            var arquivo = new StreamReader(nomeArquivo);
            while (!arquivo.EndOfStream)
            {
                string linha = arquivo.ReadLine();
                int origem = int.Parse(linha.Substring(0, 3));
                int destino = int.Parse(linha.Substring(3, 3));
                int distancia = int.Parse(linha.Substring(6, 5));
                int tempo = int.Parse(linha.Substring(11, 4));
                int custo = int.Parse(linha.Substring(15, 5));

                var ligacaoCidade = new LigacaoCidade(distancia, tempo, custo);
                matriz[origem, destino] = ligacaoCidade;
            }

            arquivo.Close();
        }

        // Método que chamará a busca de caminhos, preparando variáveis que serão utilizadas na futura busca
        public PilhaLista<PilhaLista<Movimento>> ProcurarCaminhos(int origem, int destino, string tipo)
        {
            cidadeAtual = origem;
            pilha = new PilhaLista<Movimento>();
            caminhos = new PilhaLista<PilhaLista<Movimento>>();

            if (tipo == "Recursão") // Se o tipo de busca escolhido for recursão
                return ProcurarCaminhos(destino);

            return ProcurarCaminhosPilhas(destino);
        }

        // Método que realiza a busca de caminhos entre duas cidades
        private PilhaLista<PilhaLista<Movimento>> ProcurarCaminhos(int destino)
        {
            for (int i = 0; i < matriz.GetLength(0); i++) // Testa todas cidades da matriz de adjacências
            {
                if (matriz[cidadeAtual, i] != null) // Verifica se existe ligação
                {
                    var movimentoObtido = new Movimento(cidadeAtual, i, matriz[cidadeAtual, i]);
                    pilha.Empilhar(movimentoObtido);
                    cidadeAtual = i;

                    if (cidadeAtual == destino) // Um caminho foi encontrado
                        AchouCaminho();
                    else
                    {
                        ProcurarCaminhos(destino);
                        var movimentoAnterior = pilha.Desempilhar(); // Uma saída não foi encontrada, portanto volta para uma cidade anterior
                        cidadeAtual = movimentoAnterior.Origem;
                    }
                }
            }

            return caminhos;

            void AchouCaminho() // Procedimento feito ao encontrar-se um caminho
            {
                PilhaLista<Movimento> pilhaClone = (PilhaLista<Movimento>)pilha.Clone();
                caminhos.Empilhar(pilhaClone);
                var movimentoAnterior = pilha.Desempilhar();
                cidadeAtual = movimentoAnterior.Origem;
            }
        }

        private PilhaLista<PilhaLista<Movimento>> ProcurarCaminhosPilhas(int destino)
        {
            int i = 0;
            while (true)
            {
                while (i < matriz.GetLength(0)) // Testa todas as cidades da matriz de adjacências
                {
                    bool vaiVoltar = false;

                    if (matriz[cidadeAtual, i] != null) // Verifica se existe ligação
                    {
                        vaiVoltar = true;

                        var movimentoObtido = new Movimento(cidadeAtual, i, matriz[cidadeAtual, i], i);
                        pilha.Empilhar(movimentoObtido);
                        cidadeAtual = i;

                        if (cidadeAtual == destino) // Um caminho foi encontrado
                            AchouCaminho();
                        else
                            i = 0; // Reset de índice
                    }

                    if (!vaiVoltar)
                        i++;
                }

                if (pilha.IsVazia()) // Se a pilha está vazia
                    return caminhos;
                else
                {
                    var movimentoAnterior = pilha.Desempilhar(); // Não achou saída. Volta.
                    cidadeAtual = movimentoAnterior.Origem;
                    i = movimentoAnterior.OndeParou + 1;
                }
            }


            void AchouCaminho() // Ao encontrar um caminho...
            {
                PilhaLista<Movimento> pilhaClone = (PilhaLista<Movimento>)pilha.Clone();
                caminhos.Empilhar(pilhaClone);
                var movimentoAnterior = pilha.Desempilhar();
                cidadeAtual = movimentoAnterior.Origem;

                i = movimentoAnterior.OndeParou + 1; // Começa a procurar a partir da próxima cidade
            }
        }
    }
}
