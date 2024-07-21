using AppTiendaMascotas.logica;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AppTiendaMascotas.Ventanas
{
    public partial class vtnVentaXCliente : Form
    {
        public vtnVentaXCliente()
        {
            InitializeComponent();
            style();
            informacion();
            dgvConsultaClientes.CellClick += new DataGridViewCellEventHandler(dgvConsultaClientes_CellClick);
            dgvConsultaClientes.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgvConsultaClientes_DataBindingComplete);
        }

        private Boolean bandera = true;
        private Boolean bandera2 = false;
        private Cliente cliente = new Cliente();

        private void informacion()
        {
            // Usamos Invoke para asegurarnos de que la actualización se realiza en el hilo de la UI
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(informacion));
                return;
            }

            DataTable clientes = cliente.consultarClienteIDs();
            cbxIdCliente.DataSource = clientes;
            cbxIdCliente.DisplayMember = "CODIGOCLIENTE";
            cbxIdCliente.ValueMember = "CODIGOCLIENTE";

            cbxNombreCliente.DataSource = clientes;
            cbxNombreCliente.DisplayMember = "NOMBRECLIENTE";
            cbxNombreCliente.ValueMember = "CODIGOCLIENTE";
        }

        private void style()
        {
            dgvConsultaClientes.Region = new System.Drawing.Region(CreateRoundedRectangle(dgvConsultaClientes.Width, dgvConsultaClientes.Height));

            cbxIdCliente.Anchor = AnchorStyles.Top;
            cbxNombreCliente.Anchor = AnchorStyles.Top;
            btnBuscarCliente.Anchor = AnchorStyles.Top;
            dgvConsultaClientes.Anchor = AnchorStyles.Top;
            label1.Anchor = AnchorStyles.Top;
            label2.Anchor = AnchorStyles.Top;

            // Configurar el modo de ajuste automático de filas
            dgvConsultaClientes.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
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
                dgvConsultaClientes.Location = new Point(dgvConsultaClientes.Location.X + 1, dgvConsultaClientes.Location.Y);
            }
            else if (!this.VerticalScroll.Visible && bandera2)
            {
                bandera = true;
                bandera2 = false;
                label1.Location = new Point(label1.Location.X - 2, label1.Location.Y);
                label2.Location = new Point(label2.Location.X - 2, label2.Location.Y);
                dgvConsultaClientes.Location = new Point(dgvConsultaClientes.Location.X - 2, dgvConsultaClientes.Location.Y);
            }
        }

        private void vtnResidencia_Resize(object sender, EventArgs e)
        {
            scrollBar();
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            int idCliente;
            if (int.TryParse(cbxIdCliente.SelectedValue.ToString(), out idCliente))
            {
                DataSet dsVentas = cliente.consultarVentasPorCliente(idCliente);

                DataTable dataTable = dsVentas.Tables[0];

                DataTable displayTable = new DataTable();
                displayTable.Columns.Add("VENTA");
                displayTable.Columns.Add("FECHA");
                displayTable.Columns.Add("PRODUCTO");
                displayTable.Columns.Add("PRECIO", typeof(decimal));
                displayTable.Columns.Add("MENSAJE");
                displayTable.Columns.Add("FOTO", typeof(Image));

                foreach (DataRow row in dataTable.Rows)
                {
                    DataRow newRow = displayTable.NewRow();
                    newRow["VENTA"] = row["VENTA"];
                    newRow["FECHA"] = row["FECHA"];
                    newRow["PRODUCTO"] = row["PRODUCTO"];
                    newRow["PRECIO"] = row["PRECIO"];
                    newRow["MENSAJE"] = row["MENSAJE"];
                    // Verificar si el valor es DBNull
                    if (row["FOTO"] != DBNull.Value)
                    {
                        byte[] fotoBytes = (byte[])row["FOTO"];
                        newRow["FOTO"] = ByteArrayToImage(fotoBytes);
                    }
                    else
                    {
                        newRow["FOTO"] = null;
                    }

                    displayTable.Rows.Add(newRow);
                }
                dgvConsultaClientes.DataSource = displayTable;

                // Agregar la columna de imagen si no existe
                if (!dgvConsultaClientes.Columns.Contains("FOTO"))
                {
                    DataGridViewImageColumn imageColumn = new DataGridViewImageColumn
                    {
                        Name = "FOTO",
                        HeaderText = "FOTO",
                        ImageLayout = DataGridViewImageCellLayout.Zoom
                    };
                    dgvConsultaClientes.Columns.Add(imageColumn);
                }

                // Configurar columnas específicas
                dgvConsultaClientes.Columns["MENSAJE"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgvConsultaClientes.Columns["PRODUCTO"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgvConsultaClientes.Columns["FECHA"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                dgvConsultaClientes.Columns["MENSAJE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgvConsultaClientes.Columns["PRODUCTO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgvConsultaClientes.Columns["FECHA"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                // Configurar la columna PRECIO para mostrar el símbolo $
                dgvConsultaClientes.Columns["PRECIO"].DefaultCellStyle.Format = "$ #,##0.00";

                // Ajustar el ancho de las columnas para mostrar todo el texto
                dgvConsultaClientes.Columns["VENTA"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dgvConsultaClientes.Columns["FECHA"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvConsultaClientes.Columns["PRODUCTO"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvConsultaClientes.Columns["PRECIO"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dgvConsultaClientes.Columns["MENSAJE"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                // Ajustar la altura de las filas
                dgvConsultaClientes.RowTemplate.Height = 100; // Ajusta el valor a lo que necesites
                dgvConsultaClientes.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);

                dgvConsultaClientes.AllowUserToAddRows = false; // Impide que el usuario agregue nuevas filas
                dgvConsultaClientes.AllowUserToDeleteRows = false; // Impide que el usuario elimine filas
                dgvConsultaClientes.ReadOnly = false; // Permite la edición

            }
            else
            {
                MessageBox.Show("Seleccione un cliente válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Image ByteArrayToImage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
                return null;
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }
        private void dgvConsultaClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvConsultaClientes.Columns["FOTO"].Index && e.RowIndex >= 0)
            {
                Image img = dgvConsultaClientes.Rows[e.RowIndex].Cells["FOTO"].Value as Image;
                if (img != null)
                {
                    Form imageForm = new Form();
                    PictureBox pictureBox = new PictureBox
                    {
                        Dock = DockStyle.Fill,
                        Image = img,
                        SizeMode = PictureBoxSizeMode.Zoom
                    };
                    imageForm.Controls.Add(pictureBox);
                    imageForm.Size = new Size(800, 600);
                    imageForm.ShowDialog();
                }
            }
        }

        private void dgvConsultaClientes_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Asegurarse de que la columna "PRECIO" esté configurada correctamente
            if (dgvConsultaClientes.Columns.Contains("PRECIO"))
            {
                dgvConsultaClientes.Columns["PRECIO"].DefaultCellStyle.Format = "$ #,##0.00";
            }

            if (dgvConsultaClientes.Columns.Contains("FOTO"))
            {
                dgvConsultaClientes.Columns["FOTO"].Width = 100;
            }

            // Ajustar el ancho de las columnas para mostrar todo el texto
            dgvConsultaClientes.Columns["VENTA"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgvConsultaClientes.Columns["FECHA"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvConsultaClientes.Columns["PRODUCTO"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvConsultaClientes.Columns["PRECIO"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgvConsultaClientes.Columns["MENSAJE"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // Ajustar la altura de las filas
            dgvConsultaClientes.RowTemplate.Height = 100; // Ajusta el valor a lo que necesites
            dgvConsultaClientes.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
        }


    }
}
