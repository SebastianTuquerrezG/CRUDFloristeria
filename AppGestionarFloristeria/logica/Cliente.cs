using AppTiendaMascotas.accesoDatos;
using System;
using System.Data;

namespace AppTiendaMascotas.logica
{
    internal class Cliente
    {
        private Datos dt = new Datos();

        public int ingresarCliente(long cedulaDuenio, string nombreDuenio, long numTelefonoDuenio)
        {
            int resultado;
            //paso 1: construyo la sentencia sql para insertar
            string consulta = "INSERT INTO DUENIO (CEDULADUENIO,NOMBREDUENIO,NUMTELEFONODUENIO) VALUES (" +
                cedulaDuenio + ",'" + nombreDuenio + "'," + numTelefonoDuenio + ")";
            //paso 2: enviar la consulta a la capa de accesoDatos para ejecutarla
            resultado = dt.ejecutarDML(consulta);
            return resultado;
        }

        public int eliminarCliente(int cedulaCliente)
        {
            int resultado;
            //paso 1: construyo la sentencia sql para insertar
            string consulta = "DELETE FROM DUENIO WHERE CEDULADUENIO = " + cedulaCliente;
            resultado = dt.ejecutarDML(consulta);
            return resultado;
        }

        public DataSet consultarCliente()
        {
            DataSet rDT = new DataSet();
            string consulta;
            consulta = "SELECT CODIGOCLIENTE CEDULA, NOMBRECLIENTE NOMBRE FROM CLIENTE";
            rDT = dt.ejecutarSELECT(consulta);
            return rDT;
        }

        public DataSet buscar(string aux)
        {
            DataSet rDT = new DataSet();
            string consulta;
            consulta = "SELECT serialproducto ID, nombreproducto nombre, precioproducto informacion FROM producto WHERE lower(nombreproducto) like '%" + aux + "%' UNION SELECT codempleado, nombreempleado, salarioempleado FROM empleado WHERE lower(nombreempleado) like '%" + aux + "%' UNION SELECT ceduladuenio, nombreduenio, numtelefonoduenio FROM duenio WHERE lower(nombreduenio) like '%" + aux + "%' UNION SELECT idmascota, nombremascota, ceduladuenio FROM mascota WHERE lower(nombremascota) like '%" + aux + "%'";
            rDT = dt.ejecutarSELECT(consulta);
            return rDT;
        }

        public DataSet consultarClienteMenu()
        {
            DataSet rDT = new DataSet();
            string consulta;
            consulta = "SELECT CODIGOCLIENTE CEDULA, NOMBRECLIENTE NOMBRE FROM CLIENTE";
            rDT = dt.ejecutarSELECT(consulta);
            return rDT;
        }

        public int consultarCantidadClientes()
        {
            int cantidadClientes = 0;
            DataSet rDT = new DataSet();
            string consulta = "SELECT COUNT(CODIGOCLIENTE) FROM CLIENTE";
            rDT = dt.ejecutarSELECT(consulta);
            if (rDT.Tables[0].Rows.Count > 0)
            {
                cantidadClientes = Convert.ToInt32(rDT.Tables[0].Rows[0][0]);
            }
            return cantidadClientes;
        }


        public DataTable consultarClienteIDs()
        {
            DataSet mids = new DataSet();
            string consulta;
            consulta = "SELECT CEDULADUENIO, NOMBREDUENIO FROM DUENIO";
            mids = dt.ejecutarSELECT(consulta);
            DataTable dta = mids.Tables[0];
            return dta;
        }
    }
}