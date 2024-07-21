using AppTiendaMascotas.accesoDatos;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace AppTiendaMascotas.logica
{
    internal class Venta
    {
        private Datos dt = new Datos();

        // Método para ingresar una venta
        public int ingresarVenta(int idCliente, string descVenta, int precioVenta, string mensajeVenta, DateTime fechaVenta, byte[] fotoVenta)
        {
            int resultado;
            string consulta = "INSERT INTO Venta (CodigoCliente, FechaVenta, ProductoVenta, PrecioVenta, MensajeVenta, FotoVenta) VALUES " +
                              "(@CodigoCliente, @FechaVenta, @ProductoVenta, @PrecioVenta, @MensajeVenta, @FotoVenta)";

            MySqlParameter[] parametros = new MySqlParameter[]
            {
                new MySqlParameter("@CodigoCliente", idCliente),
                new MySqlParameter("@FechaVenta", fechaVenta),
                new MySqlParameter("@ProductoVenta", descVenta),
                new MySqlParameter("@PrecioVenta", precioVenta),
                new MySqlParameter("@MensajeVenta", mensajeVenta ?? (object)DBNull.Value),
                new MySqlParameter("@FotoVenta", fotoVenta ?? (object)DBNull.Value)
            };

            resultado = dt.ejecutarDML(consulta, parametros);
            return resultado;
        }

        public int actualizarVenta(int idVenta, int idCliente, string descVenta, double precioVenta, string mensajeVenta, DateTime fechaVenta, byte[] fotoVenta)
        {
            string consulta = "UPDATE Venta SET CodigoCliente = @CodigoCliente, FechaVenta = @FechaVenta, ProductoVenta = @ProductoVenta, PrecioVenta = @PrecioVenta, MensajeVenta = @MensajeVenta, FotoVenta = @FotoVenta WHERE CodigoVenta = @CodigoVenta";

            MySqlParameter[] parametros = new MySqlParameter[]
            {
                new MySqlParameter("@CodigoVenta", idVenta),
                new MySqlParameter("@CodigoCliente", idCliente),
                new MySqlParameter("@FechaVenta", fechaVenta),
                new MySqlParameter("@ProductoVenta", descVenta),
                new MySqlParameter("@PrecioVenta", precioVenta),
                new MySqlParameter("@MensajeVenta", mensajeVenta ?? (object)DBNull.Value),
                new MySqlParameter("@FotoVenta", fotoVenta ?? (object)DBNull.Value)
            };

            return dt.ejecutarDML(consulta, parametros);
        }

        public int eliminarVenta(int idVenta)
        {
            int resultado;
            string consulta = "DELETE FROM Venta WHERE CodigoVenta = @CodigoVenta";
            MySqlParameter[] parametros = new MySqlParameter[]
            {
                new MySqlParameter("@CodigoVenta", idVenta)
            };
            resultado = dt.ejecutarDML(consulta, parametros);
            return resultado;
        }

        public DataSet consultarVentas()
        {
            string consulta = "SELECT CodigoVenta as VENTA, CodigoCliente as CLIENTE, FechaVenta as FECHA, ProductoVenta as PRODUCTO, PrecioVenta as PRECIO, MensajeVenta as MENSAJE, FotoVenta as FOTO FROM Venta";
            return dt.ejecutarSELECT(consulta);
        }


        public DataTable consultarVentaIDs()
        {
            string consulta = "SELECT CodigoVenta FROM Venta";
            DataSet ds = dt.ejecutarSELECT(consulta);
            return ds.Tables["ResultadoDatos"];
        }

        public DataSet valorVenta(string nombreProducto)
        {
            string consulta = "SELECT PrecioProducto FROM Producto WHERE NombreProducto = @NombreProducto";
            MySqlParameter[] parametros = new MySqlParameter[]
            {
                new MySqlParameter("@NombreProducto", nombreProducto)
            };
            return dt.ejecutarSELECT(consulta, parametros);
        }
    }
}
