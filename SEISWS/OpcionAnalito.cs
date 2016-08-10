using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEISWS
{
    public class OpcionAnalito
    {

        private string var_CodOpcion;
        private string var_Glosa;

        public OpcionAnalito() { }

        public OpcionAnalito(string cod, string glosa) {

            this.var_CodOpcion = cod;
            this.var_Glosa = glosa;
        
        }


        public string CodOpcion
        {
            get { return var_CodOpcion; }
            set { var_CodOpcion = value; }
        }

        public string Glosa
        {
            get { return var_Glosa; }
            set { var_Glosa = value; }
        }

        
    }
}