using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEISWS
{
    public class GradoInstruccion
    {
        private string var_codInstruccion;
        private string var_DescInstruccuion;

        public GradoInstruccion()
        {
             

        }

        public GradoInstruccion(string codInstruccion , string DescInstruccion)
        {
            this.var_codInstruccion = codInstruccion;
            this.var_DescInstruccuion = DescInstruccion;

        }

       

        public string VarCodInstruccion
        {
            get
            {
                return this.var_codInstruccion;
            }
            set
            {
                this.var_codInstruccion = value;
            }
        }

        public string VarDescInstruccuion
        {
            get
            {
                return this.var_DescInstruccuion;
            }
            set
            {
                this.var_DescInstruccuion = value;
            }
        }
    }
}