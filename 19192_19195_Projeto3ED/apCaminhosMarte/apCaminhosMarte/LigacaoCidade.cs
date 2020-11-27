using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    // Nome: Pedro Go Iqueda RA: 19195
    // Nome: Nícolas Maisonnette Duarte RA: 19192
    class LigacaoCidade : ICloneable
    {
        // Atributo que representa a distância entre duas cidades
        private int distancia;
        // Atributo que representa o tempo de percurso entre duas cidades
        private int tempo;
        // Atributo que representa o custo do percurso entre duas cidades
        private int custo;

        // Construtor da classe
        public LigacaoCidade (int distancia, int tempo, int custo)
        {
            Distancia = distancia;
            Tempo = tempo;
            Custo = custo;
        }

        // Construtor de cópia
        public LigacaoCidade(LigacaoCidade lig)
        {
            Distancia = lig.distancia;
            Tempo = lig.tempo;
            Custo = lig.custo;
        }

        // Propriedade do atributo distancia
        public int Distancia
        {
            get => distancia;
            set
            {
                if (value < 0)
                    throw new Exception ("Distancia invalida");

                distancia = value;
            }
        }

        // Propriedade do atributo tempo
        public int Tempo
        {
            get => tempo;
            set
            {
                if (value < 0)
                    throw new Exception("Tempo invalido");

                tempo = value;
            }
        }

        // Propriedade do atributo custo
        public int Custo
        {
            get => custo;
            set
            {
                if (value < 0)
                    throw new Exception("Custo invalido");

                custo = value;
;            }
        }

        // Método Clone para que possamos clonar uma instância da classe
        public object Clone()
        {
            LigacaoCidade ret = null;
            try
            {
                ret = new LigacaoCidade(this);
            }
            catch (Exception)
            {}

            return ret;
        }

        // Método de comparação entre um objeto da classe e outro objeto
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (this == obj)
                return true;

            if (!GetType().Equals(obj.GetType()))
                return false;

            LigacaoCidade lc = (LigacaoCidade) obj;

            if (distancia != lc.distancia)
                return false;

            if (tempo != lc.Tempo)
                return false;

            if (custo != lc.Custo)
                return false;

            return true;
        }

        // Método que retorna o valor do objeto da classe em formato string
        public override string ToString()
        {
            return " Distancia: " + distancia + " | Tempo: " + tempo + " | Custo: " + custo + " |"; 
        }
    }
}
