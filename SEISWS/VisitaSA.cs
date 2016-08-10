using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEISWS
{
    public class VisitaSA
    {

        private string var_codVisita;
        private string var_nombreVis;
        private string var_descripcionVis;
        


        


        public VisitaSA() { }

        public VisitaSA(string codVis, string nombreV, string descrp) {

            this.var_codVisita = codVis;
            this.var_nombreVis = nombreV;
            this.var_descripcionVis = descrp;
            

        
        
        }

        public string CodVisita
        {
            get { return var_codVisita; }
            set { var_codVisita = value; }
        }

      

        public string NombreVis
        {
            get { return var_nombreVis; }
            set { var_nombreVis = value; }
        }

        

        public string DescripcionVis
        {
            get { return var_descripcionVis; }
            set { var_descripcionVis = value; }
        }


        
    }
}