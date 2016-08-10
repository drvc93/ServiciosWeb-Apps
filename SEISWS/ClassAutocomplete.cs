using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEISWS
{
    public class ClassAutocomplete
    {
        private string var_IdMuestra ;
        public ClassAutocomplete()
        {
        }

        public ClassAutocomplete(string idMuestra)
        {
            this.var_IdMuestra = idMuestra;
            /**/
        }

        public string IdMuestra
        {
            get
            {
                return this.var_IdMuestra;
            }
            set
            {
                this.var_IdMuestra = value;
            }
        }
    }
}