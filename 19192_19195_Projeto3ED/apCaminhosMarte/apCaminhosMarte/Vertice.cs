using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    // Nome: Pedro Go Iqueda RA: 19195
    // Nome: Nícolas Maisonnette Duarte RA: 19192

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
