using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEISWS
{
    public class Cliente
    {
        private string var_codCliente;
        private string var_NombreCliente;
        private string var_DescpCortaCalle;
        private string var_DescpCalle;
        private string var_NumeroCalle;
        private string var_Urban;
        private string var_Deuda;    
        private string var_Meses;
        private string var_Celular;
        private string var_EstadoSMS;

        public Cliente(string codCliente, string NomCliente , string DescpCortaCalle, string DescpCalle, string NumCalle, string Urban, string Deuda, string Meses , string Celular,string estadoSms) {


            this.var_codCliente = codCliente;
            this.var_NombreCliente = NomCliente;
            this.var_DescpCortaCalle = DescpCortaCalle;
            this.var_DescpCalle = DescpCalle;
            this.var_NumeroCalle = NumCalle;
            this.var_Urban = Urban;
            this.var_Deuda = Deuda;
            this.var_Meses = Meses;
            this.var_Celular = Celular;
            this.var_EstadoSMS = estadoSms;

        
        }

        public Cliente() { }



        public string codCliente
        {
            get { return var_codCliente; }
            set { var_codCliente = value; }
        }
        public string NombreCliente
        {
            get { return var_NombreCliente; }
            set { var_NombreCliente = value; }
        }
        public string DescpCortaCalle
        {
            get { return var_DescpCortaCalle; }
            set { var_DescpCortaCalle = value; }
        }
        public string DescpCalle
        {
            get { return var_DescpCalle; }
            set { var_DescpCalle = value; }
        }
       

        public string NumeroCalle
        {
            get { return var_NumeroCalle; }
            set { var_NumeroCalle = value; }
        }
       



        public string Urban
        {
            get { return var_Urban; }
            set { var_Urban = value; }
        }

        public string Deuda
        {
            get { return var_Deuda; }
            set { var_Deuda = value; }
        }

        public string Meses
        {
            get { return var_Meses; }
            set { var_Meses = value; }
        }
       

        public string Celular
        {
            get { return var_Celular; }
            set { var_Celular = value; }
        }

        public string EstadoSMS
        {
            get
            {
                return var_EstadoSMS;
            }

            set
            {
                var_EstadoSMS = value;
            }
        }
    }
}