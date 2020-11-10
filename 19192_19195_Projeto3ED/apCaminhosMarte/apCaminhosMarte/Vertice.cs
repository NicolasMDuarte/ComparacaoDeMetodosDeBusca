using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    class Vertice
    {
        public String rotulo;
        public bool foiVisitado;
        private bool estaAtivo;
        public Vertice(string nomeDoVertice)
        {
            rotulo = nomeDoVertice;
            foiVisitado = false;
            estaAtivo = true;
        }
    }
}
