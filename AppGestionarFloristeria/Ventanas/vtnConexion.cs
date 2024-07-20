using AppTiendaMascotas.accesoDatos;
using System;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace AppTiendaMascotas
{
    public partial class vtnConexion : Form
    {
        public vtnConexion()
        {
            InitializeComponent();
        }

        private Datos accesoDatos = new Datos();

        private void Tienda_Load(object sender, EventArgs e)
        {
            style();
        }

        private void style()
        {
            btnConectar.Region = new System.Drawing.Region(CreateRoundedRectangle(btnConectar.Width, btnConectar.Height));
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


        private void btnConectar_Click(object sender, EventArgs e)
        {
            string nombreUsuario, nombreHost, numeroPuerto, contrasenia, nombreBaseDeDatos;
            nombreUsuario = txtNombreUsuario.Text;
            nombreHost = txtNombreHost.Text;
            numeroPuerto = txtNumeroPuerto.Text;
            contrasenia = txtContrasenia.Text;
            nombreBaseDeDatos = "euroflor";
            accesoDatos.setCadenaConexion(nombreUsuario, nombreHost, numeroPuerto, contrasenia, nombreBaseDeDatos);
            using (var conn = new MySqlConnection(accesoDatos.getCadenaConexion()))
            {
                try
                {
                    conn.Open();
                    Form aux = new Tienda();
                    MessageBox.Show("Conexión Exitosa", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    aux.ShowDialog();
                    this.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

    }
}
