using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEISWS
{
    public class Departamento
    {

        public string Codigo { get; set; }
        public string Descrip { get; set; }


        public Departamento() {

            Codigo = "";
            Descrip = "";
        
        
        }


        public Departamento(string cod, string ubig) {
            this.Codigo = cod;
            this.Descrip=ubig;

        
        }


    }
}