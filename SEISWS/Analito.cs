using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEISWS
{
    public class Analito
    {

        private string var_CodAnalito;
        private string var_Nombre;
        private string var_tipo;
        private string var_resultado;

     


        public Analito() { }

        public Analito(string cod, string nombre, string tipo,string resultado) {


            this.var_CodAnalito = cod;
            this.var_Nombre = nombre;
            this.var_tipo = tipo;
            this.var_resultado = resultado;
        
        }


        public string CodAnalito
        {
            get { return var_CodAnalito; }
            set { var_CodAnalito = value; }
        }
        

        public string Nombre
        {
            get { return var_Nombre; }
            set { var_Nombre = value; }
        }

        public string Tipo
        {
            get { return var_tipo; }
            set { var_tipo = value; }
        }

        public string Resultado
        {
            get { return var_resultado; }
            set { var_resultado = value; }
        } 

        
    }
}