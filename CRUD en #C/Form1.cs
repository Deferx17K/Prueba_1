using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_en__C
{
    public partial class Form1 : Form
    {

        string cadenaConexion = @"Server=(localdb)\MSSQLLocalDB;Database=CrudEstudiantes;Integrated Security=true;";
        public Form1()
        {
            InitializeComponent();
        }

        private void CargarDatos()
        {
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                string query = "SELECT * FROM Estudiantes ORDER BY Nombre ASC"; //nos ayuda a que los nombres se ordenen acendentemente desde la base de datos mediante el SQL.
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvEstudiantes.DataSource = dt;
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            //Nos advierte si hay campos vacios que añadan
            int edad;
            if (!int.TryParse(txtEdad.Text, out edad))
            {
                MessageBox.Show("La edad debe ser un valor numérico válido.", "Error de validación");
                txtEdad.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtApellido.Text) || string.IsNullOrWhiteSpace(txtEdad.Text) || string.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Advertencia");
                return;
            }

            //Llama los campos que se escriban en el textBox para poder agregarlo en la tabla que tambien la llama para no perder datos.
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                conn.Open();
                string query = "INSERT INTO Estudiantes (Nombre, Apellido, Edad, Correo) VALUES (@Nombre, @Apellido, @Edad, @Correo)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                cmd.Parameters.AddWithValue("@Apellido", txtApellido.Text);
                cmd.Parameters.AddWithValue("@Edad", int.Parse(txtEdad.Text));
                cmd.Parameters.AddWithValue("@Correo", txtCorreo.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Estudiante agregado correctamente.");
                CargarDatos();
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (dgvEstudiantes.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvEstudiantes.SelectedRows[0].Cells["Id"].Value);
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    conn.Open();
                    string query = "UPDATE Estudiantes SET Nombre=@Nombre, Apellido=@Apellido, Edad=@Edad, Correo=@Correo WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@Apellido", txtApellido.Text);
                    cmd.Parameters.AddWithValue("@Edad", int.Parse(txtEdad.Text));
                    cmd.Parameters.AddWithValue("@Correo", txtCorreo.Text);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Estudiante actualizado.");
                    CargarDatos();
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvEstudiantes.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvEstudiantes.SelectedRows[0].Cells["Id"].Value);
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    conn.Open();
                    string query = "DELETE FROM Estudiantes WHERE Id=@Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Estudiante eliminado.");
                    CargarDatos();
                }
            }
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void dgvEstudiantes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtNombre.Text = dgvEstudiantes.Rows[e.RowIndex].Cells["Nombre"].Value.ToString();
                txtApellido.Text = dgvEstudiantes.Rows[e.RowIndex].Cells["Apellido"].Value.ToString();
                txtEdad.Text = dgvEstudiantes.Rows[e.RowIndex].Cells["Edad"].Value.ToString();
                txtCorreo.Text = dgvEstudiantes.Rows[e.RowIndex].Cells["Correo"].Value.ToString();
            }
        }
        private void BuscarPorApellido(string apellidoFiltro)
        {
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                string query = "SELECT Id, Nombre, Apellido, Edad, Correo FROM Estudiantes WHERE Apellido LIKE @Apellido";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Apellido", "%" + apellidoFiltro + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvEstudiantes.DataSource = dt;
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BuscarPorApellido(txtBuscar.Text);
        }
    }

}
