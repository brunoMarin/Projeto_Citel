using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoCitel.Models
{
    public class MdlAjaxDataTable
    {

        //--NÃO ALTERAR O NOME "aaData", ISSO VAI NO INÍCIO DO JSON E É UMA REGRA DO AJAX JQUERY!
        public IList<IList<string>> aaData = new List<IList<string>>();
    }
}