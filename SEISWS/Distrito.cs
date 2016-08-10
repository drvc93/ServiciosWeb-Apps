using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEISWS
{
    public class Distrito
    {
        public string Codigo { get; set; }
        public string Descrip { get; set; }


        public Distrito() {

            Codigo = "";
            Descrip = "";
        
        
        }


        public Distrito(string cod, string ubig)
        {
            this.Codigo = cod;
            this.Descrip=ubig;

        
        }


    }
}