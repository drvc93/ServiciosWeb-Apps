using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data;
using SecuGen.FDxSDKPro.Windows;
namespace SEISWS
{



    /// <summary>
    /// Descripción breve de ServicioClientes
    ///// </summary>
    //[WebService(Namespace = "http://demo2.sociosensalud.org.pe/")]
    [WebService(Namespace = "http://drvc93xd-001-site1.htempurl.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio Web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]




    public class WSParticipante : System.Web.Services.WebService
    {
        //hace referencia a la clase conexion, ahi esta la cadena de conexion y nuestros metodos
        Conexion con = new Conexion();
        DataTable dtDatos = new DataTable();
        [WebMethod]
        public String LoginUsuario(String login, String pass, int codLocal)
        {
            string msje = "";
            //int intLocal = 2;
            con.Abrir();
            msje = con.InicioSesion(login, pass, codLocal);
            con.Cerrar();
            return msje;
        }

        [WebMethod]
        public Login[] LoginUsuario1(String login, String pass, int codLocal)
        {
            con.Abrir();
            Login[] log = con.InicioSesion1(login, pass, codLocal);
            con.Cerrar();
            return log;
        }
        [WebMethod]
        public string ExisteParticipante(string DocIdentidad)
        {
            SqlConnection cn = con.conexion();
            string existe = "0";
            cn.Open();
            string sql = "select Nombres,ApellidoPaterno,ApellidoMaterno," +
                    "CodigoTipoDocumento,DocumentoIdentidad,convert(varchar(10),FechaNacimiento,103) FechaNacimiento," +
                    "Sexo from PACIENTE WHERE DocumentoIdentidad = '" + DocIdentidad + "'";

            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                existe = "1";
            }
            cn.Close();
            return existe;
        }


        [WebMethod]
        public Participante[] BuscarParticipante(string DocIdentidad)
        {
            SqlConnection cn = con.conexion();

            cn.Open();
            string sql = "select CONVERT(varchar(100), CodigoPaciente, 103) AS CodigoPaciente,Nombres,ApellidoPaterno,ApellidoMaterno," +
                    "CodigoTipoDocumento,DocumentoIdentidad,convert(varchar(10),FechaNacimiento,103) FechaNacimiento," +
                    "Sexo from PACIENTE WHERE DocumentoIdentidad = '" + DocIdentidad + "'";

            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            List<Participante> lista = new List<Participante>();

            while (reader.Read())
            {
                lista.Add(new Participante(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetInt32(4),
                    reader.GetString(5),
                    reader.GetString(6),
                    reader.GetInt32(7)));
            }

            cn.Close();

            return lista.ToArray();
        }

        [WebMethod]
        public string NuevoParticipanteSimple(string Nombres, string ApellidoP, string ApellidoM, int CodigoTipoDocumento, string DocumentoIdentidad, string FechaNacimiento, int Sexo)
        {
            string msje = "";

            con.Abrir();

            dtDatos = this.RegistrarPacientes(Nombres, ApellidoP, ApellidoM, CodigoTipoDocumento, DocumentoIdentidad, FechaNacimiento, Sexo);

            if (dtDatos.Rows.Count > 0)
            {
                string Resultado = dtDatos.Rows[0]["Mensaje"].ToString();
                if (Resultado == "1")
                {
                    msje = "El participante ya existe..por favor verifique bien los datos ingresados";
                }
                else
                {
                    msje = "Los datos se grabaron correctamente";
                }
            }


            return msje;
        }

        [WebMethod]
        public string NuevoParticipanteObjeto(Participante participante)
        {
            string msje = "";

            dtDatos = this.RegistrarPacientes(participante.Nombres, participante.ApellidoPaterno, participante.ApellidoMaterno, participante.CodigoTipoDocumento, participante.DocumentoIdentidad, participante.FechaNacimiento, participante.Sexo);

            if (dtDatos.Rows.Count > 0)
            {
                string Resultado = dtDatos.Rows[0]["Mensaje"].ToString();

                if (Resultado == "1")
                {
                    msje = "Los datos se grabaron correctamente";
                }
                else
                {
                    msje = "El participante ya existe..por favor verifique bien los datos ingresados";
                }
            }

            return msje;

        }


        [WebMethod]
        public string ObtenerIdPaciente(string nombre, string apellp, string apellm, string fechaNac)
        {
            string valret = "";
            try
            {
                //string valret = "";
                SqlConnection sqlcon = con.conexion();
                SqlCommand sqlcmd = new SqlCommand("SPS_OBTENER_ID_PACIENTE", sqlcon);
                sqlcmd.Connection = sqlcon;
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();

                sqlcmd.Parameters.AddWithValue("@nombre", nombre.ToUpper().Trim());
                sqlcmd.Parameters.AddWithValue("@apellidop", apellp.ToUpper().Trim());
                sqlcmd.Parameters.AddWithValue("@apellidom", apellm.ToUpper().Trim());
                sqlcmd.Parameters.AddWithValue("@fecha", fechaNac);
                // sqlcmd.Parameters.AddWithValue("@resul", "").Direction = ParameterDirection.Output;
                //    resul.Direction = ParameterDirection.Output;
                SqlParameter result = sqlcmd.Parameters.Add("@resul", SqlDbType.VarChar);
                result.Direction = ParameterDirection.Output;
                result.Size = 100;
                sqlcmd.ExecuteNonQuery();

                valret = (string) sqlcmd.Parameters["@resul"].Value;



                sqlcon.Close();

            }

            catch (SqlException ex)
            {
                valret = ex.Message;
            }
            catch (Exception ex)
            {

                valret = ex.Message;
            }

            return valret;
        }


        [WebMethod]
        public Distrito[] ListadoDistritos(string codDistrito)
        {

            SqlConnection sqlcon = con.conexion();
            SqlCommand sqlcmd = new SqlCommand("SPS_UBIGEO_X_PROVINCIA", sqlcon);
            sqlcmd.Connection = sqlcon;
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcon.Open();



            sqlcmd.Parameters.AddWithValue("@Codigoprovincia", codDistrito);
            SqlDataReader reader = sqlcmd.ExecuteReader();
            List<Distrito> lista = new List<Distrito>();
            Distrito dep = new Distrito("0", "Seleccion Distrito");
            lista.Add(dep);

            while (reader.Read())
            {

                lista.Add(new Distrito(reader.GetString(0), reader.GetString(1)));

            }

            sqlcon.Close();

            return lista.ToArray();




        }


        [WebMethod]
        public GradoInstruccion[] ListaEstadoCivil()
        {
            SqlConnection cn = con.conexion();
            cn.Open();
            List<GradoInstruccion> listGrad = new List<GradoInstruccion>();
            GradoInstruccion item;
            SqlDataAdapter dap = new SqlDataAdapter("SPS_PARAMETROS_X_CODIGO", cn);
            DataTable dt = new DataTable();
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.AddWithValue("@Codigo", 25);
            dap.Fill(dt);

            cn.Close();
            //return dt;
            item = new GradoInstruccion("0", "Seleccione Estado");
            listGrad.Add(item);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string codInstruccion = dt.Rows[i]["CodigoParametro"].ToString();
                    string gradescripcion = dt.Rows[i]["Descripcion"].ToString();
                    item = new GradoInstruccion(codInstruccion, gradescripcion);
                    listGrad.Add(item);


                }
            }

            return listGrad.ToArray();
        }

        [WebMethod]
        public GradoInstruccion[] ListaGradoInstrc()
        {
            SqlConnection cn = con.conexion();
            cn.Open();
            List<GradoInstruccion> listGrad = new List<GradoInstruccion>();
            GradoInstruccion item;
            SqlDataAdapter dap = new SqlDataAdapter("SPS_PARAMETROS_X_CODIGO", cn);
            DataTable dt = new DataTable();
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.AddWithValue("@Codigo", 26);
            dap.Fill(dt);

            cn.Close();
            //return dt;
            item = new GradoInstruccion("0", "Seleccione Grado");
            listGrad.Add(item);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string codInstruccion = dt.Rows[i]["CodigoParametro"].ToString();
                    string gradescripcion = dt.Rows[i]["Descripcion"].ToString();
                    item = new GradoInstruccion(codInstruccion, gradescripcion);
                    listGrad.Add(item);


                }
            }

            return listGrad.ToArray();
        }

        [WebMethod]

        public Provincia[] ListadoProvincias(string codProvncia)
        {

            String sub = codProvncia.Substring(0, 5);

            SqlConnection sqlcon = con.conexion();
            SqlCommand sqlcmd = new SqlCommand("SPS_UBIGEO_X_DEPARTAMENTO", sqlcon);
            sqlcmd.Connection = sqlcon;
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcon.Open();



            sqlcmd.Parameters.AddWithValue("@CodigoDepartamento", sub);
            SqlDataReader reader = sqlcmd.ExecuteReader();
            List<Provincia> lista = new List<Provincia>();
            Provincia dep = new Provincia("0", "Seleccion Provincia");
            lista.Add(dep);

            while (reader.Read())
            {

                lista.Add(new Provincia(reader.GetString(0), reader.GetString(1)));

            }

            sqlcon.Close();

            return lista.ToArray();



        }



        [WebMethod]

        public Departamento[] ListadoDepartamentos()
        {

            SqlConnection sqlcon = con.conexion();
            SqlCommand sqlcmd = new SqlCommand("SPS_UBIGEO_X_PAIS", sqlcon);
            sqlcmd.Connection = sqlcon;
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcon.Open();



            sqlcmd.Parameters.AddWithValue("@CodigoPais", 1);
            SqlDataReader reader = sqlcmd.ExecuteReader();
            List<Departamento> lista = new List<Departamento>();
            Departamento dep = new Departamento("0", "Seleccion Departamento");
            lista.Add(dep);

            while (reader.Read())
            {

                lista.Add(new Departamento(reader.GetString(0), reader.GetString(1)));

            }

            sqlcon.Close();

            return lista.ToArray();



        }




        [WebMethod]
        public Local[] ListadoLocales()
        {
            SqlConnection cn = con.conexion();

            cn.Open();

            string sql = "SELECT CodigoLocal, Nombre FROM local";

            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            List<Local> lista = new List<Local>();
            Local loc = new Local();
            loc.CodigoLocal = 0;
            loc.Nombre = "Seleccione Local";
            lista.Add(loc);

            while (reader.Read())
            {
                lista.Add(new Local(reader.GetInt32(0), reader.GetString(1)));
            }

            cn.Close();

            return lista.ToArray();
        }

        [WebMethod]
        public Geofence[] ListadoGeofences()
        {
            SqlConnection cn = con.conexion();

            cn.Open();

            string sql = "SELECT G.CodigoGeofence, G.CodigoLocal, L.Nombre, G.Latitud, G.Longitud, G.Radio, G.DuracionExpiracion, G.TipoTransicion " +
                "FROM GEOFENCE AS G INNER JOIN " +
                "LOCAL AS L ON G.CodigoLocal = L.CodigoLocal ";

            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            List<Geofence> lista = new List<Geofence>();
            Geofence geo = new Geofence();
            geo.codigogeofence = 0;
            geo.codigolocal = 0;
            geo.nombre = "Seleccione Local";
            geo.latitud = "";
            geo.longitud = "";
            geo.radio = "";
            geo.duracionexpiracion = "";
            geo.tipotransicion = 0;

            lista.Add(geo);

            while (reader.Read())
            {
                lista.Add(new Geofence(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6),
                    reader.GetInt32(7)
                    ));
            }

            cn.Close();

            return lista.ToArray();
        }
        public DataTable RegistrarPacientes(string Nombres, string ApellidoP, string ApellidoM, int CodigoTipoDocumento, string DocumentoIdentidad, string FechaNacimiento, int Sexo)
        {
            SqlConnection cn = con.conexion();
            cn.Open();

            SqlDataAdapter dap = new SqlDataAdapter("SPI_PACIENTE", cn);
            DataTable dt = new DataTable();
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.AddWithValue("@Nombres", Nombres);
            dap.SelectCommand.Parameters.AddWithValue("@ApellidoP", ApellidoP);
            dap.SelectCommand.Parameters.AddWithValue("@ApellidoM", ApellidoM);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoTipoDocumento", CodigoTipoDocumento);
            dap.SelectCommand.Parameters.AddWithValue("@DocumentoIdentidad", DocumentoIdentidad);
            dap.SelectCommand.Parameters.AddWithValue("@FechaNacimiento", FechaNacimiento);
            dap.SelectCommand.Parameters.AddWithValue("@Sexo", Sexo);
            dap.Fill(dt);

            cn.Close();
            return dt;

        }

        public DataTable BuscarPacxDocumento(string Documento)
        {
            SqlConnection cn = con.conexion();
            cn.Open();

            SqlDataAdapter dap = new SqlDataAdapter("SPS_PACIENTE_BS", cn);
            DataTable dt = new DataTable();
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.AddWithValue("@Documento", Documento);
            dap.Fill(dt);

            cn.Close();
            return dt;
        }

        [WebMethod]
        public Proyecto[] ListadoProyectos(string local)
        {
            SqlConnection cn = con.conexion();

            cn.Open();

            string sql = "SELECT p.CodigoProyecto,p.Nombre " +
                "FROM LOCAL_PROYECTO AS lp INNER JOIN " +
                "PROYECTO AS p ON lp.CodigoProyecto = p.CodigoProyecto " +
                "WHERE lp.estado=1 AND CodigoLocal = " + local;

            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            List<Proyecto> lista = new List<Proyecto>();

            while (reader.Read())
            {
                lista.Add(new Proyecto(reader.GetInt32(0), reader.GetString(1)));
            }

            cn.Close();

            return lista.ToArray();
        }
        [WebMethod]
        public Proyecto[] ListadoProyectos1(int CodigoLocal, int CodigoUsuario)
        {
            SqlConnection cn = con.conexion();

            cn.Open();
            SqlDataAdapter dap = new SqlDataAdapter("SPS_PROYECTOS_X_LOCAL_X_USUARIO", cn);
            DataTable dt = new DataTable();
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.AddWithValue("@CodigoLocal", CodigoLocal);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoUsuario", CodigoUsuario);
            dap.Fill(dt);

            DataTableReader reader = dt.CreateDataReader();

            List<Proyecto> lista = new List<Proyecto>();

            while (reader.Read())
            {
                lista.Add(new Proyecto(reader.GetInt32(0), reader.GetString(1)));
            }

            cn.Close();

            return lista.ToArray();
        }
        [WebMethod]
        public Visita[] ListadoGrupoVisitas(string CodigoPaciente, int CodigoLocal, int CodigoProyecto)
        {
            SqlConnection cn = con.conexion();

            cn.Open();
            SqlDataAdapter dap = new SqlDataAdapter("SPS_DATOS_SEDE_PROY1", cn);
            DataTable dt = new DataTable();
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.AddWithValue("@CodigoPaciente", CodigoPaciente);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoLocal", CodigoLocal);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoProyecto", CodigoProyecto);
            dap.Fill(dt);

            DataTableReader reader = dt.CreateDataReader();

            List<Visita> lista = new List<Visita>();

            while (reader.Read())
            {
                lista.Add(new Visita(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetString(2),
                    reader.GetInt32(3),
                    reader.GetString(4),
                    reader.GetBoolean(5),
                    reader.GetInt32(6)));
            }

            cn.Close();
            return lista.ToArray();
        }
        [WebMethod]
        public int InsertarVisitas(int CodigoLocal, int CodigoProyecto, int CodigoGrupoVisita, int CodigoVisita, string CodigoPaciente, string FechaVisita, string HoraCita, int CodigoUsuario)
        {
            SqlConnection cn = con.conexion();
            SqlCommand cmd = new SqlCommand("SPI_VISITAS", cn);
            SqlTransaction trx;
            int intretorno;
            string strRespuesta;

            try
            {
                cn.Open();
                trx = cn.BeginTransaction();
                cmd.Transaction = trx;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CodigoLocal", SqlDbType.Int)).Value = CodigoLocal;
                cmd.Parameters.Add(new SqlParameter("@CodigoProyecto", SqlDbType.Int)).Value = CodigoProyecto;
                cmd.Parameters.Add(new SqlParameter("@CodigoGrupoVisita", SqlDbType.Int)).Value = CodigoGrupoVisita;
                cmd.Parameters.Add(new SqlParameter("@CodigoVisita", SqlDbType.Int)).Value = CodigoVisita;
                cmd.Parameters.Add(new SqlParameter("@CodigoPaciente", SqlDbType.VarChar, 50)).Value = CodigoPaciente;
                cmd.Parameters.Add(new SqlParameter("@FechaVisita", SqlDbType.VarChar, 10)).Value = FechaVisita;
                cmd.Parameters.Add(new SqlParameter("@HoraCita", SqlDbType.VarChar, 5)).Value = HoraCita;
                cmd.Parameters.Add(new SqlParameter("@CodigoUsuario", SqlDbType.Int)).Value = CodigoUsuario;
                cmd.Transaction = trx;
                intretorno = cmd.ExecuteNonQuery();
                trx.Commit();
                cn.Close();
                return intretorno;
            }
            catch (SqlException sqlException)
            {
                strRespuesta = sqlException.Message.ToString();
                cn.Close();
                return -1;
            }
            catch (Exception exception)
            {
                strRespuesta = exception.Message.ToString();
                cn.Close();
                return -1;
            }
        }

        [WebMethod]
        public Visitas[] ListadoVisitas(string CodigoPaciente)

        {
            SqlConnection cn = con.conexion();
            cn.Open();
            //            SELECT        PY.Nombre AS Proyecto, E.Nombre AS Visita, SUBSTRING(DATENAME(dw, V.FechaVisita), 1, 3) + ' ' + CONVERT(varchar(10), V.FechaVisita, 103) 
            //                         AS FechaVisita, CONVERT(varchar(5), V.HoraInicio, 108) AS HoraCita, EC.Descripcion AS EstadoVisita
            //            FROM         VISITAS AS V INNER JOIN PROYECTO AS PY ON V.CodigoProyecto = PY.CodigoProyecto AND V.Estado = 1 
            //                         INNER JOIN VISITA AS E ON V.CodigoProyecto = E.CodigoProyecto AND V.CodigoGrupoVisita = E.CodigoGrupoVisita AND V.CodigoVisita = E.CodigoVisita 
            //                         INNER JOIN PARAMETROS AS EC ON V.CodigoEstadoVisita = EC.CodigoParametro AND EC.Codigo = 5
            //            WHERE        (V.CodigoPaciente = 'b0875796-a823-455b-a48a-4da85d050fca') AND (V.CodigoLocal = 1) AND (V.CodigoProyecto = 1)
            string sql = "SELECT PY.Nombre AS Proyecto, E.Nombre AS Visita, " +
                "SUBSTRING(DATENAME(dw, V.FechaVisita), 1, 3) + ' ' + CONVERT(varchar(10), V.FechaVisita, 103) AS FechaVisita," +
                "CONVERT(varchar(5), V.HoraInicio, 108) AS HoraCita, EC.Descripcion AS EstadoVisita " +
                "FROM  VISITAS AS V INNER JOIN PROYECTO AS PY ON V.CodigoProyecto = PY.CodigoProyecto AND V.Estado = 1 " +
                "INNER JOIN VISITA AS E ON V.CodigoProyecto = E.CodigoProyecto AND V.CodigoGrupoVisita = E.CodigoGrupoVisita AND V.CodigoVisita = E.CodigoVisita " +
                "INNER JOIN PARAMETROS AS EC ON V.CodigoEstadoVisita = EC.CodigoParametro AND EC.Codigo = 5 " +
                "WHERE V.CodigoPaciente = '" + CodigoPaciente + "'";

            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            List<Visitas> lista = new List<Visitas>();

            while (reader.Read())
            {
                lista.Add(new Visitas(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4)));
            }

            cn.Close();
            return lista.ToArray();
        }
        [WebMethod]
        public String ListadoFormatos(String CodigoUsuario, int CodigoLocal, int CodigoProyecto, int CodigoGrupoVisita, int CodigoVisita)
        {
            SqlConnection cn = con.conexion();
            //WHERE        (F.IdTipoDeFormato = '04') AND (U.CodigoUsuarioSP = '0') AND (PU.CodigoProyecto = 5) AND (U.CodigoLocal = 1) AND (R.CodigoVisita = 1) AND 
            //             
            cn.Open();
            string sql = "SELECT DISTINCT F.IdFormatoNemotecnico AS FormID " +
                         "FROM SEIS_DATA.dbo.Usuarios AS U INNER JOIN " +
                         "SEIS_DATA.dbo.Proyecto_Usuario AS PU ON U.CodigoUsuario = PU.CodigoUsuario INNER JOIN " +
                         "SEIS_DATA.dbo.RutaServicioFormato AS R ON PU.CodigoProyecto = R.CodigoProyecto INNER JOIN " +
                         "SEIS_DATA.dbo.Formato AS F ON R.IdFormato = F.IdFormato " +
                         "WHERE F.IdTipoDeFormato = '04' AND U.CodigoUsuarioSP = '" + CodigoUsuario + "' AND " +
                            "PU.CodigoProyecto = " + CodigoProyecto + " AND U.CodigoLocal = " + CodigoLocal + " AND " +
                            "R.CodigoVisita = " + CodigoVisita + " AND R.CodigoGrupoVisita = " + CodigoGrupoVisita;
            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            String lstFormatos = "";

            while (reader.Read())
            {
                lstFormatos += reader.GetString(0) + "/";
            }

            cn.Close();

            return lstFormatos;
        }
        [WebMethod]
        public String ListadoFormatos1(String CodigoUsuario, int CodigoLocal, int CodigoProyecto)
        {
            SqlConnection cn = con.conexion();
            //WHERE        (F.IdTipoDeFormato = '04') AND (U.CodigoUsuarioSP = '0') AND (PU.CodigoProyecto = 5) AND (U.CodigoLocal = 1) 
            //             
            cn.Open();
            string sql = "SELECT DISTINCT  F.IdFormatoNemotecnico AS FormID " +
                         "FROM SEIS_DATA.dbo.Usuarios AS U INNER JOIN " +
                         "SEIS_DATA.dbo.Proyecto_Usuario AS PU ON U.CodigoUsuario = PU.CodigoUsuario INNER JOIN " +
                         "SEIS_DATA.dbo.RutaServicioFormato AS R ON PU.CodigoProyecto = R.CodigoProyecto INNER JOIN " +
                         "SEIS_DATA.dbo.Formato AS F ON R.IdFormato = F.IdFormato " +
                         "WHERE F.IdTipoDeFormato = '04' AND U.CodigoUsuarioSP = '" + CodigoUsuario + "' AND " +
                         "PU.CodigoProyecto = " + CodigoProyecto + " AND U.CodigoLocal = " + CodigoLocal;

            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            String lstFormatos = "";

            while (reader.Read())
            {
                lstFormatos += reader.GetString(0) + "/";
            }

            cn.Close();

            return lstFormatos;
        }

        [WebMethod]
        public Visitas1[] ListadoVisitas1(string CodigoPaciente)
        {
            SqlConnection cn = con.conexion();
            cn.Open();
            string sql = "SELECT PY.Nombre AS Proyecto, E.Nombre AS Visita, " +
                 "SUBSTRING(DATENAME(dw, V.FechaVisita), 1, 3) + ' ' + CONVERT(varchar(10), V.FechaVisita, 103) AS FechaVisita," +
                 "CONVERT(varchar(5), V.HoraInicio, 108) AS HoraCita, EC.Descripcion AS EstadoVisita ,CONVERT(varchar(5), V.CodigoProyecto, 103) AS CodigoProyecto," +
                 "CONVERT(varchar(5), V.CodigoGrupoVisita, 103) AS CodigoGrupoVisita,CONVERT(varchar(5), V.CodigoVisita, 103) AS CodigoVisita, CONVERT(varchar(5), V.CodigoVisitas, 103) AS CodigoVisitas " +
                 "FROM  VISITAS AS V INNER JOIN PROYECTO AS PY ON V.CodigoProyecto = PY.CodigoProyecto AND V.Estado = 1 " +
                 "INNER JOIN VISITA AS E ON V.CodigoProyecto = E.CodigoProyecto AND V.CodigoGrupoVisita = E.CodigoGrupoVisita AND V.CodigoVisita = E.CodigoVisita " +
                 "INNER JOIN PARAMETROS AS EC ON V.CodigoEstadoVisita = EC.CodigoParametro AND EC.Codigo = 5 " +
                 "WHERE V.CodigoPaciente = '" + CodigoPaciente + "'";

            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            List<Visitas1> lista = new List<Visitas1>();

            while (reader.Read())
            {
                lista.Add(new Visitas1(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6),
                    reader.GetString(7),
                    reader.GetString(8)));
            }

            cn.Close();
            return lista.ToArray();
        }

        [WebMethod]
        public Visitas1[] ListadoVisitas2(string CodigoPaciente, string CodigoUsuario)
        {
            SqlConnection cn = con.conexion();
            cn.Open();
            string sql = "SELECT PY.Nombre AS Proyecto, E.Nombre AS Visita, " +
                 "SUBSTRING(DATENAME(dw, V.FechaVisita), 1, 3) + ' ' + CONVERT(varchar(10), V.FechaVisita, 103) AS FechaVisita," +
                 "CONVERT(varchar(5), V.HoraInicio, 108) AS HoraCita, EC.Descripcion AS EstadoVisita ,CONVERT(varchar(5), V.CodigoProyecto, 103) AS CodigoProyecto," +
                 "CONVERT(varchar(5), V.CodigoGrupoVisita, 103) AS CodigoGrupoVisita,CONVERT(varchar(5), V.CodigoVisita, 103) AS CodigoVisita, CONVERT(varchar(5), V.CodigoVisitas, 103) AS CodigoVisitas " +
                 "FROM  VISITAS AS V INNER JOIN PROYECTO AS PY ON V.CodigoProyecto = PY.CodigoProyecto AND V.Estado = 1 " +
                 "INNER JOIN USUARIOS_PROYECTO AS UP ON UP.CodigoProyecto = V.CodigoProyecto " +
                 "INNER JOIN VISITA AS E ON V.CodigoProyecto = E.CodigoProyecto AND V.CodigoGrupoVisita = E.CodigoGrupoVisita AND V.CodigoVisita = E.CodigoVisita " +
                 "INNER JOIN PARAMETROS AS EC ON V.CodigoEstadoVisita = EC.CodigoParametro AND EC.Codigo = 5 " +
                 "WHERE V.CodigoPaciente = '" + CodigoPaciente + "' AND UP.CodigoUsuario = " + CodigoUsuario;

            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            List<Visitas1> lista = new List<Visitas1>();

            while (reader.Read())
            {
                lista.Add(new Visitas1(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6),
                    reader.GetString(7),
                    reader.GetString(8)));
            }

            cn.Close();
            return lista.ToArray();
        }

        [WebMethod]
        public Visitas1[] ListadoVisitas3(string CodigoPaciente, string CodigoUsuario, string CodigoProyecto)
        {
            SqlConnection cn = con.conexion();
            cn.Open();
            string sql = "SELECT PY.Nombre AS Proyecto, E.Nombre AS Visita, " +
                 "SUBSTRING(DATENAME(dw, V.FechaVisita), 1, 3) + ' ' + CONVERT(varchar(10), V.FechaVisita, 103) AS FechaVisita," +
                 "CONVERT(varchar(5), V.HoraInicio, 108) AS HoraCita, EC.Descripcion AS EstadoVisita ,CONVERT(varchar(5), V.CodigoProyecto, 103) AS CodigoProyecto," +
                 "CONVERT(varchar(5), V.CodigoGrupoVisita, 103) AS CodigoGrupoVisita,CONVERT(varchar(5), V.CodigoVisita, 103) AS CodigoVisita, CONVERT(varchar(5), V.CodigoVisitas, 103) AS CodigoVisitas " +
                 "FROM  VISITAS AS V INNER JOIN PROYECTO AS PY ON V.CodigoProyecto = PY.CodigoProyecto AND V.Estado = 1 " +
                 "INNER JOIN USUARIOS_PROYECTO AS UP ON UP.CodigoProyecto = V.CodigoProyecto " +
                 "INNER JOIN VISITA AS E ON V.CodigoProyecto = E.CodigoProyecto AND V.CodigoGrupoVisita = E.CodigoGrupoVisita AND V.CodigoVisita = E.CodigoVisita " +
                 "INNER JOIN PARAMETROS AS EC ON V.CodigoEstadoVisita = EC.CodigoParametro AND EC.Codigo = 5 " +
                 "WHERE V.CodigoPaciente = '" + CodigoPaciente + "' AND UP.CodigoUsuario = " + CodigoUsuario + " AND V.CodigoProyecto = " + CodigoProyecto;

            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            List<Visitas1> lista = new List<Visitas1>();

            while (reader.Read())
            {
                lista.Add(new Visitas1(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6),
                    reader.GetString(7),
                    reader.GetString(8)));
            }

            cn.Close();
            return lista.ToArray();
        }
        [WebMethod]
        public Visitas1[] ListadoVisitas4(string CodigoPaciente, string CodigoUsuario, string CodigoLocal)
        {
            SqlConnection cn = con.conexion();
            cn.Open();
            string sql = "SELECT DISTINCT PY.Nombre AS Proyecto, E.Nombre AS Visita, " +
                 "SUBSTRING(DATENAME(dw, V.FechaVisita), 1, 3) + ' ' + CONVERT(varchar(10), V.FechaVisita, 103) AS FechaVisita," +
                 "CONVERT(varchar(5), V.HoraInicio, 108) AS HoraCita, EC.Descripcion AS EstadoVisita ,CONVERT(varchar(5), V.CodigoProyecto, 103) AS CodigoProyecto," +
                 "CONVERT(varchar(5), V.CodigoGrupoVisita, 103) AS CodigoGrupoVisita,CONVERT(varchar(5), V.CodigoVisita, 103) AS CodigoVisita, CONVERT(varchar(5), V.CodigoVisitas, 103) AS CodigoVisitas " +
                 "FROM  VISITAS AS V INNER JOIN PROYECTO AS PY ON V.CodigoProyecto = PY.CodigoProyecto AND V.Estado = 1 " +
                 "INNER JOIN USUARIOS_PROYECTO AS UP ON UP.CodigoProyecto = V.CodigoProyecto " +
                 "INNER JOIN VISITA AS E ON V.CodigoProyecto = E.CodigoProyecto AND V.CodigoGrupoVisita = E.CodigoGrupoVisita AND V.CodigoVisita = E.CodigoVisita " +
                 "INNER JOIN PARAMETROS AS EC ON V.CodigoEstadoVisita = EC.CodigoParametro AND EC.Codigo = 5 " +
                 "WHERE V.CodigoPaciente = '" + CodigoPaciente + "' AND UP.CodigoUsuario = " + CodigoUsuario + "AND V.CodigoProyecto IN " +
                 "(SELECT LP.CodigoProyecto FROM LOCAL_PROYECTO AS LP WHERE LP.Estado = 1 AND LP.CodigoLocal = " + CodigoLocal + ")";
            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            List<Visitas1> lista = new List<Visitas1>();

            while (reader.Read())
            {
                lista.Add(new Visitas1(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6),
                    reader.GetString(7),
                    reader.GetString(8)));
            }

            cn.Close();
            return lista.ToArray();
        }

        [WebMethod]
        public string InsertarPacienteContacto(string codigoPaciente, string codigoUbigeo, string direccion, string referencia, string telefono, string Celular, string Longit, string latit, string UbigeoNac, string codGradInstruccion, string codEstadoCiv, string correo, string accion, string dni)
        {

            string val = "";

            SqlConnection sqlcon = con.conexion();
            SqlCommand sqlcmd = new SqlCommand("SPI_PACIENTE_CONTACTO_M", sqlcon);
            sqlcmd.Connection = sqlcon;
            sqlcmd.CommandType = CommandType.StoredProcedure;

            try
            {

                sqlcon.Open();
                sqlcmd.Parameters.AddWithValue("@CodigoPaciente", codigoPaciente);
                // sqlcmd.Parameters.AddWithValue("@CodigoUbigeo", codigoUbigeo);
                sqlcmd.Parameters.AddWithValue("@CodigoUbigeo", codigoUbigeo);
                sqlcmd.Parameters.AddWithValue("@Direccion", direccion);
                sqlcmd.Parameters.AddWithValue("@Referencia", referencia);
                sqlcmd.Parameters.AddWithValue("@Telefono1", telefono);
                sqlcmd.Parameters.AddWithValue("@Celular", Celular);
                sqlcmd.Parameters.AddWithValue("@GeoLong", Longit);
                sqlcmd.Parameters.AddWithValue("@GeoLati", latit);
                sqlcmd.Parameters.AddWithValue("@ubigeoNac", UbigeoNac);
                sqlcmd.Parameters.AddWithValue("@codGradIns", Convert.ToInt32(codGradInstruccion));
                sqlcmd.Parameters.AddWithValue("@codEstadoCv", Convert.ToInt32(codEstadoCiv));
                sqlcmd.Parameters.AddWithValue("@correo", correo);
                sqlcmd.Parameters.AddWithValue("@accion", accion);
                sqlcmd.Parameters.AddWithValue("@dni", dni);
                //@ubigeoNac

                if (sqlcmd.ExecuteNonQuery() == 1)
                {
                    val = "OK";

                }




            }
            catch (SqlException ex)
            {

                val = ex.Message;

            }

            finally
            {

                sqlcon.Close();

            }


            return val;

        }


        public void VisitasAutomatica(int codLocal, int codProyecto, string codPaciente, int codUser)
        {/**/


            SqlConnection sqlcon = con.conexion();
            SqlCommand sqlcmd = new SqlCommand("SPI_VISITAS_AUTO", sqlcon);
            sqlcmd.Connection = sqlcon;
            sqlcmd.CommandType = CommandType.StoredProcedure;

            try
            {

                sqlcon.Open();
                sqlcmd.Parameters.AddWithValue("@CodigoLocal", codLocal);
                // sqlcmd.Parameters.AddWithValue("@CodigoUbigeo", codigoUbigeo);
                sqlcmd.Parameters.AddWithValue("@CodigoProyecto", codProyecto);
                sqlcmd.Parameters.AddWithValue("@CodigoPaciente", codPaciente);
                sqlcmd.Parameters.AddWithValue("@CodigoUsuario", codUser);

                sqlcmd.ExecuteNonQuery();




            }
            catch (SqlException ex)
            {



            }

            finally
            {

                sqlcon.Close();

            }

        }

        public void VerificarVistaAutomatica(int codLocal, int codProyecto, string codPaciente, int codUser, int codVisitas)
        {
            SqlConnection cn = con.conexion();
            SqlDataAdapter dap = new SqlDataAdapter("SPS_VERFICAR_VISITA_AUTOMATICA", cn);
            DataTable dt = new DataTable();
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.AddWithValue("@codProyecto", codProyecto);
            dap.SelectCommand.Parameters.AddWithValue("@codVisitas", codVisitas);
            dap.SelectCommand.Parameters.AddWithValue("@codLocal", codLocal);
            dap.SelectCommand.Parameters.AddWithValue("@codPaciente", codPaciente);
            dap.Fill(dt);
            if (dt.Rows.Count > 0 && dt != null)

            {
                int var_auto = Convert.ToInt32(dt.Rows[0]["Auto"].ToString());

                if (var_auto == 1)

                {

                    this.VisitasAutomatica(codLocal, codProyecto, codPaciente, codUser);

                }

            }



        }

        [WebMethod]
        public int EstadoVisita(int CodigoLocal, int CodigoProyecto, int CodigoVisita, int CodigoVisitas,
            string CodigoPaciente, int CodigoEstadoVisita, int CodigoEstatusPaciente, int CodigoUsuario)
        {
            SqlConnection cn = con.conexion();
            SqlCommand cmd = new SqlCommand("SPU_ESTADO_VISITA", cn);
            SqlTransaction trx;
            int intretorno;
            string strRespuesta;



            try
            {
                cn.Open();
                trx = cn.BeginTransaction();
                cmd.Transaction = trx;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CodigoLocal", SqlDbType.Int)).Value = CodigoLocal;
                cmd.Parameters.Add(new SqlParameter("@CodigoProyecto", SqlDbType.Int)).Value = CodigoProyecto;
                cmd.Parameters.Add(new SqlParameter("@CodigoVisita", SqlDbType.Int)).Value = CodigoVisita;
                cmd.Parameters.Add(new SqlParameter("@CodigoVisitas", SqlDbType.Int)).Value = CodigoVisitas;
                cmd.Parameters.Add(new SqlParameter("@CodigoPaciente", SqlDbType.VarChar, 50)).Value = CodigoPaciente;
                cmd.Parameters.Add(new SqlParameter("@CodigoEstadoVisita", SqlDbType.Int)).Value = CodigoEstadoVisita;
                cmd.Parameters.Add(new SqlParameter("@CodigoEstatusPaciente", SqlDbType.Int)).Value = CodigoEstatusPaciente;
                cmd.Parameters.Add(new SqlParameter("@CodigoUsuario", SqlDbType.Int)).Value = CodigoUsuario;
                cmd.Transaction = trx;
                intretorno = cmd.ExecuteNonQuery();
                trx.Commit();
                cn.Close();
                this.VerificarVistaAutomatica(CodigoLocal, CodigoProyecto, CodigoPaciente, CodigoUsuario, CodigoVisitas);
                return intretorno;
            }
            catch (SqlException sqlException)
            {
                strRespuesta = sqlException.Message.ToString();
                cn.Close();
                return -1;
            }
            catch (Exception exception)
            {
                strRespuesta = exception.Message.ToString();
                cn.Close();
                return -1;
            }
        }

        [WebMethod]
        public lstId[] ListadoIds(string CodigoPaciente, string CodigoUsuario)
        {
            SqlConnection cn = con.conexion();
            cn.Open();
            string sql = "SELECT CONVERT(varchar(5), U.CodigoProyecto, 103) AS CodigoProyecto,PY.Nombre AS Proyecto, " +
                 "CASE WHEN ct.Numero IS NULL THEN '' ELSE ct.Numero END AS IdTAM, " +
                 "CASE WHEN ce.Numero IS NULL THEN '' ELSE ce.Numero END AS IdENR " +
                 "FROM dbo.USUARIOS_PROYECTO U " +
                 "INNER JOIN PROYECTO AS PY ON U.CodigoProyecto = PY.CodigoProyecto AND U.Estado=1 " +
                 "INNER JOIN PACIENTE_LOCAL_PROYECTO PP ON PY.CodigoProyecto = PP.CodigoProyecto " +
                 "LEFT JOIN PACIENTE_COD_TAM ct on ct.CodigoTAM=PP.CodigoTAM AND ct.CodigoLocal=PP.CodigoLocal AND ct.CodigoProyecto=PP.CodigoProyecto " +
                 "LEFT JOIN PACIENTE_COD_ENR ce on ce.CodigoENR=PP.CodigoENR AND ce.CodigoLocal=PP.CodigoLocal AND ce.CodigoProyecto=PP.CodigoProyecto " +
                 "WHERE PP.CodigoPaciente = '" + CodigoPaciente + "' AND U.CodigoUsuario = " + CodigoUsuario;

            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            List<lstId> lista = new List<lstId>();

            while (reader.Read())
            {
                lista.Add(new lstId(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3)));
            }

            cn.Close();
            return lista.ToArray();
        }



        [WebMethod]
        public string ActulizarEstadoSmsClientes (string accion){
            string result = "No se pudo actulizar";
            SqlConnection sqlcon = con.conexion();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;

            try
            {
                sqlcon.Open();
                sqlcmd.CommandText = "SPS_ACCION";
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@Accion", accion);

                int num = sqlcmd.ExecuteNonQuery();

                if ( num > 0)
                {

                    result = Convert.ToString(num);

                }

            }

            catch (SqlException sqlEx)
            {


                result = sqlEx.Message;

            }

            catch (Exception ex)
            {

                result = ex.Message;


            }

            finally
            {
                sqlcon.Close();

            }

            return result;


        }


        [WebMethod]
        public string ActulizarEstadoCliente (string codCliente)
        {
            string result = "No se pudo actulizar";
            SqlConnection sqlcon = con.conexion();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;

            try
            {
                sqlcon.Open();
                sqlcmd.CommandText = "SPU_ACTUALIZAR_ESTADO_CLIENTE";
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@CodCliente", codCliente);
               

                if (sqlcmd.ExecuteNonQuery() == 1)
                {

                    result = "OK";

                }

            }

            catch (SqlException sqlEx)
            {


                result = sqlEx.Message;

            }

            catch (Exception ex)
            {

                result = ex.Message;


            }

            finally
            {
                sqlcon.Close();

            }

            return result;

        }


        [WebMethod]
        public Cliente[] ListaClientes(string codAccion)
        {

            List<Cliente> listCliente = new List<Cliente>();
            SqlConnection cn = con.conexion();
            SqlDataAdapter dap = new SqlDataAdapter("SPS_LISTAR_CLIENTES", cn);
            DataTable dt = new DataTable();
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.AddWithValue("@accion", codAccion);

            dap.Fill(dt);

            if (dt != null && dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string cod = dt.Rows[i]["CodCliente"].ToString();
                    string nomCli = dt.Rows[i]["NomCliente"].ToString();
                    string descpCortaCalle = dt.Rows[i]["DescCortaCalle"].ToString();
                    string descCalle = dt.Rows[i]["DescpCalle"].ToString();
                    string nroCalle = dt.Rows[i]["NroCalle"].ToString();
                    string Urb = dt.Rows[i]["Urbanizacion"].ToString();
                    string deuda = dt.Rows[i]["Deuda"].ToString();
                    string meses = dt.Rows[i]["Meses"].ToString();

                    string celular = dt.Rows[i]["Celular"].ToString();
                    string estSms = dt.Rows[i]["EstadoSMS"].ToString();

                    Cliente cli = new Cliente(cod, nomCli, descpCortaCalle, descCalle, nroCalle, Urb, deuda, meses, celular, estSms);
                    listCliente.Add(cli);
                }


            }


            return listCliente.ToArray();

        }



        [WebMethod]
        public Idreg[] MostrarTipoId(int CodigoLocal, int CodigoProyecto, String CodigoPaciente)
        {
            DataTable dt = new DataTable();
            dt = this.ListarDatosxPaciente(CodigoLocal, CodigoProyecto, CodigoPaciente);
            List<Idreg> lista = new List<Idreg>();
            if (dt.Rows.Count > 0)
            {
                String vPaciente = dt.Rows[0]["NombreCompleto"].ToString();
                int vTipoTAM = Convert.ToInt32(dt.Rows[0]["TipoTAM"].ToString());
                String vIdTAM = dt.Rows[0]["IdTAM"].ToString();
                int vTipoENR = Convert.ToInt32(dt.Rows[0]["TipoENR"].ToString());
                String vIdENR = dt.Rows[0]["IdENR"].ToString();
                //if (vTipoENR == 2) msj = "auto";
                //if (vTipoENR == 0 || vTipoENR == 1) msj = "manual";
                lista.Add(new Idreg(
                    vPaciente,
                    vTipoTAM,
                    vIdTAM,
                    vTipoENR,
                    vIdENR));
            }
            return lista.ToArray();
        }

        [WebMethod]
        public string AsignarID_ENR(int TipoENR, int CodigoLocal, int CodigoProyecto, String CodigoPaciente, String IdENR, int CodigoUsuario)
        {
            DataTable dtRegistro = new DataTable();
            string msje = "";
            if (TipoENR == 0 || TipoENR == 1)
            {
                dtRegistro = this.RegistrarIdENR(CodigoLocal, CodigoProyecto, CodigoPaciente, IdENR, CodigoUsuario);
            }
            if (TipoENR == 2)
            {
                dtRegistro = this.RegistrarIdENRauto(CodigoLocal, CodigoProyecto, CodigoPaciente, CodigoUsuario);
            }
            if (dtRegistro.Rows.Count > 0)
            {
                if (dtRegistro.Rows[0]["Respuesta"].ToString() == "3")
                {
                    msje = "El ID se asigno correctamente...";
                }
                if (dtRegistro.Rows[0]["Respuesta"].ToString() == "1")
                {
                    msje = dtRegistro.Rows[0]["Texto"].ToString();
                }
            }
            return msje;
        }

        [WebMethod]
        public string AsignarID_TAM(int TipoTAM, int CodigoLocal, int CodigoProyecto, String CodigoPaciente, String IdTAM, int CodigoUsuario)
        {
            DataTable dtRegistro = new DataTable();
            string msje = "";
            if (TipoTAM == 0 || TipoTAM == 1)
            {
                dtRegistro = this.RegistrarIdTAM(CodigoLocal, CodigoProyecto, CodigoPaciente, IdTAM, CodigoUsuario);
            }
            if (TipoTAM == 2)
            {
                dtRegistro = this.RegistrarIdTAMauto(CodigoLocal, CodigoProyecto, CodigoPaciente, CodigoUsuario);
            }
            if (dtRegistro.Rows.Count > 0)
            {
                if (dtRegistro.Rows[0]["Respuesta"].ToString() == "3")
                {
                    msje = "El ID se asigno correctamente...";
                }
                if (dtRegistro.Rows[0]["Respuesta"].ToString() == "1")
                {
                    msje = dtRegistro.Rows[0]["Texto"].ToString();
                }
            }
            return msje;
        }
        public DataTable RegistrarIdENRauto(int CodigoLocal, int CodigoProyecto, string CodigoPaciente, int IdUsuario)
        {
            SqlConnection cn = con.conexion();
            SqlDataAdapter dap = new SqlDataAdapter("SPI_ID_ENR_AUTO", cn);
            DataTable dt = new DataTable();
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.AddWithValue("@CodigoLocal", CodigoLocal);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoProyecto", CodigoProyecto);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoPaciente", CodigoPaciente);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoUsuario", IdUsuario);
            dap.Fill(dt);
            return dt;
        }

        public DataTable RegistrarIdENR(int CodigoLocal, int CodigoProyecto, string CodigoPaciente, string IdENR, int IdUsuario)
        {
            SqlConnection cn = con.conexion();
            SqlDataAdapter dap = new SqlDataAdapter("SPI_ID_ENR", cn);
            DataTable dt = new DataTable();
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.AddWithValue("@CodigoLocal", CodigoLocal);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoProyecto", CodigoProyecto);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoPaciente", CodigoPaciente);
            dap.SelectCommand.Parameters.AddWithValue("@IdENR", IdENR);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoUsuario", IdUsuario);
            dap.Fill(dt);
            return dt;
        }
        public DataTable RegistrarIdTAMauto(int CodigoLocal, int CodigoProyecto, string CodigoPaciente, int IdUsuario)
        {
            SqlConnection cn = con.conexion();
            SqlDataAdapter dap = new SqlDataAdapter("SPI_ID_TAM_AUTO", cn);
            DataTable dt = new DataTable();
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.AddWithValue("@CodigoLocal", CodigoLocal);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoProyecto", CodigoProyecto);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoPaciente", CodigoPaciente);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoUsuario", IdUsuario);
            dap.Fill(dt);
            return dt;
        }
        public DataTable RegistrarIdTAM(int CodigoLocal, int CodigoProyecto, string CodigoPaciente, string IdTAM, int IdUsuario)
        {
            SqlConnection cn = con.conexion();
            SqlDataAdapter dap = new SqlDataAdapter("SPI_ID_TAM", cn);
            DataTable dt = new DataTable();
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.AddWithValue("@CodigoLocal", CodigoLocal);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoProyecto", CodigoProyecto);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoPaciente", CodigoPaciente);
            dap.SelectCommand.Parameters.AddWithValue("@IdTAM", IdTAM);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoUsuario", IdUsuario);
            dap.Fill(dt);
            return dt;
        }
        public DataTable ListarDatosxPaciente(int CodigoLocal, int CodigoProyecto, string CodigoPaciente)
        {
            SqlConnection cn = con.conexion();
            SqlDataAdapter dap = new SqlDataAdapter("SPS_CABECERA", cn);
            DataTable dt = new DataTable();
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.AddWithValue("@CodigoLocal", CodigoLocal);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoProyecto", CodigoProyecto);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoPaciente", CodigoPaciente);
            dap.Fill(dt);
            return dt;
        }
        [WebMethod]
        public int InsertarGeoPoint(
            string Latitud,
            string Longitud,
            string Fecha,
            string Hora,
            int CodigoUsuario,
            string CodigoDispositivo,
            int TransicionGeofence,
            int CodigoGeofence)
        {
            SqlConnection cn = con.conexion();
            SqlCommand cmd = new SqlCommand("SPI_GEOPOINT", cn);
            SqlTransaction trx;
            int intretorno;
            string strRespuesta;

            try
            {
                cn.Open();
                trx = cn.BeginTransaction();
                cmd.Transaction = trx;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Latitud", SqlDbType.VarChar, 50)).Value = Latitud;
                cmd.Parameters.Add(new SqlParameter("@Longitud", SqlDbType.VarChar, 50)).Value = Longitud;
                cmd.Parameters.Add(new SqlParameter("@Fecha", SqlDbType.VarChar, 10)).Value = Fecha;
                cmd.Parameters.Add(new SqlParameter("@Hora", SqlDbType.VarChar, 5)).Value = Hora;
                cmd.Parameters.Add(new SqlParameter("@CodigoUsuario", SqlDbType.Int)).Value = CodigoUsuario;
                cmd.Parameters.Add(new SqlParameter("@CodigoDispositivo", SqlDbType.VarChar, 50)).Value = CodigoDispositivo;
                cmd.Parameters.Add(new SqlParameter("@CodigoGeofence", SqlDbType.Int)).Value = CodigoGeofence;
                cmd.Parameters.Add(new SqlParameter("@TransicionGeofence", SqlDbType.Int)).Value = TransicionGeofence;
                cmd.Transaction = trx;
                intretorno = cmd.ExecuteNonQuery();
                trx.Commit();
                cn.Close();
                return intretorno;
            }
            catch (SqlException sqlException)
            {
                strRespuesta = sqlException.Message.ToString();
                cn.Close();
                return -1;
            }
            catch (Exception exception)
            {
                strRespuesta = exception.Message.ToString();
                cn.Close();
                return -1;
            }
        }

        [WebMethod]
        public int EstadoENR_TAM(string Tipo, int CodigoProyecto)
        {

            int intretorno = -1;
            string strRespuesta;

            SqlConnection cn = con.conexion();
            try
            {
                cn.Open();

                string sql = "SELECT ENR,TAM " +
                    "FROM PROYECTO " +
                    "WHERE estado=1 AND CodigoProyecto = " + CodigoProyecto;

                SqlCommand cmd = new SqlCommand(sql, cn);

                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                if (Tipo == "ENR")
                {
                    intretorno = reader.GetInt32(0);
                }
                else
                {
                    if (Tipo == "TAM")
                    {
                        intretorno = reader.GetInt32(1);
                    }
                }
                cn.Close();

                return intretorno;

            }
            catch (SqlException sqlException)
            {
                strRespuesta = sqlException.Message.ToString();
                cn.Close();
                return -1;
            }
            catch (Exception exception)
            {
                strRespuesta = exception.Message.ToString();
                cn.Close();
                return -1;
            }

        }

        [WebMethod]
        public int TienePermisos(int CodigoLocal, int CodigoUsuario, int CodigoProyecto)
        {

            int intretorno = -1;
            string strRespuesta;

            SqlConnection cn = con.conexion();
            try
            {
                cn.Open();

                string sql = "SELECT COUNT(PY.CodigoProyecto) AS num_pys  FROM ( " +
                    "select p.CodigoProyecto,p.Nombre, " +
                    "ISNULL((select Estado from USUARIOS_PROYECTO up where up.CodigoProyecto=p.CodigoProyecto " +
                    "and up.CodigoProyecto=lp.CodigoProyecto and up.CodigoUsuario = " + CodigoUsuario + " ),0) Estado " +
                    "from PROYECTO p inner join LOCAL_PROYECTO lp on lp.CodigoProyecto=p.CodigoProyecto " +
                    "where p.Estado=1 and lp.CodigoLocal = " + CodigoLocal + " ) PY WHERE PY.Estado = 1  AND PY.CodigoProyecto = " + CodigoProyecto;

                SqlCommand cmd = new SqlCommand(sql, cn);

                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                intretorno = reader.GetInt32(0);
                cn.Close();

                return intretorno;

            }
            catch (SqlException sqlException)
            {
                strRespuesta = sqlException.Message.ToString();
                cn.Close();
                return -1;
            }
            catch (Exception exception)
            {
                strRespuesta = exception.Message.ToString();
                cn.Close();
                return -1;
            }

        }

        #region SEISLab Movil

        [WebMethod]

        public CabeceraSA ObtenerCabeceraSA(string IdMuestra)
        {

            CabeceraSA cab = new CabeceraSA();
            SqlConnection sqlcon = con.conexion();
            DataTable dt = new DataTable();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            try
            {
                sqlcon.Open();
                sqlcmd.CommandText = "SPS_BUSQUEDA_SA_MOVIL";
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@IdMuestra", IdMuestra);
                sqlcmd.Parameters.AddWithValue("@accion", '1');
                SqlDataAdapter dataAdap = new SqlDataAdapter(sqlcmd);
                dataAdap.Fill(dt);
                string idTam = dt.Rows[0]["IdTam"].ToString();
                string tipo_m = dt.Rows[0]["tipo_muestra"].ToString();
                string edad = dt.Rows[0]["Edad"].ToString();

                cab.Id_Muestra = IdMuestra;
                cab.IdTam = idTam;
                cab.Tipo_muestra = tipo_m;
                cab.VarEdad = edad;
            }

            catch (Exception EX)
            {
                string msj = EX.Message;

            }

            finally { sqlcon.Close(); }
            return cab;

        }

        [WebMethod]

        public string ObtenerEstadoExamen(string CodigoExa, string CodSA)
        {

            string result = "";
            string Validado, Registrado, Liberado, Pendiente;


            SqlConnection sqlcon = con.conexion();
            DataTable dt = new DataTable();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            try
            {
                sqlcon.Open();
                sqlcmd.CommandText = "SPS_BUSQUEDA_SA_MOVIL";
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@codigoSA", CodSA);
                sqlcmd.Parameters.AddWithValue("@cogidoExa ", CodigoExa);
                sqlcmd.Parameters.AddWithValue("@accion", '2');
                SqlDataAdapter dataAdap = new SqlDataAdapter(sqlcmd);
                dataAdap.Fill(dt);
                Validado = this.ValidarEstadoExamen("Validado", dt);
                Registrado = this.ValidarEstadoExamen("Registrado", dt);
                Liberado = this.ValidarEstadoExamen("Liberado", dt);
                Pendiente = this.ValidarEstadoExamen("Pendiente", dt);

                if (Validado == "Si" && Liberado != "Si")
                {
                    result = "Validado";
                }
                else if (Registrado == "Si" && Validado != "Si" && Liberado != "Si")
                {
                    result = "Registrado";
                }

                else if (Liberado == "Si" && Validado != "Si")
                {
                    result = "Liberado";
                }

                else if (Pendiente == "Si")
                {
                    result = "Pendiente";
                }
                else if (Validado == "Si" && Liberado == "Si")
                {
                    result = "Liberado";

                }



            }
            catch (Exception ex)
            {

                result = ex.Message;

            }

            return result;


        }



        public string ValidarEstadoExamen(string estado, DataTable dt)
        {


            string result = "No";


            if (estado == "Registrado")
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["FlagR"].ToString() == "1")
                    {

                        result = "Si";
                        break;

                    }



                }



            }

            else if (estado == "Pendiente")
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["FlagR"].ToString() == "1")
                    {

                        result = "No";
                        break;

                    }

                    else if (dt.Rows[i]["FlagR"].ToString() == "N")
                    {

                        result = "Si";


                    }




                }



            }

            else if (estado == "Validado")
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["FlagV"].ToString() == "N")
                    {

                        result = "No";
                        break;

                    }

                    else if (dt.Rows[i]["FlagV"].ToString() == "1")
                    {

                        result = "Si";


                    }

                }

            }

            else if (estado == "Liberado")
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["FlagI"].ToString() == "N")
                    {

                        result = "No";
                        break;

                    }

                    else if (dt.Rows[i]["FlagI"].ToString() == "1")
                    {

                        result = "Si";


                    }

                }

            }


            return result;

        }

        [WebMethod]
        public ExamenSA[] ListarExamenesDispon(string codArea, string TipoExa)
        {

            List<ExamenSA> lisExam = new List<ExamenSA>();
            SqlConnection sqlcon = con.conexion();
            DataTable dt = new DataTable();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;

            try
            {
                sqlcon.Open();
                sqlcmd.CommandText = "SPS_BUSQUEDA_SA_MOVIL";
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@cogidoArea ", codArea);
                sqlcmd.Parameters.AddWithValue("@nomTipoMuestra", TipoExa);
                sqlcmd.Parameters.AddWithValue("@accion", '5');
                SqlDataAdapter dataAdap = new SqlDataAdapter(sqlcmd);
                dataAdap.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string codExa = dt.Rows[i]["CodigoExamen"].ToString();

                    string examenNom = dt.Rows[i]["Nombre"].ToString();


                    ExamenSA Exa = new ExamenSA(codExa, examenNom);
                    lisExam.Add(Exa);
                }



            }

            catch (Exception EX)
            {
                string msj = EX.Message;

            }

            finally { sqlcon.Close(); }

            return lisExam.ToArray();





        }

        public string InsertarSAExamenesResultados(int codSA, int codLocal, int codProyecto, int codUser)
        {/**/

            string result = "";
            SqlConnection sqlcon = con.conexion();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;

            try
            {
                sqlcon.Open();
                sqlcmd.CommandText = "SPI_SA_RESULTADOS_CARGAR";
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@idSA", codSA);
                sqlcmd.Parameters.AddWithValue("@CodigoLocal", codLocal);
                sqlcmd.Parameters.AddWithValue("@CodigoProyecto", codProyecto);
                sqlcmd.Parameters.AddWithValue("@CodigoUsuario", codUser);
                sqlcmd.Parameters.AddWithValue("@Sexo", 0);

                if (sqlcmd.ExecuteNonQuery() == 1)
                {

                    result = "OK";

                }

            }

            catch (SqlException sqlEx)
            {


                result = sqlEx.Message;

            }

            catch (Exception ex)
            {

                result = ex.Message;


            }

            finally
            {
                sqlcon.Close();

            }

            return result;

        }



        public string InsertarNuevosExamenes(string CodSA, string CodArea, string CodExa, string CodUser, int CodLocal, int CodProyec, int CodGrupoVis, int CodVisita, int CodEmp, int CodMuest, int CodigoVisitas, string codPaciente)
        {
            string result = "";
            SqlConnection sqlcon = con.conexion();
            DataTable dt = new DataTable();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;

            try
            {
                sqlcon.Open();
                sqlcmd.CommandText = "SPI_SA_EXA_VISIT_PACIENTE";
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@CodigoSA", CodSA);
                sqlcmd.Parameters.AddWithValue("@CodigoLocal", CodLocal);
                sqlcmd.Parameters.AddWithValue("@CodigoProyecto", CodProyec);
                sqlcmd.Parameters.AddWithValue("@CodigoGrupoVisita", CodGrupoVis);
                sqlcmd.Parameters.AddWithValue("@CodigoVisita", CodVisita);
                sqlcmd.Parameters.AddWithValue("@CodigoEmpresa", CodEmp);
                sqlcmd.Parameters.AddWithValue("@CodigoArea", CodArea);
                sqlcmd.Parameters.AddWithValue("@CodigoTipoMuestra", CodMuest);
                sqlcmd.Parameters.AddWithValue("@CodigoExamen", CodExa);
                sqlcmd.Parameters.AddWithValue("@CodigoVisitas", CodigoVisitas);
                sqlcmd.Parameters.AddWithValue("@CodigoPaciente", codPaciente);
                sqlcmd.Parameters.AddWithValue("@CodigoUsuario", CodUser);

                if (sqlcmd.ExecuteNonQuery() == 1)
                {

                    result = "OK";

                }

            }

            catch (SqlException sqlEx)
            {


                result = sqlEx.Message;

            }

            catch (Exception ex)
            {

                result = ex.Message;


            }

            finally
            {
                sqlcon.Close();

            }

            return result;
        }


        [WebMethod]

        public string AgregarExamenes(string CodSA, string CodArea, string nomTipoMuestra, string CodExa, string CodUser)
        {
            string result = "";
            SqlConnection sqlcon = con.conexion();
            DataTable dt = new DataTable();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            try
            {
                sqlcon.Open();
                sqlcmd.CommandText = "SPS_BUSQUEDA_SA_MOVIL";
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@nomTipoMuestra", nomTipoMuestra);
                sqlcmd.Parameters.AddWithValue("@codigoSA", CodSA);
                sqlcmd.Parameters.AddWithValue("@accion", '6');
                SqlDataAdapter dataAdap = new SqlDataAdapter(sqlcmd);


                dataAdap.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    int codlocal = Convert.ToInt32(dt.Rows[i]["CodigoLocal"].ToString());
                    int codProyecto = Convert.ToInt32(dt.Rows[i]["CodigoProyecto"].ToString());
                    int codGrupoVisita = Convert.ToInt32(dt.Rows[i]["CodigoGrupoVisita"].ToString());
                    int codVisita = Convert.ToInt32(dt.Rows[i]["CodigoVisita"].ToString());
                    int codEmpresa = Convert.ToInt32(dt.Rows[i]["CodigoEmpresa"].ToString());
                    int codMuestra = Convert.ToInt32(dt.Rows[i]["IdTipoMuestra"].ToString());
                    int codigoVisitas = Convert.ToInt32(dt.Rows[i]["CodigoVisitas"].ToString());
                    string codPaciente = dt.Rows[i]["CodigoPaciente"].ToString();
                    string var_codArea = this.GetCodAreaExamen(CodExa);

                    result = this.InsertarNuevosExamenes(CodSA, var_codArea, CodExa, CodUser, codlocal, codProyecto, codGrupoVisita, codVisita, codEmpresa, codMuestra, codigoVisitas, codPaciente);
                    if (result == "OK")
                    {/**/

                        result = this.InsertarSAExamenesResultados(Convert.ToInt32(CodSA), codlocal, codProyecto, Convert.ToInt32(CodUser));
                    }
                }

            }
            catch (Exception ex)
            {

                result = ex.Message;

            }

            return result;


        }


        public string GetCodAreaExamen(string codExamen)
        {/**/

            string result = "";
            SqlConnection sqlcon = con.conexion();
            DataTable dt = new DataTable();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;

            try
            {
                sqlcon.Open();
                sqlcmd.CommandText = "SPS_BUSQUEDA_SA_MOVIL";
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@cogidoExa", codExamen);
                sqlcmd.Parameters.AddWithValue("@accion", "7");
                SqlDataAdapter dataAdap = new SqlDataAdapter(sqlcmd);
                dataAdap.Fill(dt);



                result = dt.Rows[0]["CodigoArea"].ToString();




            }
            catch (Exception ex)
            {

                result = ex.Message;

            }

            return result;
        }




        [WebMethod]

        public ExamenSA[] ListaExamenesSA(string IdMuestra)
        {

            List<ExamenSA> lisExam = new List<ExamenSA>();
            SqlConnection sqlcon = con.conexion();
            DataTable dt = new DataTable();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            try
            {
                sqlcon.Open();
                sqlcmd.CommandText = "SPS_BUSQUEDA_SA_MOVIL";
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@IdMuestra", IdMuestra);
                sqlcmd.Parameters.AddWithValue("@accion", '1');
                SqlDataAdapter dataAdap = new SqlDataAdapter(sqlcmd);
                dataAdap.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string codsa = dt.Rows[i]["CodigoSA"].ToString();
                    string codExa = dt.Rows[i]["CodigoExamen"].ToString();
                    string flagp = dt.Rows[i]["FlagP"].ToString();
                    string flagv = dt.Rows[i]["FlagV"].ToString();
                    string flagi = dt.Rows[i]["FlagI"].ToString();
                    string examenNom = dt.Rows[i]["Examen"].ToString();
                    string flagr = dt.Rows[i]["FlagR"].ToString();

                    ExamenSA Exa = new ExamenSA(codsa, codExa, flagp, flagv, flagi, examenNom, flagr);
                    lisExam.Add(Exa);
                }



            }

            catch (Exception EX)
            {
                string msj = EX.Message;

            }

            finally { sqlcon.Close(); }

            return lisExam.ToArray();

        }


        [WebMethod]
        public OpcionAnalito[] ListaOpcionAnalito(string codAnalito, string codSA, string codExa)
        {

            List<OpcionAnalito> opAnalitos = new List<OpcionAnalito>();
            SqlConnection sqlcon = con.conexion();
            int flag = 0;
            DataTable dt = new DataTable();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;

            try
            {
                sqlcon.Open();
                sqlcmd.CommandText = "SPS_BUSQUEDA_SA_MOVIL";
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@codigoAnalito", codAnalito);

                sqlcmd.Parameters.AddWithValue("@accion", '3');

                sqlcmd.Parameters.AddWithValue("@codigoSA", Convert.ToInt32(codSA));
                sqlcmd.Parameters.AddWithValue("@cogidoExa", codExa);

                SqlDataAdapter dataAdap = new SqlDataAdapter(sqlcmd);
                dataAdap.Fill(dt);
                //string ISNULL = dt.Rows[0]["EsNulo"].ToString();
                //if (ISNULL == "1")
                //{
                //    OpcionAnalito opnull = new OpcionAnalito("0", "Seleccione opcion");
                //    opAnalitos.Add(opnull);
                //    /**/
                //}



                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string codOp = dt.Rows[i]["CodigoOpcionResultado"].ToString();
                    flag = Convert.ToInt32(codOp);
                    string descripcion = dt.Rows[i]["Glosa"].ToString();

                    OpcionAnalito op = new OpcionAnalito(codOp, descripcion);
                    opAnalitos.Add(op);


                }

                OpcionAnalito opcNull = new OpcionAnalito(Convert.ToString(flag + 1), "Sin Valor");
                opAnalitos.Add(opcNull);
            }

            catch (Exception ex)
            {

                string msj = ex.Message;

            }

            finally { sqlcon.Close(); }


            return opAnalitos.ToArray();
        }


        [WebMethod]

        public string ActualizarAnalito(string resultado, string codOpc, string codSA, string codAnalito, string fechaPro)
        {

            string result = "";
            SqlConnection sqlcon = con.conexion();

            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            sqlcmd.CommandText = "SPI_SA_RESULTADOS_MOVIL";
            sqlcmd.CommandType = CommandType.StoredProcedure;

            try
            {

                sqlcon.Open();
                sqlcmd.Parameters.AddWithValue("@resultado", resultado);
                sqlcmd.Parameters.AddWithValue("@codigoSA", Convert.ToInt32(codSA));
                sqlcmd.Parameters.AddWithValue("@codigoAnalito", codAnalito);
                sqlcmd.Parameters.AddWithValue("@codigoOpc", codOpc);
                sqlcmd.Parameters.AddWithValue("@fechaPro", Convert.ToDateTime(fechaPro));


                if (sqlcmd.ExecuteNonQuery() == 1)
                {
                    result = "OK";

                }




            }
            catch (SqlException ex)
            {

                result = ex.Message;

            }

            finally
            {

                sqlcon.Close();

            }

            return result;

        }




        [WebMethod]
        public Area[] ListarArea()
        {

            List<Area> areas = new List<Area>();
            SqlConnection sqlcon = con.conexion();
            DataTable dt = new DataTable();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;

            try
            {
                sqlcon.Open();
                sqlcmd.CommandText = "SPS_BUSQUEDA_SA_MOVIL";
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@accion", '4');
                SqlDataAdapter dataAdap = new SqlDataAdapter(sqlcmd);
                dataAdap.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string codArea = dt.Rows[i]["CodigoArea"].ToString();
                    string nombre = dt.Rows[i]["Nombre"].ToString();
                    string numero = dt.Rows[i]["Numero"].ToString();

                    Area ar = new Area();
                    ar.CodArea = codArea;
                    ar.Nombre = nombre;
                    ar.Numero = numero;
                    areas.Add(ar);


                    // Analito an = new Analito(codAnalito, nombre, tipo, resultado);
                    // analitos.Add(an);

                }
            }

            catch (Exception ex)
            {

                string msj = ex.Message;

            }

            finally { sqlcon.Close(); }


            return areas.ToArray();

        }


        [WebMethod]
        public Analito[] ListarAnalitos(string CodigoExa, string CodSA)
        {
            List<Analito> analitos = new List<Analito>();

            SqlConnection sqlcon = con.conexion();
            DataTable dt = new DataTable();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;

            try
            {
                sqlcon.Open();
                sqlcmd.CommandText = "SPS_BUSQUEDA_SA_MOVIL";
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@codigoSA", CodSA);
                sqlcmd.Parameters.AddWithValue("@cogidoExa ", CodigoExa);
                sqlcmd.Parameters.AddWithValue("@accion", '2');
                SqlDataAdapter dataAdap = new SqlDataAdapter(sqlcmd);
                dataAdap.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string codAnalito = dt.Rows[i]["CodigoAnalito"].ToString();
                    string nombre = dt.Rows[i]["Nombre"].ToString();
                    string tipo = dt.Rows[i]["Tipo"].ToString();
                    string resultado = "";
                    if (tipo == "3")
                    {

                        resultado = dt.Rows[i]["CodigoOpcion"].ToString();
                    }

                    else if (tipo == "2" || tipo == "1")
                    {

                        resultado = dt.Rows[i]["Resultado"].ToString();

                    }


                    Analito an = new Analito(codAnalito, nombre, tipo, resultado);
                    analitos.Add(an);

                }
            }

            catch (Exception ex)
            {

                string msj = ex.Message;

            }

            finally { sqlcon.Close(); }

            return analitos.ToArray();


        }

        [WebMethod]
        public string EliminarAnalito(string codSA, string codAnalito)
        {/**/
            string res = "Error";
            SqlConnection sqlcon = con.conexion();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            sqlcmd.CommandText = "SPD_ANALITOS_SA_RESULTADOS";
            sqlcmd.CommandType = CommandType.StoredProcedure;

            try
            {

                sqlcon.Open();
                sqlcmd.Parameters.AddWithValue("@codigoAnalito", codAnalito);
                sqlcmd.Parameters.AddWithValue("@codSA", Convert.ToInt32(codSA));


                if (sqlcmd.ExecuteNonQuery() == 1)
                {
                    res = "OK";

                }




            }
            catch (Exception ex)
            {

                res = ex.Message;

            }

            finally
            {

                sqlcon.Close();

            }

            return res;


        }

        [WebMethod]

        public VisitaSA[] ListarVisitasSA(string codGrupoVis, string codProyecto)
        {
            List<VisitaSA> visitasSA = new List<VisitaSA>();
            SqlConnection sqlcon = con.conexion();
            DataTable dt = new DataTable();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;


            try
            {
                sqlcon.Open();
                sqlcmd.CommandText = "SPS_BUSCAR_PACIENTE_SA_MOVIL";
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@codigoProyecto ", Convert.ToInt32(codProyecto));
                sqlcmd.Parameters.AddWithValue("@codGrupoVis ", Convert.ToInt32(codGrupoVis));
                sqlcmd.Parameters.AddWithValue("@accion ", '1');
                SqlDataAdapter dataAdap = new SqlDataAdapter(sqlcmd);
                dataAdap.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string codVisita = dt.Rows[i]["CodigoVisita"].ToString();
                    string descripcion = dt.Rows[i]["Descripcion"].ToString();
                    string nombre = dt.Rows[i]["Nombre"].ToString();

                    VisitaSA vis = new VisitaSA(codVisita, nombre, descripcion);
                    visitasSA.Add(vis);


                }
            }

            catch (Exception ex)
            {

                string msj = ex.Message;

            }

            return visitasSA.ToArray();


        }


        [WebMethod]

        public GrupoVisita[] ListaGrupoVisita(string codProyec)
        {

            List<GrupoVisita> grupovisitas = new List<GrupoVisita>();
            SqlConnection sqlcon = con.conexion();
            DataTable dt = new DataTable();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;


            try
            {
                sqlcon.Open();
                sqlcmd.CommandText = "SPS_GRUPO_VISITA_X_PROYECTO";
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@CodigoProyecto", Convert.ToInt32(codProyec));

                SqlDataAdapter dataAdap = new SqlDataAdapter(sqlcmd);
                dataAdap.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string codigoGrup = dt.Rows[i]["CodigoGrupoVisita"].ToString();
                    string descripcion = dt.Rows[i]["Descripcion"].ToString();


                    GrupoVisita grup = new GrupoVisita(codigoGrup, descripcion);
                    grupovisitas.Add(grup);




                }
            }

            catch (Exception ex)
            {

                string msj = ex.Message;

            }

            finally { sqlcon.Close(); }

            return grupovisitas.ToArray();


        }


        [WebMethod]

        public Muestra[] ObtenerMuestras()
        {


            SqlConnection sqlcon = con.conexion();
            DataTable dt = new DataTable();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            List<Muestra> listMuestras = new List<Muestra>();


            try
            {
                sqlcon.Open();
                sqlcmd.CommandText = "SPS_TIPO_MUESTRA_ACTIVAS";
                sqlcmd.CommandType = CommandType.StoredProcedure;
                //  sqlcmd.Parameters.AddWithValue("@CodigoProyecto", Convert.ToInt32(codProyec));

                SqlDataAdapter dataAdap = new SqlDataAdapter(sqlcmd);
                dataAdap.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string codMuestra = dt.Rows[i]["CodigoTipoMuestra"].ToString();
                    string descripcion = dt.Rows[i]["Nombre"].ToString();
                    string check = "false";

                    Muestra muestra = new Muestra(codMuestra, descripcion, check);

                    listMuestras.Add(muestra);





                }
            }

            catch (Exception ex)
            {

                string msj = ex.Message;

            }

            finally { sqlcon.Close(); }

            return listMuestras.ToArray();



        }


        [WebMethod]

        public Material[] ObetnerMateriales(string codMuestra)
        {

            List<Material> ListMateriales = new List<Material>();

            SqlConnection sqlcon = con.conexion();
            DataTable dt = new DataTable();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;




            try
            {
                sqlcon.Open();
                sqlcmd.CommandText = "SPS_MATERIAL_X_TIPOMUESTRA";
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@CodigoTipoMuestra", Convert.ToInt32(codMuestra));
                SqlDataAdapter dataAdap = new SqlDataAdapter(sqlcmd);
                dataAdap.Fill(dt);


                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string codMaterial = dt.Rows[i]["CodigoMaterial"].ToString();
                    string tipoMuestra = dt.Rows[i]["TipoMuestra"].ToString();
                    string presentacion = dt.Rows[i]["Presentacion"].ToString();
                    string reactivo = dt.Rows[i]["Reactivo"].ToString();
                    string vol = dt.Rows[i]["Volumen"].ToString();
                    string cantd = dt.Rows[i]["Cant"].ToString();


                    Material matrial = new Material(codMaterial, tipoMuestra, presentacion, reactivo, vol, cantd);

                    ListMateriales.Add(matrial);

                }


            }

            catch (Exception ex)
            {

                string ms = ex.Message;
            }


            finally { sqlcon.Close(); }

            return ListMateriales.ToArray();



        }

        //SPS_BUSCAR_PACIENTE_X_DNI

        [WebMethod]
        public PacienteContacto[] BuscarPacientePorDni(string dni)
        {

            SqlConnection sqlcon = con.conexion();
            DataTable dt = new DataTable();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            List<PacienteContacto> listPac = new List<PacienteContacto>();

            try
            {
                sqlcon.Open();
                sqlcmd.CommandText = "SPS_BUSCAR_PACIENTE_X_DNI";
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@DNI", dni);
                SqlDataAdapter dataAdap = new SqlDataAdapter(sqlcmd);
                dataAdap.Fill(dt);

                if (dt != null && dt.Rows.Count > 0)

                {
                    PacienteContacto pct = new PacienteContacto();
                    pct.VarDepart = dt.Rows[0]["CodigoUbigeo"].ToString();
                    pct.VarProv = dt.Rows[0]["Nombres"].ToString();
                    pct.VarDist = "";
                    pct.VarDomicilo = dt.Rows[0]["Direccion"].ToString();
                    pct.VarReferencia = dt.Rows[0]["Referencia"].ToString();
                    pct.VarCorreo = dt.Rows[0]["Correo"].ToString();
                    pct.VarCelular = dt.Rows[0]["Celular"].ToString();
                    pct.VarTelefono = dt.Rows[0]["Telefono1"].ToString();
                    pct.VarGeoLongitud = dt.Rows[0]["Geo_Longitud"].ToString();
                    pct.VarGeoLatitud = dt.Rows[0]["Geo_Latitud"].ToString();
                    pct.VarEstadoCivil = dt.Rows[0]["CodigoEstadoCivil"].ToString();
                    pct.VarGradoInst = dt.Rows[0]["CodigoGradoInstruccion"].ToString();
                    pct.VarDepNacimiento = dt.Rows[0]["CodigoUbigeoNacimiento"].ToString();
                    pct.VarProvNacimiento = "";
                    pct.VarDistNacinmiento = "";
                    listPac.Add(pct);
                }

            }
            catch
            {/**/
            }

            return listPac.ToArray();
        }

        [WebMethod]
        public PacienteSA ObertenerPacienteSA(string codGrupoVis, string codProyecto, string codVisita, string Dni)
        {

            SqlConnection sqlcon = con.conexion();
            DataTable dt = new DataTable();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            PacienteSA result = null;


            try
            {
                sqlcon.Open();
                sqlcmd.CommandText = "SPS_BUSCAR_PACIENTE_SA_MOVIL";
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@accion", "2");
                sqlcmd.Parameters.AddWithValue("@codGrupoVis", Convert.ToInt32(codGrupoVis));
                sqlcmd.Parameters.AddWithValue("@codigoProyecto", Convert.ToInt32(codProyecto));
                sqlcmd.Parameters.AddWithValue("@CodVisita", Convert.ToInt32(codVisita));
                sqlcmd.Parameters.AddWithValue("@Dni", Dni);


                SqlDataAdapter dataAdap = new SqlDataAdapter(sqlcmd);
                dataAdap.Fill(dt);


                string codPac = dt.Rows[0]["CodigoPaciente"].ToString();
                string Nombre = dt.Rows[0]["Nombres"].ToString();
                string ApellioP = dt.Rows[0]["ApellidoPaterno"].ToString();
                string ApellioM = dt.Rows[0]["ApellidoMaterno"].ToString();
                string docIdent = dt.Rows[0]["IdTam"].ToString();
                string fechaN = dt.Rows[0]["FechaNac"].ToString();
                string sexo = dt.Rows[0]["Sexo"].ToString();
                string codVisitas = dt.Rows[0]["CodigoVisitas"].ToString();

                PacienteSA part = new PacienteSA(codPac, Nombre, ApellioP, ApellioM, docIdent, fechaN, sexo, codVisitas);
                result = part;

            }

            catch (Exception ex)
            {

                string ms = ex.Message;
            }


            finally { sqlcon.Close(); }

            return result;

        }

        // SEIS LAB  FEBRERO 2016

        [WebMethod]
        public ClassAutocomplete[] GetArrayAtucomplete(string idMuestra)
        {
            //SPS_BUSQUEDA_SA_MOVIL


            List<ClassAutocomplete> listAutocomplete = new List<ClassAutocomplete>();

            SqlConnection sqlcon = con.conexion();
            DataTable dt = new DataTable();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;


            try
            {
                sqlcon.Open();
                sqlcmd.CommandText = "SPS_BUSQUEDA_SA_MOVIL";
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@IdMuestra", idMuestra);
                sqlcmd.Parameters.AddWithValue("@accion", "8");
                SqlDataAdapter dataAdap = new SqlDataAdapter(sqlcmd);
                dataAdap.Fill(dt);


                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string var = dt.Rows[i]["Numero"].ToString();
                    ClassAutocomplete claut = new ClassAutocomplete();
                    claut.IdMuestra = var;
                    listAutocomplete.Add(claut);




                }


            }

            catch (Exception ex)
            {

                string ms = ex.Message;
                //listAutocomplete.Add(ms);
            }


            finally { sqlcon.Close(); }

            return listAutocomplete.ToArray();




        }



        // FIN

        [WebMethod]
        public int AgregarHuella(string CodigoPaciente, string Huella)
        {
            SqlConnection cn = con.conexion();
            SqlCommand cmd = new SqlCommand("INSERT INTO Huellas VALUES ('" +
                    CodigoPaciente + "', '" + Huella + "')", cn);
            SqlTransaction trx;
            int intretorno;

            try
            {
                cn.Open();
                trx = cn.BeginTransaction();
                cmd.Transaction = trx;
                intretorno = cmd.ExecuteNonQuery();
                trx.Commit();
                cn.Close();
                return intretorno;
            }
            catch (SqlException sqlException)
            {
                cn.Close();
                return -1;
            }
            catch (Exception exception)
            {
                cn.Close();
                return -1;
            }
        }

        // checa si la informacion de un paciente ya ha sido insertada
        [WebMethod]
        public int PacienteTieneHuella(string CodigoPaciente)
        {
            SqlConnection cn = con.conexion();
            cn.Open();
            int existe = 0;
            string sql = "SELECT Huella FROM Huellas WHERE CodigoPaciente = '" +
                    CodigoPaciente + "'";

            SqlCommand cmd = new SqlCommand(sql, cn);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                existe = 1;
            }
            cn.Close();
            return existe;
        }

        // procesa todas las huellas salvadas para ver si un coincide con 
        // una huella de prueba.
        [WebMethod]
        public string BuscarHuella(string Huella)
        {
            byte[] huellaTemplate;

            SGFingerPrintManager m_FPM;
            Int32 err;


            try
            {
                // process the given fingerprint into the correct template
                huellaTemplate = Convert.FromBase64String(Huella);
            }
            catch (Exception e)
            {
                return "fingerprintNotConverted";
            }

            try
            {
                // load in the fingerprint tools
                m_FPM = new SGFingerPrintManager();
                err = m_FPM.InitEx(300, 400, 500);

                if (err != (Int32) SGFPMError.ERROR_NONE)
                {
                    return "Secugen Load Error " + err.ToString();
                }
            }
            catch (Exception e)
            {
                return "fingerprintManagerNotFound";
            }

            try
            {
                // prep the data retrieval of fingerprints
                SqlConnection cn = con.conexion();
                cn.Open();
                string sql = "SELECT CodigoPaciente, Huella FROM Huellas";
                SqlCommand cmd = new SqlCommand(sql, cn);
                SqlDataReader reader = cmd.ExecuteReader();

                string fingerprintStr;
                byte[] fingerprintTemplate;
                bool matched = false;
                bool error_triggered = false;



                while (reader.Read())
                {
                    fingerprintStr = reader.GetString(1);
                    try
                    {
                        fingerprintTemplate = Convert.FromBase64String(fingerprintStr);

                        SGFPMSecurityLevel secu_level = SGFPMSecurityLevel.NORMAL;
                        err = m_FPM.MatchTemplate(huellaTemplate, fingerprintTemplate, secu_level, ref matched);
                        if (err == (Int32) SGFPMError.ERROR_NONE)
                        {
                            if (matched)
                            {

                                string usuario = reader.GetString(0);
                                // return CodigoPaciente for hits
                                cn.Close();
                                return usuario;
                            }
                        }
                        else
                        {
                            return "SecugenError: " + err.ToString();
                        }
                    }
                    catch (Exception e)
                    {
                        error_triggered = true;
                    }

                }
                cn.Close();
                if (error_triggered)
                {
                    return "someMatchDidntWork";
                }
                else
                {
                    return "fingerprintNotFound";
                }


            }
            catch (Exception e)
            {
                return e.Message;
            }



        }


        /* [WebMethod]
   public int[] VentanaDeVisitas(string CodigoProyecto, string CodigoGrupoVisita, string CodigoVisita)
   {
       SqlConnection cn = con.conexion();
       cn.Open();

       sql = "SELECT DiasAntes, DiasDespues FROM Visita WHERE CodigoProyecto=" + 
               CodigoProyecto + " AND CodigoGrupoVisita=" + CodigoGrupoVisita +
               " AND CodigoVisita=" + CodigoVisita; 

        SqlCommand cmd = new SqlCommand(sql, cn);
        SqlDataReader reader = cmd.ExecuteReader();

        if (reader.HasRows) {
           reader.Read();
           return [reader.GetInt32(0), reader.GetInt32(1)];
        }
        else {

        }

   } */


        [WebMethod]
        public Visita[] ListadoGrupoVisita1(int CodigoProyecto)
        {
            string sql;
            SqlConnection cn = con.conexion();
            cn.Open();


            sql = "SELECT CodigoProyecto, CodigoGrupoVisita, Nombre, CodigoVisita, " +
            "Descripcion, Auto, DiasVisitaProx, DiasAntes, DiasDespues, " +
            "OrdenVisita FROM Visita WHERE CodigoProyecto=" + CodigoProyecto;
            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataAdapter dap = new SqlDataAdapter(sql, cn);
            SqlDataReader reader = cmd.ExecuteReader();

            List<Visita> lista = new List<Visita>();

            while (reader.Read())
            {
                lista.Add(new Visita(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetString(2),
                    reader.GetInt32(3),
                    reader.GetString(4),
                   Convert.ToBoolean(reader.GetInt32(5)),
                    0, // dependiente does not exist in database
                    reader.GetInt32(6),
                    reader.GetInt32(7),
                    reader.GetInt32(8),
                     reader.GetInt32(9)));
            }

            cn.Close();
            return lista.ToArray();
        }


        [WebMethod]
        public Participante[] ObtenerPacienteDeCodigoPaciente(string codigopaciente)
        {

            try
            {
                SqlConnection cn = con.conexion();

                cn.Open();
                string sql = "select CONVERT(varchar(100), CodigoPaciente, 103) AS CodigoPaciente,Nombres,ApellidoPaterno,ApellidoMaterno," +
                        "CodigoTipoDocumento,DocumentoIdentidad,convert(varchar(10),FechaNacimiento,103) AS FechaNacimiento," +
                        "Sexo from PACIENTE WHERE CodigoPaciente = '" + codigopaciente + "'";

                SqlCommand cmd = new SqlCommand(sql, cn);

                SqlDataReader reader = cmd.ExecuteReader();

                List<Participante> lista = new List<Participante>();

                while (reader.Read())
                {
                    lista.Add(new Participante(
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetInt32(4),
                        reader.GetString(5),
                        reader.GetString(6),
                        reader.GetInt32(7)));
                }

                cn.Close();

                return lista.ToArray();
            }



            catch (Exception ex)
            {
                return null;
            }

        }

        [WebMethod]
        public string BuscarHuellaFiltrado(string Huella, string nombres, string apellidop, string apellidom)
        {
            byte[] huellaTemplate;

            SGFingerPrintManager m_FPM;
            Int32 err;


            try
            {
                // process the given fingerprint into the correct template
                huellaTemplate = Convert.FromBase64String(Huella);
            }
            catch (Exception e)
            {
                return "fingerprintNotConverted";
            }

            try
            {
                // load in the fingerprint tools
                m_FPM = new SGFingerPrintManager();
                err = m_FPM.InitEx(300, 400, 500);

                if (err != (Int32) SGFPMError.ERROR_NONE)
                {
                    return "Secugen Load Error " + err.ToString();
                }
            }
            catch (Exception e)
            {
                return "fingerprintManagerNotFound";
            }

            try
            {
                // prep the data retrieval of fingerprints
                SqlConnection cn = con.conexion();
                cn.Open();

                string where_clause = "WHERE";
                bool first_done = false;
                if (nombres != "")
                {
                    if (first_done)
                    {
                        where_clause += " AND Paciente.Nombres = '" + nombres.ToUpper().Trim() + "'";
                    }
                    else
                    {
                        first_done = true;
                        where_clause += " Paciente.Nombres = '" + nombres.ToUpper().Trim() + "'";
                    }
                }
                if (apellidom != "")
                {
                    if (first_done)
                    {
                        where_clause += " AND Paciente.ApellidoMaterno = '" + apellidom.ToUpper().Trim() + "'";
                    }
                    else
                    {
                        first_done = true;
                        where_clause += " Paciente.ApellidoMaterno = '" + apellidom.ToUpper().Trim() + "'";
                    }
                }
                if (apellidop != "")
                {
                    if (first_done)
                    {
                        where_clause += " AND Paciente.ApellidoPaterno = '" + apellidop.ToUpper().Trim() + "'";
                    }
                    else
                    {
                        first_done = true;
                        where_clause += " Paciente.ApellidoPaterno = '" + apellidop.ToUpper().Trim() + "'";
                    }
                }


                string sql = "SELECT Huellas.CodigoPaciente, Huellas.Huella FROM Huellas INNER JOIN Paciente " +
                                "ON Huellas.CodigoPaciente = convert(varchar(36), Paciente.CodigoPaciente)" +
                                where_clause;
                SqlCommand cmd = new SqlCommand(sql, cn);
                SqlDataReader reader = cmd.ExecuteReader();

                string fingerprintStr;
                byte[] fingerprintTemplate;
                bool matched = false;
                bool error_triggered = false;



                while (reader.Read())
                {
                    fingerprintStr = reader.GetString(1);
                    try
                    {
                        fingerprintTemplate = Convert.FromBase64String(fingerprintStr);

                        SGFPMSecurityLevel secu_level = SGFPMSecurityLevel.NORMAL;
                        err = m_FPM.MatchTemplate(huellaTemplate, fingerprintTemplate, secu_level, ref matched);
                        if (err == (Int32) SGFPMError.ERROR_NONE)
                        {
                            if (matched)
                            {

                                string usuario = reader.GetString(0);
                                // return CodigoPaciente for hits
                                cn.Close();
                                return usuario;
                            }
                        }
                        else
                        {
                            return "SecugenError: " + err.ToString();
                        }
                    }
                    catch (Exception e)
                    {
                        error_triggered = true;
                    }

                }
                cn.Close();
                if (error_triggered)
                {
                    return "someMatchDidntWork";
                }
                else
                {
                    return "fingerprintNotFound";
                }


            }
            catch (Exception e)
            {
                return e.Message; ;
            }



        }


        [WebMethod]
        public Visitas2[] ListadoVisitas5(string CodigoPaciente, string CodigoUsuario, string CodigoProyecto)
        {
            SqlConnection cn = con.conexion();
            cn.Open();
            string sql = "SELECT PY.Nombre AS Proyecto, E.Nombre AS Visita, " +
                 "SUBSTRING(DATENAME(dw, V.FechaVisita), 1, 3) + ' ' + CONVERT(varchar(10), V.FechaVisita, 103) AS FechaVisita," +
                 "CONVERT(varchar(5), V.HoraInicio, 108) AS HoraCita, EC.Descripcion AS EstadoVisita ,CONVERT(varchar(5), V.CodigoProyecto, 103) AS CodigoProyecto," +
                 "CONVERT(varchar(5), V.CodigoGrupoVisita, 103) AS CodigoGrupoVisita,CONVERT(varchar(5), V.CodigoVisita, 103) AS CodigoVisita, CONVERT(varchar(5), " +
                 "V.CodigoVisitas, 103) AS CodigoVisitas, CASE WHEN FechaUpdEstado IS NULL THEN '' ELSE CONVERT(varchar(23), FechaUpdEstado, 120)  END as FechaUpdEstado " +
                 "FROM  VISITAS AS V INNER JOIN PROYECTO AS PY ON V.CodigoProyecto = PY.CodigoProyecto AND V.Estado = 1 " +
                 "INNER JOIN USUARIOS_PROYECTO AS UP ON UP.CodigoProyecto = V.CodigoProyecto " +
                 "INNER JOIN VISITA AS E ON V.CodigoProyecto = E.CodigoProyecto AND V.CodigoGrupoVisita = E.CodigoGrupoVisita AND V.CodigoVisita = E.CodigoVisita " +
                 "INNER JOIN PARAMETROS AS EC ON V.CodigoEstadoVisita = EC.CodigoParametro AND EC.Codigo = 5 " +
                 "WHERE V.CodigoPaciente = '" + CodigoPaciente + "' AND UP.CodigoUsuario = " + CodigoUsuario + " AND V.CodigoProyecto = " + CodigoProyecto;

            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            List<Visitas2> lista = new List<Visitas2>();

            while (reader.Read())
            {
                lista.Add(new Visitas2(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6),
                    reader.GetString(7),
                    reader.GetString(8),
                    reader.GetString(9)));
            }

            cn.Close();
            return lista.ToArray();
        }

        [WebMethod]
        public Visitas3[] ObtenerVisitasCanceladas(string codigoProyecto, string status)
        {
            SqlConnection cn = con.conexion();
            cn.Open();
            string sql = "SELECT TOP(30) PY.Nombre AS Proyecto, E.Nombre AS Visita, " +
                 "SUBSTRING(DATENAME(dw, V.FechaVisita), 1, 3) + ' ' + CONVERT(varchar(10), V.FechaVisita, 103) AS FechaVisita," +
                 "CONVERT(varchar(5), V.HoraInicio, 108) AS HoraCita, EC.Descripcion AS EstadoVisita ,CONVERT(varchar(5), V.CodigoProyecto, 103) AS CodigoProyecto," +
                 "CONVERT(varchar(5), V.CodigoGrupoVisita, 103) AS CodigoGrupoVisita,CONVERT(varchar(5), V.CodigoVisita, 103) AS CodigoVisita, CONVERT(varchar(5), " +
                 "V.CodigoVisitas, 103) AS CodigoVisitas, CASE WHEN FechaUpdEstado IS NULL THEN '' ELSE CONVERT(varchar(23), FechaUpdEstado, 120) END as FechaUpdEstado, " +
                 "V.CodigoPaciente AS CodigoPaciente, P.Nombres + ' ' + P.ApellidoMaterno + ' ' + P.ApellidoPaterno AS NombresPaciente " +
                 "FROM  VISITAS AS V INNER JOIN PROYECTO AS PY ON V.CodigoProyecto = PY.CodigoProyecto AND V.Estado = 1 " +
                 "INNER JOIN USUARIOS_PROYECTO AS UP ON UP.CodigoProyecto = V.CodigoProyecto " +
                 "INNER JOIN VISITA AS E ON V.CodigoProyecto = E.CodigoProyecto AND V.CodigoGrupoVisita = E.CodigoGrupoVisita AND V.CodigoVisita = E.CodigoVisita " +
                 "INNER JOIN PARAMETROS AS EC ON V.CodigoEstadoVisita = EC.CodigoParametro AND EC.Codigo = 5 " +
                 "LEFT JOIN PACIENTE AS P ON P.CodigoPaciente=V.CodigoPaciente" +
                 "WHERE V.CodigoProyecto = " + codigoProyecto + " AND V.CodigoEstadoVisita = " + status + " " +
                 "ORDER BY V.FechaUpdEstado DESC";

            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            List<Visitas3> lista = new List<Visitas3>();

            while (reader.Read())
            {
                lista.Add(new Visitas3(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6),
                    reader.GetString(7),
                    reader.GetString(8),
                    reader.GetString(9),
                    reader.GetString(10),
                    reader.GetString(11)));
            }

            cn.Close();
            return lista.ToArray();
        }

        #endregion 

    }





}

