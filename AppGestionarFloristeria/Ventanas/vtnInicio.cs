using AppTiendaMascotas.logica;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace AppTiendaMascotas.Ventanas
{
    public partial class vtnInicio : Form
    {
        public vtnInicio()
        {
            InitializeComponent();
            style();
            informacion();
        }

        private Venta venta = new Venta();
        private Cliente cliente = new Cliente();
        private Boolean bandera = true;
        private Boolean bandera2 = false;

        private void informacion()
        {
            DataSet dsResultado = new DataSet();

            // Consultar cuántos clientes cumplen años hoy
            dsResultado = cliente.consultarClienteMenu();
            dataGridClientes.DataSource = dsResultado;
            dataGridClientes.DataMember = "ResultadoDatos";

            // Consultar cuántos clientes hay registrados
            int cantidadClientes = cliente.consultarCantidadClientes();
            lblClientesRegistrados.Text = cantidadClientes.ToString();
        }


        private void style()
        {
            dataGridClientes.Region = new System.Drawing.Region(CreateRoundedRectangle(dataGridClientes.Width, dataGridClientes.Height));

            lblClientesRegistrados.Anchor = AnchorStyles.Top;
            dataGridClientes.Anchor = AnchorStyles.Top;
            label1.Anchor = AnchorStyles.Top;
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
                lblClientesRegistrados.Location = new Point(lblClientesRegistrados.Location.X + 1, lblClientesRegistrados.Location.Y);
                label1.Location = new Point(label1.Location.X + 1, label1.Location.Y);
                dataGridClientes.Location = new Point(dataGridClientes.Location.X + 1, dataGridClientes.Location.Y);
                dataGridClientes.Location = new Point(dataGridClientes.Location.X + 1, dataGridClientes.Location.Y);
            }
            else if (!this.VerticalScroll.Visible && bandera2)
            {
                bandera = true;
                bandera2 = false;
                lblClientesRegistrados.Location = new Point(lblClientesRegistrados.Location.X - 2, lblClientesRegistrados.Location.Y);
                label1.Location = new Point(label1.Location.X - 2, label1.Location.Y);
                dataGridClientes.Location = new Point(dataGridClientes.Location.X - 2, dataGridClientes.Location.Y);
                dataGridClientes.Location = new Point(dataGridClientes.Location.X - 2, dataGridClientes.Location.Y);
            }
        }

        private void vtnInicio_Resize(object sender, EventArgs e)
        {
            scrollBar();
        }
    }
}