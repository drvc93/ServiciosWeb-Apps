using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEISWS
{
    public class Area

    {

        private string var_numero;
        private string var_codArea;
        private string var_nombre;

        public Area() { }


        public Area(string num, string cod, string nom) {

            this.var_numero = num;
            this.var_codArea = cod;
            this.var_nombre = nom;
        
        
        } 



        public string Numero
        {
            get { return var_numero; }
            set { var_numero = value; }
        }

    

        public string CodArea
        {
            get { return var_codArea; }
            set { var_codArea = value; }
        }



        public string Nombre
        {
            get { return var_nombre; }
            set { var_nombre = value; }
        }

        

     
        


    }
}