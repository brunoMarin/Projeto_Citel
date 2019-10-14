using ProjetoCitel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjetoCitel.Controllers
{
    public class ProdutoController : Controller
    {
        string mensagemRetornoJSON = string.Empty;

        MdlAjaxDataTable mdlAjaxDataTable = new MdlAjaxDataTable();
        ProdutoModel proModel = new ProdutoModel();

        public ActionResult Index()
        {

            using (ProdutoModel model = new ProdutoModel())
            {
                ProdutoCollection produtos = model.GetProduto();
                return View(produtos);
            }
        }

        [HttpGet]
        public ActionResult Produto()
        {
            return View();
        }

        #region ::. GET PRODUTO .::

        [HttpGet]
        public ActionResult GETProduto()
        {
            ProdutoCollection produtoCollection = new ProdutoCollection();
            ProdutoModel model = new ProdutoModel();
            produtoCollection = model.GetProduto();

            return Json(produtoCollection, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region ::. GET PRODUTO DATATABLE .::

        [HttpGet]
        public ActionResult GETProdutoDataTable()
        {
            ProdutoCollection produtoCollection = new ProdutoCollection();
            ProdutoModel model = new ProdutoModel();
            produtoCollection = model.GetProduto();

            foreach (var item in produtoCollection)
            {
                IList<string> dataRow = new List<string>();
                dataRow.Add(item.IdProduto.ToString());
                dataRow.Add(item.IdCategoria.IdCategoria.ToString());
                dataRow.Add(item.IdCategoria.DcCategoria);
                dataRow.Add(item.DcProduto);
                dataRow.Add(item.VlrPreco.ToString());
                dataRow.Add(item.DcCaracteristica);

                string botaoAcaoHtmlExluir = "";
                string botaoAcaoHtmlAlterar = "";
                botaoAcaoHtmlAlterar = "<button onclick =\"abrirModalCadastrarProduto('A','" + item.IdProduto.ToString() + "','" + item.IdCategoria.IdCategoria.ToString() + "','" + item.DcProduto + "','" + item.VlrPreco.ToString() + "','" + item.DcCaracteristica + "','0')\" class=\"btn btn-flat btn-sm btn-light texto_escuro text-center\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"Alterar produto\"><i class=\"fa fa-edit\"></i></button>";
                botaoAcaoHtmlExluir = "<button onclick=\"excluirProduto(" + item.IdProduto.ToString() + ")\" class=\"btn btn-flat btn-sm btn-danger text-white text-center margem_botao_acao\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"Excluir Produto\"><i class=\"fa fa-trash\"></i></button>";

                dataRow.Add(botaoAcaoHtmlAlterar);
                dataRow.Add(botaoAcaoHtmlExluir);

                mdlAjaxDataTable.aaData.Add(dataRow);
            }

            return Json(mdlAjaxDataTable, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region ::. GET PRODUTO DATATABLE FILTRO .::

        [HttpGet]
        public ActionResult GETProdutoDataTableFiltro(string IdCategoria)
        {
            int idIdCategoriaTryParse;
            int IdCategoriaConvertida = 0;

            if (int.TryParse(IdCategoria, out idIdCategoriaTryParse))
                IdCategoriaConvertida = idIdCategoriaTryParse;


            ProdutoCollection produtoCollection = new ProdutoCollection();
            ProdutoModel model = new ProdutoModel();

            if (IdCategoriaConvertida == 0)
            {
                produtoCollection = model.GetProduto();
            }
            else
            {
                produtoCollection = model.GetProdutoFiltro(IdCategoriaConvertida);
            }

            foreach (var item in produtoCollection)
            {
                IList<string> dataRow = new List<string>();
                dataRow.Add(item.IdProduto.ToString());
                dataRow.Add(item.IdCategoria.IdCategoria.ToString());
                dataRow.Add(item.IdCategoria.DcCategoria);
                dataRow.Add(item.DcProduto);
                dataRow.Add(item.VlrPreco.ToString());
                dataRow.Add(item.DcCaracteristica);

                string botaoAcaoHtmlExluir = "";
                string botaoAcaoHtmlAlterar = "";
                botaoAcaoHtmlAlterar = "<button onclick =\"abrirModalCadastrarProduto('A','" + item.IdProduto.ToString() + "','" + item.IdCategoria.ToString() + "','" + item.DcProduto + "','" + item.VlrPreco.ToString() + "','" + item.DcCaracteristica + "','0')\" class=\"btn btn-flat btn-sm btn-light texto_escuro text-center\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"Alterar produto\"><i class=\"fa fa-edit\"></i></button>";
                botaoAcaoHtmlExluir = "<button onclick=\"excluirProduto(" + item.IdProduto.ToString() + ")\" class=\"btn btn-flat btn-sm btn-danger text-white text-center margem_botao_acao\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"Excluir Produto\"><i class=\"fa fa-trash\"></i></button>";

                dataRow.Add(botaoAcaoHtmlAlterar);
                dataRow.Add(botaoAcaoHtmlExluir);

                mdlAjaxDataTable.aaData.Add(dataRow);
            }

            return Json(mdlAjaxDataTable, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region ::. INSERIR/ALTERAR PRODUTO .::

        [HttpPost]
        public JsonResult POSTInserirProduto(string IdProduto, string IdCategoria, string DcProduto, string VlrPreco, string DcCaracteristica)
        {
            mensagemRetornoJSON = string.Empty;
            int idIdProdutoTryParse;
            int IdProdutoConvertida = 0;

            int idIdCategoriaTryParse;
            int IdCategoriaConvertida = 0;

            decimal VlrPrecoTryPasrse;
            decimal VlrPrecoConvertida = 0;

            if (int.TryParse(IdProduto, out idIdProdutoTryParse))
                IdProdutoConvertida = idIdProdutoTryParse;

            if (int.TryParse(IdCategoria, out idIdCategoriaTryParse))
                IdCategoriaConvertida = idIdCategoriaTryParse;

            if (decimal.TryParse(VlrPreco, out VlrPrecoTryPasrse))
                VlrPrecoConvertida = VlrPrecoTryPasrse;

            Produto produto = new Produto();
            produto.IdProduto = IdProdutoConvertida;
            produto.IdCategoria = new Categoria()
            {
                IdCategoria = IdCategoriaConvertida
            };

            produto.DcProduto = DcProduto;
            produto.VlrPreco = VlrPrecoConvertida;
            produto.DcCaracteristica = DcCaracteristica;

            using (ProdutoModel model = new ProdutoModel())
            {

                string retornoBD = "";
                if (produto.IdProduto > 0)
                    retornoBD = model.UpdateProduto(produto);
                else
                    retornoBD = model.InsertProduto(produto);

                if (retornoBD.Equals("OK"))
                    mensagemRetornoJSON = "OK";


                mensagemRetornoJSON = retornoBD;

                return Json(mensagemRetornoJSON);
            }
        }

        #endregion

        #region ::. DELETAR PRODUTO .::

        [HttpPost]
        public JsonResult POSTDeletarProduto(string IdProduto)
        {
            mensagemRetornoJSON = string.Empty;
            int idIdProdutoTryParse;
            int IdProdutoConvertido = 0;

            if (int.TryParse(IdProduto, out idIdProdutoTryParse))
                IdProdutoConvertido = idIdProdutoTryParse;

            using (ProdutoModel model = new ProdutoModel())
            {
                string retornoBD = "";
                retornoBD = model.DeleteProduto(IdProdutoConvertido);

                if (retornoBD.Equals("OK"))
                    mensagemRetornoJSON = "OK";


                mensagemRetornoJSON = retornoBD;

                return Json(mensagemRetornoJSON);
            }
        }
        #endregion

    }
}