using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace AppTiendaMascotas.accesoDatos
{
    internal class Datos
    {
        public static string cadenaConexion = "";

        // Método que crea una instrucción DML
        public int ejecutarDML(string consulta)
        {
            int filasAfectadas = 0;
            MySqlConnection miConexion = new MySqlConnection(cadenaConexion);
            MySqlCommand miComando = new MySqlCommand(consulta, miConexion);
            try
            {
                miConexion.Open();
                filasAfectadas = miComando.ExecuteNonQuery();
                miConexion.Close();
                return filasAfectadas;
            }
            catch (Exception ex)
            {
                miConexion.Close();
                MessageBox.Show("Ocurrió un error con la base de datos: " + ex.Message);
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

        public DataSet ejecutarSELECT(string consulta)
        {
            // Paso 1: Crear un DataSet vacío
            DataSet ds = new DataSet();
            // Paso 2: Crear un adaptador
            MySqlDataAdapter miAdaptador = new MySqlDataAdapter(consulta, cadenaConexion);
            // Paso 3: Llenar el DataSet a través del adaptador
            miAdaptador.Fill(ds, "ResultadoDatos");
            return ds;
        }

        public int ConsultarIngXEmpleado(int codEmpleado, DateTime fechaInicio, DateTime fechaFin)
        {
            int total = 0;
            MySqlConnection connection = new MySqlConnection(getCadenaConexion());
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
                    Console.WriteLine("Result: " + total);
                }
                return total;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error con la base de datos: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return total;
        }

        public DataSet ConsultarResidenciasXAlojamiento(string tipoResidencia)
        {
            DataSet ds = new DataSet();
            MySqlConnection connection = new MySqlConnection(getCadenaConexion());
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "pr_verificacion_residencias";
                command.Parameters.AddWithValue("@p_tipo_residencia", tipoResidencia);
                command.CommandType = CommandType.StoredProcedure;
                MySqlDataAdapter myAdapter = new MySqlDataAdapter(command);
                myAdapter.Fill(ds, "ResultadoDatos");
                return ds;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error con la base de datos: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return ds;
        }

        public DataSet ConsultarListarEmpleados(DateTime fechaInicio, DateTime fechaFin)
        {
            DataSet ds = new DataSet();
            MySqlConnection connection = new MySqlConnection(getCadenaConexion());
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "listar_empleados";
                // Agregar los parámetros de entrada al objeto MySqlCommand
                command.Parameters.AddWithValue("@p_fechaInicio", fechaInicio);
                command.Parameters.AddWithValue("@p_fechaFin", fechaFin);
                command.CommandType = CommandType.StoredProcedure;
                MySqlDataAdapter myAdapter = new MySqlDataAdapter(command);
                myAdapter.Fill(ds, "ResultadoDatos");
                return ds;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error con la base de datos: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return ds;
        }
    }
}
