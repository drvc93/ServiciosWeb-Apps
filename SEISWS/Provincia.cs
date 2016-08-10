using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEISWS
{
    public class Provincia
    {

         public string Codigo { get; set; }
        public string Descrip { get; set; }


        public Provincia() {

            Codigo = "";
            Descrip = "";
        
        
        }


        public Provincia(string cod, string ubig)
        {
            this.Codigo = cod;
            this.Descrip=ubig;

        
        }
    }
}