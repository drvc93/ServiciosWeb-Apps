using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEISWS
{
    public class GrupoVisita
    {
        private string var_CodGrupo;
        private string var_DescrGrupo;


        public GrupoVisita() { }

        public GrupoVisita(string Cod, string Descripcion) {
            this.var_CodGrupo = Cod;
            this.var_DescrGrupo = Descripcion;
        
        
        
         }


        public string CodGrupo
        {
            get { return var_CodGrupo; }
            set { var_CodGrupo = value; }
        }

        public string DescrGrupo
        {
            get { return var_DescrGrupo; }
            set { var_DescrGrupo = value; }
        }




    }
}