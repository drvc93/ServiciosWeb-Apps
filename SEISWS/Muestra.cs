using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEISWS
{
    public class Muestra
    {
        private string var_CodMuestra;
        private string var_Descripcion;
        private string var_IsChecked;


        public Muestra() { }

        public Muestra( string cod , string descripion ,string check) {


            this.var_CodMuestra = cod;
            this.var_Descripcion = descripion;
            this.var_IsChecked = check;
         


        
        }



        public string CodMuestra
        {
            get { return var_CodMuestra; }
            set { var_CodMuestra = value; }
        }
       
        public string Descripcion
        {
            get { return var_Descripcion; }
            set { var_Descripcion = value; }
        }
        

        public string IsChecked
        {
            get { return var_IsChecked; }
            set { var_IsChecked = value; }
        }




    }
}