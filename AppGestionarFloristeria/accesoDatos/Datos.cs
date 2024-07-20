using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace AppTiendaMascotas.accesoDatos
{
    internal class Datos
    {
        public static string cadenaConexion = "";

        // Método que ejecuta una instrucción DML usando parámetros
        public int ejecutarDML(string consulta, params MySqlParameter[] parametros)
        {
            int filasAfectadas = 0;
            using (MySqlConnection miConexion = new MySqlConnection(cadenaConexion))
            {
                using (MySqlCommand miComando = new MySqlCommand(consulta, miConexion))
                {
                    miComando.Parameters.AddRange(parametros);
                    try
                    {
                        miConexion.Open();
                        filasAfectadas = miComando.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ocurrió un error con la base de datos: " + ex.Message);
                    }
                }
            }
            return filasAfectadas;
        }

        public void setCadenaConexion(string userId, string hostName, string portNumber, string password, string database)
        {
            cadenaConexion = $"Server={hostName};Port={portNumber};Database={database};User Id={userId};Password={password};";
        }

        public string getCadenaConexion()
        {
            return cadenaConexion;
        }

        public DataSet ejecutarSELECT(string consulta, params MySqlParameter[] parametros)
        {
            DataSet ds = new DataSet();
            using (MySqlConnection miConexion = new MySqlConnection(cadenaConexion))
            {
                using (MySqlDataAdapter miAdaptador = new MySqlDataAdapter(consulta, miConexion))
                {
                    miAdaptador.SelectCommand.Parameters.AddRange(parametros);
                    miAdaptador.Fill(ds, "ResultadoDatos");
                }
            }
            return ds;
        }

        public int ConsultarIngXEmpleado(int codEmpleado, DateTime fechaInicio, DateTime fechaFin)
        {
            int total = 0;
            using (MySqlConnection connection = new MySqlConnection(getCadenaConexion()))
            {
                try
                {
                    connection.Open();
                    string consulta = "SELECT total_ingresos_empleado(@codEmpleado, @fechaInicio, @fechaFin)";
                    using (MySqlCommand command = new MySqlCommand(consulta, connection))
                    {
                        command.Parameters.AddWithValue("@codEmpleado", codEmpleado);
                        command.Parameters.AddWithValue("@fechaInicio", fechaInicio.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@fechaFin", fechaFin.ToString("yyyy-MM-dd"));
                        total = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error con la base de datos: " + ex.Message);
                }
            }
            return total;
        }

        public DataSet ConsultarResidenciasXAlojamiento(string tipoResidencia)
        {
            DataSet ds = new DataSet();
            using (MySqlConnection connection = new MySqlConnection(getCadenaConexion()))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand("pr_verificacion_residencias", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_tipo_residencia", tipoResidencia);
                        MySqlDataAdapter myAdapter = new MySqlDataAdapter(command);
                        myAdapter.Fill(ds, "ResultadoDatos");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error con la base de datos: " + ex.Message);
                }
            }
            return ds;
        }

        public DataSet ConsultarListarEmpleados(DateTime fechaInicio, DateTime fechaFin)
        {
            DataSet ds = new DataSet();
            using (MySqlConnection connection = new MySqlConnection(getCadenaConexion()))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand("listar_empleados", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_fechaInicio", fechaInicio);
                        command.Parameters.AddWithValue("@p_fechaFin", fechaFin);
                        MySqlDataAdapter myAdapter = new MySqlDataAdapter(command);
                        myAdapter.Fill(ds, "ResultadoDatos");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error con la base de datos: " + ex.Message);
                }
            }
            return ds;
        }
    }
}
