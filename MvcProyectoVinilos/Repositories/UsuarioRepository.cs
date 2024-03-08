using Microsoft.Data.SqlClient;
using MvcProyectoVinilos.Models;
using ProyectoMvcVinilacion.Data;
using ProyectoMvcVinilacion.Models;
using System.Data;
using static System.Net.Mime.MediaTypeNames;

namespace MvcProyectoVinilos.Repositories
{
    public class UsuarioRepository
    {
        private WebContext context;
        private DataTable tabla;
        private SqlConnection cn;
        private SqlCommand com;

        public UsuarioRepository(WebContext webContext)
        {
            this.context = webContext;

            string connectionString = "Data Source=LOCALHOST\\SQLEXPRESS;Initial Catalog=VINILOS;User ID=sa;Password=MCSD2023;Encrypt=False;Trust Server Certificate=True";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = cn;
            this.tabla = new DataTable();
            string sql = "select * from USUARIOS";
            SqlDataAdapter ad = new SqlDataAdapter(sql, this.cn);
            ad.Fill(this.tabla);
        }

        private bool ExisteEmail(string email)
        {
            var consulta = from datos in this.context.Usuarios
                           where datos.Email == email
                           select datos;
            if (consulta.Count() > 0)
            {
                //El email existe en la base de datos
                return true;
            }
            else
            {
                return false;
            }
        }

        public Usuario LogInUsuario(string email, string contraseña)
        {
            // Buscar el usuario por correo electrónico
            Usuario usuario = this.context.Usuarios.SingleOrDefault(x => x.Email == email);

            // Si no se encuentra el usuario, devolver null
            if (usuario == null)
            {
                return null;
            }
            else
            {
                // Comparar la contraseña proporcionada con la almacenada en la base de datos
                if (usuario.Contraseña == contraseña)
                {
                    // Contraseña correcta, devolver el usuario
                    return usuario;
                }
                else
                {
                    // Contraseña incorrecta
                    return null;
                }
            }
        }

        public bool Registro(string contraseña, string email, string nombre, string apellidos)
        {

            bool compro=this.ExisteEmail(email);

            if (compro==false)
            {
                int nextid = this.tabla.AsEnumerable().Max(x => x.Field<int>("IdUsuario")) + 1;
                var sql = "Insert into USUARIOS VALUES(@id,@contraseña,@email,@nombre,@apellidos,2)";

                this.com.Parameters.AddWithValue("@id", nextid);
                this.com.Parameters.AddWithValue("@contraseña", contraseña);
                this.com.Parameters.AddWithValue("@email", email);
                this.com.Parameters.AddWithValue("@nombre", nombre);
                this.com.Parameters.AddWithValue("@apellidos", apellidos);

                this.com.CommandType = CommandType.Text;
                this.com.CommandText = sql;

                this.cn.Open();
                int af = this.com.ExecuteNonQuery();

                this.cn.Close();
                this.com.Parameters.Clear();
                return true;
            }
            else
            {
                return false;

            }
            
            
        }

        public Usuario FindUsuario(int id)
        {
            var consulta = from datos in this.tabla.AsEnumerable() where datos.Field<int>("IDUSUARIO") == id select datos;

            var row = consulta.FirstOrDefault();
            Usuario usuario = new Usuario
            {
                IdUsuario = row.Field<int>("IdUsuario"),
                Nombre = row.Field<string>("Nombre"),
                Email = row.Field<string>("EMAIL"),
                Rol = row.Field<int>("ROL"),
                Apellidos = row.Field<string>("APELLIDOS"),
                Contraseña = row.Field<string>("CONTRASEÑA"),
            };
            return usuario;
        }

        public void EditarUusario(string nombre,string apellido, string email, string contraseña,int id)
        {
            
                var sql = "UPDATE Usuarios SET Contraseña = @contra,    Email = @email,     Nombre = @nombre,     Apellidos = @apellidos    WHERE IdUsuario =@id ;";

                this.com.Parameters.AddWithValue("@id", id);
                this.com.Parameters.AddWithValue("@contra", contraseña);
                this.com.Parameters.AddWithValue("@email", email);
                this.com.Parameters.AddWithValue("@nombre", nombre);
                this.com.Parameters.AddWithValue("@apellidos", apellido);

                this.com.CommandType = CommandType.Text;
                this.com.CommandText = sql;

                this.cn.Open();
                int af = this.com.ExecuteNonQuery();

                this.cn.Close();
                this.com.Parameters.Clear();
                
            
        }

        public List<Sugerencia> GetSugerencias()
        {
            var sugerencias = this.context.Sugerencias.ToList();
            return sugerencias;

        }

        public void InsertSugerencia(string email, string nombre,string sugerencia)
        {
            var sql = "INSERT INTO SUGERENCIAS (EMAIL, NOMBRE, SUGERENCIA) " +
           "VALUES (@email, @nombre, @sugerencia)";

            this.com.Parameters.AddWithValue("@email", email);
            this.com.Parameters.AddWithValue("@nombre", nombre);
            this.com.Parameters.AddWithValue("@sugerencia", sugerencia);

            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;

            this.cn.Open();
            int af = this.com.ExecuteNonQuery();

            this.cn.Close();
            this.com.Parameters.Clear();


        }

    }
}
