using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MySql.Data.MySqlClient;

namespace ProjetoCitel.Models
{
    public class ProdutoModel : IDisposable
    {

        #region ::. ABRIR CONEXÃO  ====> ALTERAR CONNECTION STRING.::

        private MySqlConnection connection;

        public ProdutoModel()
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

        public string InsertProduto(Produto produto)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbcitel.uspInsertProduto";
                cmd.Parameters.AddWithValue("_IdCategoria", produto.IdCategoria.IdCategoria);
                cmd.Parameters.AddWithValue("_DcProduto", produto.DcProduto);
                cmd.Parameters.AddWithValue("_VlrPreco", produto.VlrPreco);
                cmd.Parameters.AddWithValue("_DcCaracteristica", produto.DcCaracteristica);

                return cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        #endregion

        #region ::. SELECT .::

        public ProdutoCollection GetProduto()
        {
            ProdutoCollection produtoCollection = new ProdutoCollection();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbcitel.uspSelectProduto";

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Produto produto = new Produto();
                produto.IdProduto = (int)reader["IdProduto"];
                produto.IdCategoria = new Categoria()
                {
                    IdCategoria = (int)reader["IdCategoria"],
                    DcCategoria = (string)reader["DcCategoria"]
                };
                    
                produto.DcProduto = (string)reader["DcProduto"];
                produto.VlrPreco = (decimal)reader["VlrPreco"];
                produto.DcCaracteristica = (string)reader["DcCaracteristica"];

                produtoCollection.Add(produto);
            }

            return produtoCollection;
        }

        #endregion

        #region ::. UPDATE .::

        public string UpdateProduto(Produto produto)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbcitel.uspUpdateProduto";
                cmd.Parameters.AddWithValue("_IdProduto", produto.IdProduto);
                cmd.Parameters.AddWithValue("_IdCategoria", produto.IdCategoria.IdCategoria);
                cmd.Parameters.AddWithValue("_DcProduto", produto.DcProduto);
                cmd.Parameters.AddWithValue("_VlrPreco", produto.VlrPreco);
                cmd.Parameters.AddWithValue("_DcCaracteristica", produto.DcCaracteristica);

                return cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion

        #region ::. DELETE .::

        public string DeleteProduto(int id)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbcitel.uspDeleteProduto";
                cmd.Parameters.AddWithValue("_IdProduto", id);

                return cmd.ExecuteScalar().ToString();

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion

        #region ::. SELECT FILTRO .::

        public ProdutoCollection GetProdutoFiltro(int IdCategoria)
        {
            ProdutoCollection produtoCollection = new ProdutoCollection();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbcitel.uspSelectProdutoFiltro";
            cmd.Parameters.AddWithValue("_IdCategoria", IdCategoria);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Produto produto = new Produto();
                produto.IdProduto = (int)reader["IdProduto"];
                produto.IdCategoria = new Categoria()
                {
                    IdCategoria = (int)reader["IdCategoria"],
                    DcCategoria = (string)reader["DcCategoria"]
                };

                produto.DcProduto = (string)reader["DcProduto"];
                produto.VlrPreco = (decimal)reader["VlrPreco"];
                produto.DcCaracteristica = (string)reader["DcCaracteristica"];

                produtoCollection.Add(produto);
            }

            return produtoCollection;
        }

        #endregion
    }
}