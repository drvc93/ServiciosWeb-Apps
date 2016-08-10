using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEISWS
{
    public class PacienteSA
    {
        private string var_CodigoPaciente;
        private string var_Nombres;
        private string var_ApellidoP;
        private string var_ApellidoM;
        private string var_DocId;
        private string var_FechaN;
        private string var_Sexo;
        private string var_codVisitas;

        


        public PacienteSA() { }

        public PacienteSA(string codPac , string nombres ,string apellidoP,string apellidoM,string docId,string fechaN,string sexo,string codVisitas) {

            this.var_CodigoPaciente = codPac;
            this.var_Nombres = nombres;
            this.var_ApellidoP = apellidoP;
            this.var_ApellidoM = apellidoM;
            this.var_DocId = docId;
            this.var_FechaN = fechaN;
            this.var_Sexo = sexo;
            this.var_codVisitas = codVisitas;

        
        }

        public string CodigoPaciente
        {
            get { return var_CodigoPaciente; }
            set { var_CodigoPaciente = value; }
        }

        public string Nombres
        {
            get { return var_Nombres; }
            set { var_Nombres = value; }
        }

        public string ApellidoP
        {
            get { return var_ApellidoP; }
            set { var_ApellidoP = value; }
        }


        public string ApellidoM
        {
            get { return var_ApellidoM; }
            set { var_ApellidoM = value; }
        }

        public string DocId
        {
            get { return var_DocId; }
            set { var_DocId = value; }
        }

        public string FechaN
        {
            get { return var_FechaN; }
            set { var_FechaN = value; }
        }


        public string Sexo
        {
            get { return var_Sexo; }
            set { var_Sexo = value; }
        }

        public string CodVisitas
        {
            get { return var_codVisitas; }
            set { var_codVisitas = value; }
        }



    }
}