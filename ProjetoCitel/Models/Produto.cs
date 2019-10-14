using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoCitel.Models
{
    public class Produto
    {
        public int IdProduto { get; set; }

        public Categoria IdCategoria { get; set; }
        public string DcProduto { get; set; }
        public decimal VlrPreco { get; set; }
        public string DcCaracteristica { get; set; }
    }

    public class ProdutoCollection : List<Produto> { }
}