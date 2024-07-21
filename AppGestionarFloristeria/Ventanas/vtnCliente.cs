using AppTiendaMascotas.logica;
using System;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Windows.Forms;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using MySql.Data.MySqlClient;
using AppTiendaMascotas.accesoDatos;

namespace AppTiendaMascotas.Ventanas
{
    public partial class vtnCliente : Form
    {
        private Datos datos = new Datos();
        private Cliente cliente = new Cliente();
        private DataTable dataTable;

        public vtnCliente()
        {
            InitializeComponent();
            style();
            informacion();
        }

        private Boolean bandera = true;
        private Boolean bandera2 = false;

        private void informacion()
        {
            // Usamos Invoke para asegurarnos de que la actualización se realiza en el hilo de la UI
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(informacion));
                return;
            }

            cbxIdClienteDelete.DataSource = cliente.consultarClienteIDs();
            cbxIdClienteDelete.DisplayMember = "NOMBRECLIENTE";
            cbxIdClienteDelete.ValueMember = "CODIGOCLIENTE";

            string connectionString = datos.getCadenaConexion();
            using (var connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM CLIENTE";
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, connection);
                dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dgvConsultaClientes.DataSource = dataTable;
                dgvConsultaClientes.AllowUserToAddRows = false;
                dgvConsultaClientes.AllowUserToDeleteRows = false;
                dgvConsultaClientes.ReadOnly = false;
            }

        }

        private void style()
        {
            dgvConsultaClientes.Region = new System.Drawing.Region(CreateRoundedRectangle(dgvConsultaClientes.Width, dgvConsultaClientes.Height));

            txtCorreoCliente.Anchor = AnchorStyles.Top;
            txtNumTele.Anchor = AnchorStyles.Top;
            txtNombreC.Anchor = AnchorStyles.Top;
            cbxIdClienteDelete.Anchor = AnchorStyles.Top;
            pictureBox1.Anchor = AnchorStyles.Top;
            btnEliminarCliente.Anchor = AnchorStyles.Top;
            btnGuardarC.Anchor = AnchorStyles.Top;
            dgvConsultaClientes.Anchor = AnchorStyles.Top;
            pictureBox2.Anchor = AnchorStyles.Top;
            pictureBox3.Anchor = AnchorStyles.Top;
            label1.Anchor = AnchorStyles.Top;
            label2.Anchor = AnchorStyles.Top;
            label3.Anchor = AnchorStyles.Top;
            label4.Anchor = AnchorStyles.Top;
            label5.Anchor = AnchorStyles.Top;
            label6.Anchor = AnchorStyles.Top;
            label7.Anchor = AnchorStyles.Top;
        }

        private System.Drawing.Drawing2D.GraphicsPath CreateRoundedRectangle(int buttonWidth, int buttonHeight)
        {
            System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
            int cornerRadius = 20;
            buttonPath.AddArc(0, 0, cornerRadius, cornerRadius, 180, 90);
            buttonPath.AddArc(buttonWidth - cornerRadius, 0, cornerRadius, cornerRadius, 270, 90);
            buttonPath.AddArc(buttonWidth - cornerRadius, buttonHeight - cornerRadius, cornerRadius, cornerRadius, 0, 90);
            buttonPath.AddArc(0, buttonHeight - cornerRadius, cornerRadius, cornerRadius, 90, 90);
            buttonPath.CloseFigure();
            return buttonPath;
        }

        private void scrollBar()
        {
            if (this.VerticalScroll.Visible && bandera)
            {
                bandera = false;
                bandera2 = true;
                label1.Location = new Point(label1.Location.X + 1, label1.Location.Y);
                label2.Location = new Point(label2.Location.X + 1, label2.Location.Y);
                label3.Location = new Point(label3.Location.X + 1, label3.Location.Y);
                label4.Location = new Point(label4.Location.X + 1, label4.Location.Y);
                label5.Location = new Point(label5.Location.X + 1, label5.Location.Y);
                label6.Location = new Point(label6.Location.X + 1, label6.Location.Y);
                label7.Location = new Point(label7.Location.X + 1, label7.Location.Y);
                dgvConsultaClientes.Location = new Point(dgvConsultaClientes.Location.X + 1, dgvConsultaClientes.Location.Y);
            }
            else if (!this.VerticalScroll.Visible && bandera2)
            {
                bandera = true;
                bandera2 = false;
                label1.Location = new Point(label1.Location.X - 2, label1.Location.Y);
                label2.Location = new Point(label2.Location.X - 2, label2.Location.Y);
                label3.Location = new Point(label3.Location.X - 2, label3.Location.Y);
                label4.Location = new Point(label4.Location.X - 2, label4.Location.Y);
                label5.Location = new Point(label5.Location.X - 2, label5.Location.Y);
                label6.Location = new Point(label6.Location.X - 2, label6.Location.Y);
                label7.Location = new Point(label7.Location.X - 2, label7.Location.Y);
                dgvConsultaClientes.Location = new Point(dgvConsultaClientes.Location.X - 2, dgvConsultaClientes.Location.Y);
            }
        }

        private void vtnResidencia_Resize(object sender, EventArgs e)
        {
            scrollBar();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (txtCorreoCliente.Text.Equals("") || txtNumTele.Text.Equals("") || txtNombreC.Text.Equals(""))
            {
                MessageBox.Show("Hay espacios vacios", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int resultado;
            string nombreCliente, correoCliente, numTelefono;
            DateTime dateTime;

            try
            {
                numTelefono = txtNumTele.Text;
                nombreCliente = txtNombreC.Text;
                correoCliente = txtCorreoCliente.Text;
                dateTime = timeFechaCliente.Value;
                resultado = cliente.ingresarCliente(nombreCliente, correoCliente, numTelefono, dateTime);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Formato incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (resultado > 0)
            {
                informacion();
                MessageBox.Show("Cliente registrado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCorreoCliente.Text = "";
                txtNumTele.Text = "";
                txtNombreC.Text = "";
            }
            else
            {
                MessageBox.Show("Cliente no registrado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (cbxIdClienteDelete.Text.Equals(""))
            {
                MessageBox.Show("Hay espacios vacios", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int idCliente, resultado;

            try
            {
                idCliente = Convert.ToInt32(cbxIdClienteDelete.SelectedValue);
                resultado = cliente.eliminarCliente(idCliente);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Formato incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (resultado > 0)
            {
                informacion();
                MessageBox.Show("Cliente eliminado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Cliente no eliminado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardarChange_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    int idCliente = Convert.ToInt32(row["CODIGOCLIENTE"]);
                    string nombreCliente = row["NOMBRECLIENTE"].ToString();
                    string correoCliente = row["CORREOCLIENTE"].ToString();
                    string telefonoCliente = row["TELEFONOCLIENTE"].ToString();
                    DateTime fechaNacimientoCliente = Convert.ToDateTime(row["FECHANACIMIENTOCLIENTE"]);

                    cliente.actualizarCliente(idCliente, nombreCliente, correoCliente, telefonoCliente, fechaNacimientoCliente);
                }

                MessageBox.Show("Cambios guardados exitosamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar los cambios: " + ex.Message);
            }
        }
    }
}
