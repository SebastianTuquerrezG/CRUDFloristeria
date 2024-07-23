using AppTiendaMascotas.logica;
using MySqlX.XDevAPI;
using OfficeOpenXml;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AppTiendaMascotas.Ventanas
{
    public partial class vtnVenta : Form
    {
        public vtnVenta()
        {
            InitializeComponent();
            style();
            informacion();
            dgvConsultaVenta.CellClick += new DataGridViewCellEventHandler(dgvConsultaVenta_CellClick);
            dgvConsultaVenta.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgvConsultaVenta_DataBindingComplete);
        }

        private Boolean bandera = true;
        private Boolean bandera2 = false;
        private Cliente cli = new Cliente();
        private Venta vent = new Venta();
        private DataTable dataTable;

        private void informacion()
        {
            // Configurar los combo boxes
            cbxCliente.DataSource = cli.consultarClienteIDs();
            cbxCliente.DisplayMember = "NOMBRECLIENTE";
            cbxCliente.ValueMember = "CODIGOCLIENTE";

            cbxIdVentaDelete.DataSource = vent.consultarVentaIDs();
            cbxIdVentaDelete.DisplayMember = "CODIGOVENTA";
            cbxIdVentaDelete.ValueMember = "CODIGOVENTA";

            // Obtener los datos de ventas
            DataSet dsResultado = vent.consultarVentas();

            // Convertir el DataSet a DataTable
            dataTable = dsResultado.Tables["ResultadoDatos"]; // Asignar a la variable de instancia

            // Crear una nueva DataTable para el DataGridView
            DataTable displayTable = new DataTable();
            displayTable.Columns.Add("VENTA");
            displayTable.Columns.Add("CLIENTE");
            displayTable.Columns.Add("FECHA");
            displayTable.Columns.Add("PRODUCTO");
            displayTable.Columns.Add("PRECIO", typeof(decimal));
            displayTable.Columns.Add("MENSAJE");
            displayTable.Columns.Add("FOTO", typeof(Image));

            foreach (DataRow row in dataTable.Rows)
            {
                DataRow newRow = displayTable.NewRow();
                newRow["VENTA"] = row["VENTA"];
                newRow["CLIENTE"] = row["CLIENTE"];
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

            dgvConsultaVenta.DataSource = displayTable;

            // Agregar la columna de imagen si no existe
            if (!dgvConsultaVenta.Columns.Contains("FOTO"))
            {
                DataGridViewImageColumn imageColumn = new DataGridViewImageColumn
                {
                    Name = "FOTO",
                    HeaderText = "FOTO",
                    ImageLayout = DataGridViewImageCellLayout.Zoom
                };
                dgvConsultaVenta.Columns.Add(imageColumn);
            }

            // Configurar columnas específicas
            dgvConsultaVenta.Columns["MENSAJE"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvConsultaVenta.Columns["PRODUCTO"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvConsultaVenta.Columns["FECHA"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dgvConsultaVenta.Columns["MENSAJE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvConsultaVenta.Columns["PRODUCTO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvConsultaVenta.Columns["FECHA"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // Configurar la columna PRECIO para mostrar el símbolo $
            dgvConsultaVenta.Columns["PRECIO"].DefaultCellStyle.Format = "$ #,##0.00";

            // Ajustar el ancho de las columnas para mostrar todo el texto
            dgvConsultaVenta.Columns["VENTA"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvConsultaVenta.Columns["CLIENTE"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvConsultaVenta.Columns["FECHA"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvConsultaVenta.Columns["PRODUCTO"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvConsultaVenta.Columns["PRECIO"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvConsultaVenta.Columns["MENSAJE"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // Ajustar la altura de las filas
            dgvConsultaVenta.RowTemplate.Height = 40; // Ajusta el valor a lo que necesites
            dgvConsultaVenta.Refresh();
            dgvConsultaVenta.AllowUserToAddRows = false; // Impide que el usuario agregue nuevas filas
            dgvConsultaVenta.AllowUserToDeleteRows = false; // Impide que el usuario elimine filas
            dgvConsultaVenta.ReadOnly = false; // Permite la edición
        }

        private Image ByteArrayToImage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
                return null;
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                // Asegurarse de que MemoryStream no sea nulo
                if (ms == null)
                    return null;

                try
                {
                    return Image.FromStream(ms);
                }
                catch (ArgumentException)
                {
                    // Manejo del caso cuando la imagen no puede ser creada desde el stream
                    return null;
                }
            }
        }

        private byte[] ImageToByteArray(Image image)
        {
            if (image == null)
                return null;
            using (MemoryStream ms = new MemoryStream())
            {
                try
                {
                    image.Save(ms, image.RawFormat);
                    return ms.ToArray();
                }
                catch (ExternalException)
                {
                    // Manejar el caso donde la imagen no se puede guardar
                    return null;
                }
            }
        }


        private void dgvConsultaVenta_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Asegurarse de que la columna "PRECIO" esté configurada correctamente
            if (dgvConsultaVenta.Columns.Contains("PRECIO"))
            {
                dgvConsultaVenta.Columns["PRECIO"].DefaultCellStyle.Format = "$ #,##0.00";
            }

            if (dgvConsultaVenta.Columns.Contains("FOTO"))
            {
                dgvConsultaVenta.Columns["FOTO"].Width = 100;
            }

            // Ajustar el ancho de las columnas para mostrar todo el texto
            dgvConsultaVenta.Columns["VENTA"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvConsultaVenta.Columns["CLIENTE"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvConsultaVenta.Columns["FECHA"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvConsultaVenta.Columns["PRODUCTO"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvConsultaVenta.Columns["PRECIO"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvConsultaVenta.Columns["MENSAJE"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // Ajustar la altura de las filas
            dgvConsultaVenta.RowTemplate.Height = 40; // Ajusta el valor a lo que necesites
        }

        private void dgvConsultaVenta_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvConsultaVenta.Columns["FOTO"].Index && e.RowIndex >= 0)
            {
                Image img = dgvConsultaVenta.Rows[e.RowIndex].Cells["FOTO"].Value as Image;
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

        private void style()
        {
            dgvConsultaVenta.Region = new System.Drawing.Region(CreateRoundedRectangle(dgvConsultaVenta.Width, dgvConsultaVenta.Height));

            txtPrecioVenta.Anchor = AnchorStyles.Top;
            cbxIdVentaDelete.Anchor = AnchorStyles.Top;
            cbxCliente.Anchor = AnchorStyles.Top;
            btnEliminar.Anchor = AnchorStyles.Top;
            btnGuardar.Anchor = AnchorStyles.Top;
            dgvConsultaVenta.Anchor = AnchorStyles.Top;
            pictureBox3.Anchor = AnchorStyles.Top;
            timeFechaVenta.Anchor = AnchorStyles.Top;
            label1.Anchor = AnchorStyles.Top;
            label2.Anchor = AnchorStyles.Top;
            label3.Anchor = AnchorStyles.Top;
            label4.Anchor = AnchorStyles.Top;
            label5.Anchor = AnchorStyles.Top;
            label6.Anchor = AnchorStyles.Top;
            label7.Anchor = AnchorStyles.Top;
            label8.Anchor = AnchorStyles.Top;
            label10.Anchor = AnchorStyles.Top;
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
                MoveControls(1);
            }
            else if (!this.VerticalScroll.Visible && bandera2)
            {
                bandera = true;
                bandera2 = false;
                MoveControls(-2);
            }
        }

        private void MoveControls(int offset)
        {
            label1.Location = new Point(label1.Location.X + offset, label1.Location.Y);
            label2.Location = new Point(label2.Location.X + offset, label2.Location.Y);
            label3.Location = new Point(label3.Location.X + offset, label3.Location.Y);
            label4.Location = new Point(label4.Location.X + offset, label4.Location.Y);
            label5.Location = new Point(label5.Location.X + offset, label5.Location.Y);
            label6.Location = new Point(label6.Location.X + offset, label6.Location.Y);
            label7.Location = new Point(label7.Location.X + offset, label7.Location.Y);
            label8.Location = new Point(label8.Location.X + offset, label8.Location.Y);
            label10.Location = new Point(label10.Location.X + offset, label10.Location.Y);
            dgvConsultaVenta.Location = new Point(dgvConsultaVenta.Location.X + offset, dgvConsultaVenta.Location.Y);
        }

        private void vtnVenta_Resize(object sender, EventArgs e)
        {
            scrollBar();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (cbxCliente.Text.Equals("") || richTextBoxDesc.Text.Equals("") || txtPrecioVenta.Text.Equals(""))
            {
                MessageBox.Show("Hay espacios requeridos vacíos", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int idCliente, valorVenta, resultado;
            DateTime fechaVenta = timeFechaVenta.Value;
            byte[] fotoVenta = GetImageBytes(pictureBoxVenta.Image);

            try
            {
                idCliente = Convert.ToInt32(cbxCliente.SelectedValue);
                valorVenta = int.Parse(txtPrecioVenta.Text);
                // Si el ID de la venta está vacío, es una nueva venta
                if (string.IsNullOrEmpty(cbxIdVentaDelete.Text))
                {
                    resultado = vent.ingresarVenta(idCliente, richTextBoxDesc.Text, valorVenta, richTextBoxMensaje.Text, fechaVenta, fotoVenta);
                }
                else
                {
                    int idVenta = Convert.ToInt32(cbxIdVentaDelete.SelectedValue);
                    resultado = vent.actualizarVenta(idVenta, idCliente, richTextBoxDesc.Text, valorVenta, richTextBoxMensaje.Text, fechaVenta, fotoVenta);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Formato incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (resultado > 0)
            {
                informacion();
                MessageBox.Show("Venta registrada", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPrecioVenta.Text = "";
                pictureBoxVenta.Image = null;
                richTextBoxDesc.Text = "";
                cbxCliente.Text = "";
                timeFechaVenta.Value = DateTime.Now;
                cbxIdVentaDelete.Text = ""; // Limpiar el combobox de eliminación
            }
            else
            {
                MessageBox.Show("Venta no registrada", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private byte[] GetImageBytes(Image image)
        {
            if (image == null)
                return null;

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (cbxIdVentaDelete.Text.Equals(""))
            {
                MessageBox.Show("Hay espacios vacíos", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int idVenta, resultado;

            try
            {
                idVenta = Convert.ToInt32(cbxIdVentaDelete.SelectedValue);
                resultado = vent.eliminarVenta(idVenta);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Formato incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (resultado > 0)
            {
                informacion();
                MessageBox.Show("Venta eliminada", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Venta no eliminada", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtPrecioVenta_TextChanged(object sender, EventArgs e)
        {
            string idProducto = cbxCliente.Text; // Usa el valor correcto para el producto
            if (txtPrecioVenta.Text.Equals("") || !int.TryParse(txtPrecioVenta.Text, out int cantidad))
            {
                txtPrecioVenta.Text = "";
                return;
            }

            DataSet res = vent.valorVenta(idProducto);
            if (res.Tables["ResultadoDatos"].Rows.Count > 0)
            {
                int valor = Convert.ToInt32(res.Tables["ResultadoDatos"].Rows[0]["PRECIOPRODUCTO"]);
                txtPrecioVenta.Text = Convert.ToString(valor * cantidad);
            }
        }

        private void btnElegirFoto_Click(object sender, EventArgs e)
        {
            // Crear una instancia del OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Configurar las opciones del OpenFileDialog
            openFileDialog.Filter = "Archivos de Imagen|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Todos los Archivos|*.*";
            openFileDialog.Title = "Seleccionar Imagen";

            // Mostrar el cuadro de diálogo y verificar si el usuario seleccionó un archivo
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Cargar la imagen seleccionada en el PictureBox
                pictureBoxVenta.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        private void btnGuardarChanges_Click(object sender, EventArgs e)
        {
            try
            {
                // Recorre cada fila del DataGridView
                foreach (DataGridViewRow row in dgvConsultaVenta.Rows)
                {
                    if (row.IsNewRow)
                    {
                        continue;
                    }

                    int ventaId = Convert.ToInt32(row.Cells["VENTA"].Value);
                    int cliente = Convert.ToInt32(row.Cells["CLIENTE"].Value);
                    DateTime fecha = Convert.ToDateTime(row.Cells["FECHA"].Value);
                    string producto = row.Cells["PRODUCTO"].Value.ToString();
                    double precio = Convert.ToDouble(row.Cells["PRECIO"].Value);
                    string mensaje = row.Cells["MENSAJE"].Value.ToString();
                    Image foto = row.Cells["FOTO"].Value as Image;
                    byte[] fotoBytes = ImageToByteArray(foto);

                    // Aquí puedes usar un método de la clase Venta para actualizar los datos en la base de datos
                    vent.actualizarVenta(ventaId, cliente, producto, precio, mensaje, fecha, fotoBytes);
                }

                MessageBox.Show("Cambios guardados correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar cambios: " + ex.Message);
            }
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                // Configurar el contexto de licencia para EPPlus
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Para uso no comercial

                // Crear el archivo Excel
                using (var package = new ExcelPackage())
                {
                    // Crear una hoja de trabajo
                    var worksheet = package.Workbook.Worksheets.Add("Ventas");

                    // Agregar el encabezado
                    for (int i = 0; i < dgvConsultaVenta.Columns.Count; i++)
                    {
                        if (dgvConsultaVenta.Columns[i].Name != "FOTO") // Excluir la columna de imagen
                        {
                            worksheet.Cells[1, i + 1].Value = dgvConsultaVenta.Columns[i].HeaderText;
                        }
                    }

                    // Agregar el contenido de las filas
                    for (int i = 0; i < dgvConsultaVenta.Rows.Count; i++)
                    {
                        if (dgvConsultaVenta.Rows[i].IsNewRow)
                            continue;

                        for (int j = 0; j < dgvConsultaVenta.Columns.Count; j++)
                        {
                            if (dgvConsultaVenta.Columns[j].Name != "FOTO") // Excluir la columna de imagen
                            {
                                worksheet.Cells[i + 2, j + 1].Value = dgvConsultaVenta.Rows[i].Cells[j].Value?.ToString(); // Conversión explícita
                            }
                        }
                    }

                    // Definir la ruta de la carpeta y del archivo
                    string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DatosEuroflor");

                    // Verificar si la carpeta existe, si no, crearla
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Definir la ruta completa del archivo
                    string filePath = Path.Combine(folderPath, "Ventas.xlsx");

                    // Guardar el archivo
                    File.WriteAllBytes(filePath, package.GetAsByteArray());

                    MessageBox.Show($"Datos exportados exitosamente a {filePath}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al exportar datos: " + ex.Message);
            }
        }
    }
}
