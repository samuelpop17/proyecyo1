using Microsoft.Data.SqlClient;
using MvcProyectoVinilos.Models;
using ProyectoMvcVinilacion.Data;
using System.Data;

namespace MvcProyectoVinilos.Repositories
{
    public class ComentarioRepository
    {
        private WebContext context;
        private DataTable tabla;
        private SqlConnection cn;
        private SqlCommand com;

        public ComentarioRepository(WebContext context)
        {
            this.context = context;
            string connectionString = "Data Source=LOCALHOST\\SQLEXPRESS;Initial Catalog=VINILOS;User ID=sa;Password=MCSD2023;Encrypt=False;Trust Server Certificate=True";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = cn;
            this.tabla = new DataTable();
            string sql = "select * from Comentarios";
            SqlDataAdapter ad = new SqlDataAdapter(sql, this.cn);
            ad.Fill(this.tabla);
        } 

        public List<Comentario> GetComentarios()
        {
            var consulta = from datos in this.tabla.AsEnumerable() select datos;
            List<Comentario> comentarios = new List<Comentario>();
            foreach (var row in consulta)
            {
                Comentario comentario = new Comentario
                {
                    IdComentario = row.Field<int>("IdComentario"),
                    IdUsuario = row.Field<int>("IdUsuario"),
                    Coment = row.Field<string>("COMENTARIO"),
                    NombreUusario = row.Field<string>("NombreUsuario"),
                    
                };

                comentarios.Add(comentario);
            }
            return comentarios;
        }

        public void InsertComentario(string comentario,string nombre, int usuario)
        {
            int nextid = this.tabla.AsEnumerable().Max(x => x.Field<int>("IdComentario")) + 1;

            var sql = "Insert into COMENTARIOS VALUES(@id,@nombre,@idusu,@comentario)";

            this.com.Parameters.AddWithValue("@id", nextid);
            this.com.Parameters.AddWithValue("@nombre", nombre);
            this.com.Parameters.AddWithValue("@idusu", usuario);
            this.com.Parameters.AddWithValue("@comentario", comentario);

            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;

            this.cn.Open();
            int af = this.com.ExecuteNonQuery();

            this.cn.Close();
            this.com.Parameters.Clear();


        }

        public void EliminarComentario(int id)
        {
            var sql = "DELETE FROM COMENTARIOS WHERE IdComentario = @idComentario";

            this.com.Parameters.AddWithValue("@idComentario", id);

            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;

            this.cn.Open();
            int af = this.com.ExecuteNonQuery();

            this.cn.Close();
            this.com.Parameters.Clear();
        }

    }
}
