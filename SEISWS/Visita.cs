using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEISWS
{
    public class Visita
    {
        public int CodigoProyecto { get; set; }
        public int CodigoGrupoVisita { get; set; }
        public string NombreGrupoVisita { get; set; }
        public int CodigoVisita { get; set; }
        public string DescripcionVisita { get; set; }
        public Boolean GenerarAuto { get; set; }
        public int Dependiente { get; set; }
        public int DiasVisitaProx { get; set; }
        public int DiasAntes { get; set; }
        public int DiasDespues { get; set; }
        public int OrdenVisita { get; set; }


        public Visita()
        {
            this.CodigoProyecto = 0;
            this.CodigoGrupoVisita = 0;
            this.NombreGrupoVisita = "";
            this.CodigoVisita = 0;
            this.DescripcionVisita = "";
            this.GenerarAuto = false;
            this.Dependiente = 0;
            this.DiasVisitaProx = 0;
            this.DiasAntes = 0;
            this.DiasDespues = 0;
            this.OrdenVisita = 0;
        }

        public Visita(int CodigoProyecto,
            int CodigoGrupoVisita,
            string NombreGrupoVisita,
            int CodigoVisita,
            string DescripcionVisita,
            Boolean GenerarAuto,
            int Dependiente
            )
        {
            this.CodigoProyecto = CodigoProyecto;
            this.CodigoGrupoVisita = CodigoGrupoVisita;
            this.NombreGrupoVisita = NombreGrupoVisita;
            this.CodigoVisita = CodigoVisita ;
            this.DescripcionVisita = DescripcionVisita;
            this.GenerarAuto = GenerarAuto ;
            this.Dependiente = Dependiente;
            this.DiasVisitaProx = 0;
            this.DiasAntes = 0;
            this.DiasDespues = 0;
            this.OrdenVisita = 0;

        }

        public Visita(int CodigoProyecto,
            int CodigoGrupoVisita,
            string NombreGrupoVisita,
            int CodigoVisita,
            string DescripcionVisita,
            Boolean GenerarAuto,
            int Dependiente,
            int DiasVisitaProx,
            int DiasAntes,
            int DiasDespues,
            int OrdenVisita
            )
        {
            this.CodigoProyecto = CodigoProyecto;
            this.CodigoGrupoVisita = CodigoGrupoVisita;
            this.NombreGrupoVisita = NombreGrupoVisita;
            this.CodigoVisita = CodigoVisita ;
            this.DescripcionVisita = DescripcionVisita;
            this.GenerarAuto = GenerarAuto ;
            this.Dependiente = Dependiente;
            this.DiasVisitaProx = DiasVisitaProx;
            this.DiasAntes = DiasAntes;
            this.DiasDespues = DiasDespues;
            this.OrdenVisita = OrdenVisita;

        }
    }
}