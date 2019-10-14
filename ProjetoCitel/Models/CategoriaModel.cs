using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data;
using MySql.Data.MySqlClient;

namespace ProjetoCitel.Models
{
    public class CategoriaModel : IDisposable
    {

        #region ::. ABRIR CONEXÃO  ====> ALTERAR CONNECTION STRING.::

        private MySqlConnection connection;

        public CategoriaModel()
        {
            string strConn = @"Server=localhost;Database=dbcitel;Uid=root;Pwd=root;";
            connection = new MySqlConnection(strConn);
            connection.Open();
        }

        #endregion

        #region ::. DISPOSE .::

        public void Dispose()
        {
            connection.Close();
        }

        #endregion

        #region ::. INSERT .::

        public string InsertCategoria(Categoria categoria)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbcitel.uspInsertCategoria";
                cmd.Parameters.AddWithValue("_DcCategoria", categoria.DcCategoria);

                return cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        #endregion

        #region ::. SELECT .::

        public CategoriaCollection GetCategoria()
        {
            CategoriaCollection categoriaCollection = new CategoriaCollection();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbcitel.uspSelectCategoria";

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Categoria categoria = new Categoria();
                categoria.IdCategoria = (int)reader["IdCategoria"];
                categoria.DcCategoria = (string)reader["DcCategoria"];

                categoriaCollection.Add(categoria);
            }

            return categoriaCollection;
        }

        #endregion

        #region ::. UPDATE .::

        public string UpdateCategoria(Categoria categoria)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbcitel.uspUpdateCategoria";
                cmd.Parameters.AddWithValue("_IdCategoria", categoria.IdCategoria);
                cmd.Parameters.AddWithValue("_DcCategoria", categoria.DcCategoria);

                return cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion

        #region ::. DELETE .::

        public string DeleteCategoria(int id)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbcitel.uspDeleteCategoria";
                cmd.Parameters.AddWithValue("_IdCategoria", id);

                return cmd.ExecuteScalar().ToString();

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion
    }
}