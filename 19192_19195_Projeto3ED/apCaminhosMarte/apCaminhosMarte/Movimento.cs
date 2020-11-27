using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    // Nome: Pedro Go Iqueda RA: 19195
    // Nome: Nícolas Maisonnette Duarte RA: 19192
    class Movimento: ICloneable
    {
        // Atributo que representa a cidade de origem
        private int origem;
        // Atributo que representa a cidade de destino
        private int destino;
        // Atributo que representa os dados do percurso
        private LigacaoCidade lc;
        // Atributo que representa o índice da cidade onde o movimento parou
        private int ondeParou;

        // Construtor da classe
        public Movimento (int origem, int destino, LigacaoCidade lc)
        {
            Origem = origem;
            Destino = destino;
            Lc = (LigacaoCidade)lc.Clone();
        }

        public Movimento(int origem, int destino, LigacaoCidade lc, int ondeParou)
        {
            Origem = origem;
            Destino = destino;
            Lc = (LigacaoCidade) lc.Clone();
            OndeParou = ondeParou;
        }

        // Construtor de cópia
        public Movimento(Movimento mov)
        {
            Origem = mov.origem;
            Destino = mov.destino;
            Lc = (LigacaoCidade)mov.lc.Clone();
            OndeParou = mov.ondeParou;
        }

        // Propriedade do atributo origem
        public int Origem
        {
            get => origem;
            set
            {
                if (value < 0)
                    throw new Exception("Origem invalida");

                origem = value;
            }
        }

        // Propriedade do atributo destino
        public int Destino
        {
            get => destino;
            set
            {
                if (value < 0)
                    throw new Exception("Destino invalido");

                destino = value;
            }
        }

        // Propriedade do atributo lc
        public LigacaoCidade Lc
        {
            get => lc;
            set
            {
                lc = value;
            }
        }

        public int OndeParou 
        { 
            get => ondeParou; 
            set => ondeParou = value; 
        }

        // Método de comparação entre um objeto da classe e outro objeto
        public override bool Equals (Object obj)
        {
            if (obj == null)
                return false;

            if (this == obj)
                return true;

            if (!GetType().Equals(obj.GetType()))
                return false;

            Movimento mov = (Movimento)obj;

            if (origem != mov.origem)
                return false;

            if (destino != mov.destino)
                return false;

            if (!lc.Equals(mov.lc))
                return false;

            if (!ondeParou.Equals(mov.ondeParou))
                return false;

            return true;
        }

        // Método de comparação que verifica qual objeto é maior (objeto da classe ou objeto do parâmetro)
        public int CompareTo (Movimento m)
        {
            return origem.CompareTo(m.origem);
        }

        // Método que retorna o valor do objeto da classe em formato string
        public override string ToString()
        {
            return $"| O: {origem}| D: {destino} | Dados: {lc} | Índice: {ondeParou}";
        }

        // Método Clone para que possamos clonar uma instância da classe
        public object Clone()
        {
            Movimento ret = null;
            try
            {
                ret = new Movimento(this);
            }
            catch (Exception)
            {}

            return ret;
        }
    }
}
