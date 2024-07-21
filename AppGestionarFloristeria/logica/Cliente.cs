using AppTiendaMascotas.accesoDatos;
using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace AppTiendaMascotas.logica
{
    internal class Cliente
    {
        private Datos dt = new Datos();

        public int ingresarCliente(string nombreDuenio, string correoCliente, string numTelefonoDuenio, DateTime fechaNacimientoDuenio)
        {
            int resultado;
            string consulta = "INSERT INTO CLIENTE (NOMBRECLIENTE, CORREOCLIENTE, TELEFONOCLIENTE, FECHANACIMIENTOCLIENTE) VALUES (@nombreDuenio, @correoCliente, @numTelefonoDuenio, @fechaNacimientoDuenio)";

            MySqlParameter[] parametros = {
                new MySqlParameter("@nombreDuenio", nombreDuenio),
                new MySqlParameter("@correoCliente", correoCliente),
                new MySqlParameter("@numTelefonoDuenio", numTelefonoDuenio),
                new MySqlParameter("@fechaNacimientoDuenio", fechaNacimientoDuenio.ToString("yyyy-MM-dd"))
            };

            resultado = dt.ejecutarDML(consulta, parametros);
            return resultado;
        }

        public int eliminarCliente(int cedulaCliente)
        {
            int resultado;
            string consulta = "DELETE FROM CLIENTE WHERE CODIGOCLIENTE = @cedulaCliente";
            MySqlParameter[] parametros = {
                new MySqlParameter("@cedulaCliente", cedulaCliente)
            };

            resultado = dt.ejecutarDML(consulta, parametros);
            return resultado;
        }

        public DataSet consultarCliente()
        {
            string consulta = "SELECT CODIGOCLIENTE CEDULA, NOMBRECLIENTE NOMBRE FROM CLIENTE";
            return dt.ejecutarSELECT(consulta);
        }

        public DataSet buscar(string aux)
        {
            string consulta = "SELECT CODIGOCLIENTE, NOMBRECLIENTE, CORREOCLIENTE, TELEFONOCLIENTE, FECHANACIMIENTOCLIENTE FROM CLIENTE WHERE lower(NOMBRECLIENTE) like @aux UNION SELECT CODIGOVENTA, CODIGOCLIENTE, FECHAVENTA, PRODUCTOVENTA, PRECIOVENTA, MENSAJEVENTA FROM VENTA WHERE lower(PRODUCTOVENTA) like @aux";
            MySqlParameter[] parametros = {
                new MySqlParameter("@aux", "%" + aux.ToLower() + "%")
            };
            return dt.ejecutarSELECT(consulta, parametros);
        }

        public DataSet consultarClienteMenu()
        {
            string consulta = "SELECT CODIGOCLIENTE CEDULA, NOMBRECLIENTE NOMBRE FROM CLIENTE";
            return dt.ejecutarSELECT(consulta);
        }

        public int consultarCantidadClientes()
        {
            int cantidadClientes = 0;
            string consulta = "SELECT COUNT(CODIGOCLIENTE) FROM CLIENTE";
            DataSet rDT = dt.ejecutarSELECT(consulta);
            if (rDT.Tables[0].Rows.Count > 0)
            {
                cantidadClientes = Convert.ToInt32(rDT.Tables[0].Rows[0][0]);
            }
            return cantidadClientes;
        }

        public DataTable consultarClienteIDs()
        {
            string consulta = "SELECT CODIGOCLIENTE, NOMBRECLIENTE FROM CLIENTE";
            DataSet mids = dt.ejecutarSELECT(consulta);
            return mids.Tables[0];
        }

        public DataSet consultarClientesCumpleañosHoy()
        {
            // Obtener la fecha actual
            DateTime today = DateTime.Now;
            // Formatear el mes y el día para la consulta
            string todayMonthDay = today.ToString("MM-dd");

            // Consulta SQL para seleccionar clientes cuyo mes y día de nacimiento coincidan con la fecha actual
            string consulta = @"
            SELECT CODIGOCLIENTE CLIENTE, NOMBRECLIENTE NOMBRE, CORREOCLIENTE CORREO, TELEFONOCLIENTE TELEFONO
            FROM CLIENTE
            WHERE DATE_FORMAT(FECHANACIMIENTOCLIENTE, '%m-%d') = @todayMonthDay";

                MySqlParameter[] parametros = {
            new MySqlParameter("@todayMonthDay", todayMonthDay)
            };

            return dt.ejecutarSELECT(consulta, parametros);
        }
    }
}
