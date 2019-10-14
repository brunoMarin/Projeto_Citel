using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoCitel.Models
{
    public class Categoria
    {
        public int IdCategoria { get; set; }

        public string DcCategoria { get; set; }

    }

    public class CategoriaCollection : List<Categoria> { }
}