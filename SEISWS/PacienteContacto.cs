using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEISWS
{
    public class PacienteContacto
    {
        private string var_Depart;
        private string var_Prov;
        private string var_Dist;
        private string var_Domicilo;
        private string var_Referencia;
        private string var_Telefono;
        private string var_Celular;
        private string var_geoLongitud;
        private string var_geoLatitud;
        private string var_DepNacimiento;
        private string var_ProvNacimiento;
        private string var_DistNacinmiento;
        private string var_GradoInst;
        private string var_EstadoCivil;
        private string var_Correo;
       
        public PacienteContacto()
        {
        }

        public PacienteContacto(string depart, string prov , string dist , string domicilio , string referencia , string telefono , string cel , string geoLongitd, string geoLatitud,string depNac,
            string provNac , string distNac, string gradoIns, string estadoCivil , string correo) {

                this.var_Depart = depart;
                this.var_Prov = prov;
                this.var_Dist = dist;
                this.var_Domicilo = domicilio;
                this.var_Referencia = referencia;
                this.var_Telefono = telefono;
                this.var_Celular = cel;
                this.var_geoLongitud = geoLongitd;
                this.var_geoLatitud = geoLatitud;
                this.var_DepNacimiento = depNac;
                this.var_ProvNacimiento = provNac;
                this.var_DistNacinmiento = distNac;
                this.var_GradoInst = gradoIns;
                this.var_EstadoCivil = estadoCivil;
                this.var_Correo = correo;


        
        }
        
        public string VarDepart
        {
            get
            {
                return this.var_Depart;
            }
            set
            {
                this.var_Depart = value;
            }
        }

        public string VarProv
        {
            get
            {
                return this.var_Prov;
            }
            set
            {
                this.var_Prov = value;
            }
        }

        public string VarDist
        {
            get
            {
                return this.var_Dist;
            }
            set
            {
                this.var_Dist = value;
            }
        }

        public string VarDomicilo
        {
            get
            {
                return this.var_Domicilo;
            }
            set
            {
                this.var_Domicilo = value;
            }
        }

        public string VarReferencia
        {
            get
            {
                return this.var_Referencia;
            }
            set
            {
                this.var_Referencia = value;
            }
        }

        public string VarTelefono
        {
            get
            {
                return this.var_Telefono;
            }
            set
            {
                this.var_Telefono = value;
            }
        }

        public string VarCelular
        {
            get
            {
                return this.var_Celular;
            }
            set
            {
                this.var_Celular = value;
            }
        }

        public string VarGeoLongitud
        {
            get
            {
                return this.var_geoLongitud;
            }
            set
            {
                this.var_geoLongitud = value;
            }
        }

        public string VarGeoLatitud
        {
            get
            {
                return this.var_geoLatitud;
            }
            set
            {
                this.var_geoLatitud = value;
            }
        }

        public string VarDepNacimiento
        {
            get
            {
                return this.var_DepNacimiento;
            }
            set
            {
                this.var_DepNacimiento = value;
            }
        }

        public string VarProvNacimiento
        {
            get
            {
                return this.var_ProvNacimiento;
            }
            set
            {
                this.var_ProvNacimiento = value;
            }
        }

        public string VarDistNacinmiento
        {
            get
            {
                return this.var_DistNacinmiento;
            }
            set
            {
                this.var_DistNacinmiento = value;
            }
        }

        public string VarGradoInst
        {
            get
            {
                return this.var_GradoInst;
            }
            set
            {
                this.var_GradoInst = value;
            }
        }

        public string VarEstadoCivil
        {
            get
            {
                return this.var_EstadoCivil;
            }
            set
            {
                this.var_EstadoCivil = value;
            }
        }

        public string VarCorreo
        {
            get
            {
                return this.var_Correo;
            }
            set
            {
                this.var_Correo = value;
            }
        }
    }
}