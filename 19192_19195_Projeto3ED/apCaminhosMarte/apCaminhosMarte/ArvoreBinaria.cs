﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace apCaminhosMarte
{
    // Pedro Go Iqueda RA: 19195
    // Nome: Nícolas Maisonnette Duarte RA: 19192
    public class ArvoreBinaria<Dado> where Dado : IComparable<Dado>
    {
        // Atributos que representa a raiz
        private NoArvore<Dado> raiz;
        // Atributo que representa o nó atual
        private NoArvore<Dado> atual;
        // Atributo que representa o nó anterior ao nó atual
        private NoArvore<Dado> antecessor;

        // Construtor da classe
        public ArvoreBinaria()
        { }

        // Propriedade do atributo raiz
        public NoArvore<Dado> Raiz
        {
            get => raiz;
            set
            {
                raiz = value;
            }
        }

        // Propriedade do atributo atual
        public NoArvore<Dado> Atual
        {
            get => atual;
            set
            {
                atual = value;
            }
        }

        // Propriedade do atributo antecessor
        public NoArvore<Dado> Antecessor
        {
            get => antecessor;
            set
            {
                antecessor = value;
            }
        }

        // Método auxiliar que verifica a existência de uma determinada informação
        private bool Existe(Dado info)
        {
            if (info == null)
                throw new Exception("Dado invalido");

            NoArvore<Dado> atual = raiz;
            for (; ; )
            {
                if (atual == null)
                    return false;

                if (info.CompareTo(atual.Info) < 0)
                {
                    antecessor = atual;
                    atual = atual.Esq;
                }
                else
                    if (info.CompareTo(atual.Info) == 0)
                    return true;
                else
                {
                    antecessor = atual;
                    atual = atual.Dir;
                }
            }
        }

        // Método auxiliar que compõe numa string a árvore
        private void Percorrer(NoArvore<Dado> atual, ref string ret)
        {
            if (atual != null)
            {
                ret += atual.Info.ToString() + ", ";
                Percorrer(atual.Esq, ref ret);
                Percorrer(atual.Dir, ref ret);
            }
        }

        // Método que retorna o valor do objeto da classe em formato string
        public override string ToString()
        {
            string ret = "";
            Percorrer(raiz, ref ret);
            return ret;
        }

        // Método que desenha a árvore e seus valores em um componente
        public void DesenharArvore(bool primeiraVez, NoArvore<Dado> raiz, int x, int y, double angulo, double incremento, double comprimento, Graphics g)
        {
            int xf, yf;
            if (raiz != null)
            {
                Pen caneta = new Pen(Color.Red);
                xf = (int)Math.Round(x + Math.Cos(angulo) * comprimento);
                yf = (int)Math.Round(y + Math.Sin(angulo) * comprimento);

                if (primeiraVez)
                    yf = 25;

                g.DrawLine(caneta, x, y, xf, yf);
                DesenharArvore(false, raiz.Esq, xf, yf, Math.PI / 2 + incremento,
                incremento * 0.60, comprimento * 0.8, g);
                DesenharArvore(false, raiz.Dir, xf, yf, Math.PI / 2 - incremento,
                incremento * 0.60, comprimento * 0.8, g);
                SolidBrush preenchimento = new SolidBrush(Color.Blue);
                g.FillEllipse(preenchimento, xf - 25, yf - 15, 42, 30);
                g.DrawString(Convert.ToString(raiz.Info.ToString()), new Font("Comic Sans", 10, FontStyle.Bold),
                new SolidBrush(Color.Black), xf - 23, yf - 7);
            }
        }

        // Método que coleta a altura da árvore
        public int getAltura(NoArvore<Dado> no)
        {
            if (no != null)
                return no.Altura;
            else
                return -1;
        }

        // Método que insere de forma balanceada um dado na árvore
        public NoArvore<Dado> InserirBalanceado(Dado item, NoArvore<Dado> noAtual)
        {
            if (noAtual == null)
            {
                raiz = new NoArvore<Dado>(item, null, null, 0);
                noAtual = new NoArvore<Dado>(item);
            }
            else
            {
                if (item.CompareTo(noAtual.Info) < 0)
                {
                    noAtual.Esq = InserirBalanceado(item, noAtual.Esq);
                    if (getAltura(noAtual.Esq) - getAltura(noAtual.Dir) == 2) // getAltura testa nulo
                        if (item.CompareTo(noAtual.Esq.Info) < 0)
                            noAtual = RotacaoSimplesComFilhoEsquerdo(noAtual);
                        else
                            noAtual = RotacaoDuplaComFilhoEsquerdo(noAtual);
                }
                else
                if (item.CompareTo(noAtual.Info) > 0)
                {
                    noAtual.Dir = InserirBalanceado(item, noAtual.Dir);
                    if (getAltura(noAtual.Dir) - getAltura(noAtual.Esq) == 2) // getAltura testa nulo
                        if (item.CompareTo(noAtual.Dir.Info) > 0)
                            noAtual = RotacaoSimplesComFilhoDireito(noAtual);
                        else
                            noAtual = RotacaoDuplaComFilhoDireito(noAtual);
                }
                //else ; - não faz nada, valor duplicado
                noAtual.Altura = Math.Max(getAltura(noAtual.Esq), getAltura(noAtual.Dir)) + 1;
            }
            return noAtual;
        }

        // Faz uma rotação simples com o filho esquerdo
        private NoArvore<Dado> RotacaoSimplesComFilhoEsquerdo(NoArvore<Dado> no)
        {
            NoArvore<Dado> temp = no.Esq;
            no.Esq = temp.Dir;
            temp.Dir = no;
            no.Altura = Math.Max(getAltura(no.Esq), getAltura(no.Dir)) + 1;
            temp.Altura = Math.Max(getAltura(temp.Esq), getAltura(no)) + 1;
            return temp;
        }

        // Faz uma rotação simples com o filho direito
        private NoArvore<Dado> RotacaoSimplesComFilhoDireito(NoArvore<Dado> no)
        {
            NoArvore<Dado> temp = no.Dir;
            no.Dir = temp.Esq;
            temp.Esq = no;
            no.Altura = Math.Max(getAltura(no.Esq), getAltura(no.Dir)) + 1;
            temp.Altura = Math.Max(getAltura(temp.Dir), getAltura(no)) + 1;
            return temp;
        }

        // Faz uma rotação dupla com o filho esquerdo
        private NoArvore<Dado> RotacaoDuplaComFilhoEsquerdo(NoArvore<Dado> no)
        {
            no.Esq = RotacaoSimplesComFilhoDireito(no.Esq);
            return RotacaoSimplesComFilhoEsquerdo(no);
        }

        // Faz uma rotação dupla com o filho direito
        private NoArvore<Dado> RotacaoDuplaComFilhoDireito(NoArvore<Dado> no)
        {
            no.Dir = RotacaoSimplesComFilhoEsquerdo(no.Dir);
            return RotacaoSimplesComFilhoDireito(no);
        }
    }
}
