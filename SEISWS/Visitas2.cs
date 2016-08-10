using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEISWS
{
    public class Visitas2
    {
        public string Proyecto { get; set; }
        public string Visita { get; set; }
        public string FechaVisita { get; set; }
        public string HoraCita { get; set; }
        public string EstadoVisita { get; set; }
        public string CodigoProyecto { get; set; }
        public string CodigoGrupoVisita { get; set; }
        public string CodigoVisita { get; set; }
        public string CodigoVisitas { get; set; }
        public string FechaUpdEstado {get; set; }

        public Visitas2()
        {
            this.Proyecto = "";
            this.Visita = "";
            this.FechaVisita = "";
            this.HoraCita = "";
            this.EstadoVisita = "";
            this.CodigoProyecto = "";
            this.CodigoGrupoVisita = "";
            this.CodigoVisita = "";
            this.CodigoVisitas = "";
            this.FechaUpdEstado = "";
        }

        public Visitas2(
            string Proyecto,
            string Visita,
            string FechaVisita,
            string HoraCita,
            string EstadoVisita,
            string CodigoProyecto,
            string CodigoGrupoVisita,
            string CodigoVisita,
            string CodigoVisitas,
            string FechaUpdEstado
            )
        {
            this.Proyecto = Proyecto;
            this.Visita = Visita;
            this.FechaVisita = FechaVisita;
            this.HoraCita = HoraCita;
            this.EstadoVisita = EstadoVisita;
            this.CodigoProyecto = CodigoProyecto;
            this.CodigoGrupoVisita = CodigoGrupoVisita;
            this.CodigoVisita = CodigoVisita;
            this.CodigoVisitas = CodigoVisitas;
            this.FechaUpdEstado = FechaUpdEstado;
        }
    }

}