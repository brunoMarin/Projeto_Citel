using ProjetoCitel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjetoCitel.Controllers
{
    public class CategoriaController : Controller
    {

        string mensagemRetornoJSON = string.Empty;

        MdlAjaxDataTable mdlAjaxDataTable = new MdlAjaxDataTable();
        CategoriaModel catModel = new CategoriaModel();

        // GET: Categoria
        public ActionResult Index()
        {

            using (CategoriaModel model = new CategoriaModel())
            {
                CategoriaCollection categorias = model.GetCategoria();
                return View(categorias);
            }
        }


        [HttpGet]
        public ActionResult Categoria()
        {
            return View();
        }


        #region ::. GET .::

        [HttpGet]
        public ActionResult GETCategoria()
        {
            CategoriaCollection categoriaCollection = new CategoriaCollection();
            CategoriaModel model = new CategoriaModel();
            categoriaCollection = model.GetCategoria();

            return Json(categoriaCollection, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region ::. GET DATATABLE .::

        [HttpGet]
        public ActionResult GETCategoriaDataTable()
        {
            CategoriaCollection categoriaCollection = new CategoriaCollection();
            CategoriaModel model = new CategoriaModel();
            categoriaCollection = model.GetCategoria();

            foreach (var item in categoriaCollection)
            {
                IList<string> dataRow = new List<string>();
                dataRow.Add(item.IdCategoria.ToString());
                dataRow.Add(item.DcCategoria);

                string botaoAcaoHtmlExluir = "";
                string botaoAcaoHtmlAlterar = "";
                botaoAcaoHtmlAlterar = "<button onclick =\"abrirModalCadastrarCategoria('A','" + item.IdCategoria.ToString() + "','" + item.DcCategoria + "','0')\" class=\"btn btn-flat btn-sm btn-light texto_escuro text-center\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"Alterar Categoria\"><i class=\"fa fa-edit\"></i></button>";
                botaoAcaoHtmlExluir = "<button onclick=\"excluirCategoria(" + item.IdCategoria.ToString() + ")\" class=\"btn btn-flat btn-sm btn-danger text-white text-center margem_botao_acao\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"Excluir Categoria\"><i class=\"fa fa-trash\"></i></button>";

                dataRow.Add(botaoAcaoHtmlAlterar);
                dataRow.Add(botaoAcaoHtmlExluir);

                mdlAjaxDataTable.aaData.Add(dataRow);
            }

            return Json(mdlAjaxDataTable, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region ::. INSERIR CATEGORIA .::

        [HttpPost]
        public JsonResult POSTInserirCategoria(string IdCategoria, string DcCategoria)
        {
            mensagemRetornoJSON = string.Empty;
            int idIdCategoriaTryParse;
            int IdCategoriaConvertida = 0;

            if (int.TryParse(IdCategoria, out idIdCategoriaTryParse))
                IdCategoriaConvertida = idIdCategoriaTryParse;

            Categoria categoria = new Categoria();
            categoria.IdCategoria = IdCategoriaConvertida;
            categoria.DcCategoria = DcCategoria;

            using (CategoriaModel model = new CategoriaModel())
            {


                string retornoBD = "";
                if (categoria.IdCategoria > 0)
                    retornoBD = model.UpdateCategoria(categoria);
                else
                    retornoBD = model.InsertCategoria(categoria);

                if (retornoBD.Equals("OK"))
                    mensagemRetornoJSON = "OK";


                mensagemRetornoJSON = retornoBD;

                return Json(mensagemRetornoJSON);
            }
        }

        #endregion

        #region ::. DELETAR CATEGORIA .::

        [HttpPost]
        public JsonResult POSTDeletarCategoria(string IdCategoria)
        {
            mensagemRetornoJSON = string.Empty;
            int idIdCategoriaTryParse;
            int IdCategoriaConvertida = 0;
            int qtdRegistroEncontrado = 0;

            if (int.TryParse(IdCategoria, out idIdCategoriaTryParse))
                IdCategoriaConvertida = idIdCategoriaTryParse;

            

            using (CategoriaModel model = new CategoriaModel())
            {
                ProdutoModel produtoModel = new ProdutoModel();
                qtdRegistroEncontrado = produtoModel.GetProduto().FindAll(x => x.IdCategoria.IdCategoria == IdCategoriaConvertida).Count;

                string retornoBD = "";

                if (qtdRegistroEncontrado > 0 )
                {
                    retornoBD = "Exclusão não permitida! Há produto(s) cadastrado(s) com essa categoria.";
                }
                else
                {
                    retornoBD = model.DeleteCategoria(IdCategoriaConvertida);
                }

                if (retornoBD.Equals("OK"))
                    mensagemRetornoJSON = "OK";


                mensagemRetornoJSON = retornoBD;

                return Json(mensagemRetornoJSON);
            }
        }
        #endregion

    }
}