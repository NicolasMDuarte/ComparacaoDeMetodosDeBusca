using System;
using System.Collections.Generic;
using System.Text;

namespace apCaminhosMarte
{
    // Nome: Pedro Go Iqueda RA: 19195
    // Nome: Nícolas Maisonnette Duarte RA: 19192
    public class NoArvore<Dado> : IComparable<NoArvore<Dado>> where Dado : IComparable<Dado>
    {
        // Atributo que representa a informação do ramo/raiz
        private Dado info;
        // Atributo que representa o ramo descendente esquerdo
        private NoArvore<Dado> esq;
        // Atributo que representa o ramo descendente direito
        private NoArvore<Dado> dir;
        // Atributo que representa a altura daquele nó
        private int altura;
        // Atributo que representa se o nó está marcado para morrer
        private bool estaMarcadoParaMorrer;

        // Construtor da classe
        public NoArvore(Dado info, NoArvore<Dado> esq, NoArvore<Dado> dir, int altura)
        {
            Info = info;
            Esq = esq;
            Dir = dir;
            Altura = altura;
        }

        public NoArvore(Dado info)
        {
            Info = info;
            Esq = null;
            Dir = null;
            Altura = 0;
        }

        // Propriedade do atributo info
        public Dado Info
        {
            get => info;
            set
            {
                info = value;
            }
        }

        // Propriedade do atributo esq
        public NoArvore<Dado> Esq
        {
            get => esq;
            set
            {
                esq = value;
            }
        }

        // Propriedade do atributo dir
        public NoArvore<Dado> Dir
        {
            get => dir;
            set
            {
                dir = value;
            }
        }

        //Propriedade do atributo estaMarcadoParaMorrer
        public bool EstaMarcadoParaMorrer 
        { 
            get => estaMarcadoParaMorrer;
            set => estaMarcadoParaMorrer = value; 
        }

        //Propriedade do atributo altura
        public int Altura 
        { 
            get => altura; 
            set => altura = value; 
        }

        // Método de comparação de informações entre dois nós
        public int CompareTo(NoArvore<Dado> obj)
        {
            return info.CompareTo(obj.info);
        }

        // Método de comparação entre dois nós
        public bool Equals(NoArvore<Dado> obj)
        {
            if (obj == null)
                return false;

            return info.Equals(obj.info);
        }
    }
}
