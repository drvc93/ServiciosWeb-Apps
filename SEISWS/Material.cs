using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEISWS
{
    public class Material
    {  
        private string var_CodMaterial;
        private string var_TipoMuestra;   
        private string var_Presentacion;
        private string var_Reactivo;
        private string var_Volumen;
        private string var_Cantidad;

        



        public Material() { }


        public Material( string codMaterial ,string TipoMu,string Presentacion,string react , string  vol , string cant) {

            this.var_CodMaterial = codMaterial;
            this.var_TipoMuestra = TipoMu;
            this.var_Presentacion = Presentacion;
            this.var_Reactivo = react;
            this.var_Volumen = vol;
            this.var_Cantidad = cant;
        
        }


         
        /// <summary>
        ///  set get 
        /// </summary>
        public string CodMaterial
        {
            get { return var_CodMaterial; }
            set { var_CodMaterial = value; }
        }


        public string TipoMuestra
        {
            get { return var_TipoMuestra; }
            set { var_TipoMuestra = value; }
        }

        public string Presentacion
        {
            get { return var_Presentacion; }
            set { var_Presentacion = value; }
        }


        public string Reactivo
        {
            get { return var_Reactivo; }
            set { var_Reactivo = value; }
        }

        public string Volumen
        {
            get { return var_Volumen; }
            set { var_Volumen = value; }
        }

        public string Cantidad
        {
            get { return var_Cantidad; }
            set { var_Cantidad = value; }
        }



    }
}