using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEISWS
{
    public class ExamenSA
    {

        #region variables
     

        private string var_codigoSA;
        private string var_codigoExamen;
        private string var_FlagP;
        private string var_FlagV;
        private string var_FlagI;
        private string var_NomExamen;
        private string var_FlagR;

    
        #endregion



        public ExamenSA() { }

        public ExamenSA(string Codigo,string NomExa) {
            this.var_codigoExamen = Codigo;
            this.var_NomExamen = NomExa;
        
        
        }

        public ExamenSA(string CodSA , string CodExamen,string Flagp,string Flagv,string Flagi,string NomExam,string Flagr) {
 
            this.var_codigoSA=CodSA;
            this.var_codigoExamen=CodExamen;
            this.var_FlagP = Flagp;
            this.var_FlagV = Flagv;
            this.var_FlagI = Flagi;
            this.var_NomExamen = NomExam;
            this.var_FlagR = Flagr;
        
        }

        public string NomExamen
        {
            get { return var_NomExamen; }
            set { var_NomExamen = value; }
        }

        public string FlagI
        {
            get { return var_FlagI; }
            set { var_FlagI = value; }
        }
  

  
        public string CodigoSA
        {
            get { return var_codigoSA; }
            set { var_codigoSA = value; }
        }

        public string CodigoExamen
        {
            get { return var_codigoExamen; }
            set { var_codigoExamen = value; }
        }

        public string FlagP
        {
            get { return var_FlagP; }
            set { var_FlagP = value; }
        }


        public string FlagV
        {
            get { return var_FlagV; }
            set { var_FlagV = value; }
        }

        public string FlagR
        {
            get { return var_FlagR; }
            set { var_FlagR = value; }
        }


    }
}