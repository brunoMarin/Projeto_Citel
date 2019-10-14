//===============================================
//             INICIAR O FORM   
//===============================================
$(document).ready(function () {
    carregarComboCategoria();
    atualizarDataTable();
});

//===============================================
// CARREGAR COMBO DE CATEGORIA
//===============================================
function carregarComboCategoria() {
    var url = location.href; //pega endereço que esta no navegador
    url = url.split("/"); //quebra o endereço de acordo com a / (barra)
    var urlRaizSite = url[2];

    $.ajax({
        url: "http://" + urlRaizSite + "/Categoria/GETCategoria",
        type: 'GET',
        dataType: 'json',
        success: function (data) {

            //Adicionar TODAS
            $('<option value="' + "0" + '">' + "TODAS" + '</option>').appendTo('#ViewConsultaCategoria');
           // $('<option value="' + "0" + '">' + "TODAS" + '</option>').appendTo('#cbxCategoria');

            //Percorrer todos encontrados e adicionar
            $.each(data, function (i, item) {
                $('<option style="color:#' + item.CorHexadecimalCalendario + '" value="' + item.IdCategoria + '"><strong>' + item.DcCategoria + ' </strong></option>').appendTo('#ViewConsultaCategoria'); //Filtro Consulta
                $('<option style="color:#' + item.CorHexadecimalCalendario + '" value="' + item.IdCategoria + '"><strong>' + item.DcCategoria + ' </strong></option>').appendTo('#cbxCategoria'); //Campo Inserção Modal
            });
        },
        error: function () {
            console.log('err')
        }
    });
}

//===============================================
// ABRIR MODAL CADASTRAR PRODUTO
//===============================================
function abrirModalCadastrarProduto(acao, IdProduto, IdCategoria, DcProduto, VlrPreco, DcCaracteristica, ocultarRodape) {

    var divExcluirCadastroProduto = document.getElementById("divExcluirCadastroProduto");
    var divRodapeCadastroProduto = document.getElementById("divRodapeCadastroProduto");

    if (acao == "I") {
        document.getElementById('txtProdutoCodigoConsultar').value = "0";
        document.getElementById('cbxCategoria').value = '';
        document.getElementById('txtProdutoDescricao').value = '';
        document.getElementById('txtValor').value = '';
        document.getElementById('txtProdutoCaracteristica').value = '';
        //Ocultar o Excluir
        divExcluirCadastroProduto.style.display = "none";

    } else if (acao == "A") {

        //Exibir o Excluir
        divExcluirCadastroProduto.style.display = "block";

        //Na alteração passo os valores recebidos pelo controller
        document.getElementById('txtProdutoCodigoConsultar').value = IdProduto;
        document.getElementById('cbxCategoria').value = IdCategoria;
        document.getElementById('txtProdutoDescricao').value = DcProduto;
        document.getElementById('txtValor').value = VlrPreco;
        document.getElementById('txtProdutoCaracteristica').value = DcCaracteristica;
    }

    if (ocultarRodape == "1") {
        divRodapeCadastroProduto.style.display = "none";
    } else {
        divRodapeCadastroProduto.style.display = "block";
    }

    $('#ViewCadastrarProdutoMdl').modal('show');
   
}

//===============================================
// FUNÇÃO PARA SALVAR/ALTERAR A PRODUTO
//===============================================
function inserirCadastroProduto() {

    var parametros = {
        IdProduto: $("#txtProdutoCodigoConsultar").val(),
        DcProduto: $("#txtProdutoDescricao").val(),
        IdCategoria: $('#cbxCategoria').val(),
        VlrPreco: $('#txtValor').val(),
        DcCaracteristica: $('#txtProdutoCaracteristica').val(),
    };

    if (parametros.DcProduto == null || parametros.DcProduto == '') {
        exibirToastErro('Informe a descrição da Produto.');
        return;
    }

    if (parametros.IdCategoria == null || parametros.IdCategoria == '0') {
        exibirToastErro('Informe a categoria do Produto.');
        return;
    }

    if (parametros.VlrPreco == null || parametros.VlrPreco == '0' || parametros.VlrPreco == '') {
        exibirToastErro('Informe o preço do Produto.');
        return;
    }

    if (parametros.DcCaracteristica == null || parametros.DcCaracteristica == '') {
        exibirToastErro('Informe a característica do Produto.');
        return;
    }
    //Pegar a raiz do site para passar o método de controler, se não vai passar na frente do atual e nao vai encontrar
    var url = location.href; //pega endereço que esta no navegador
    url = url.split("/"); //quebra o endereço de acordo com a / (barra)
    var urlRaizSite = url[2];

    $.ajax({
        url: "http://" + urlRaizSite + "/Produto/POSTInserirProduto",
        type: 'POST',
        dataType: "json",
        contentType: 'application/json',
        data: JSON.stringify(parametros),

        beforeSend: function () { //Antes de enviar a requisição AJAX
            $('#divPainelAguarde').show(); //Exibir a Div de Aguarde
        },
        success: function (response) { //Se deu certo na requisição AJAX
            if (response == "OK") {
                $('#divPainelAguarde').hide();

                $('#ViewCadastrarProdutoMdl').modal('hide');

                //Exibir o alerta de sucesso
                if (parametros.IdProduto == 0 || parametros.IdProduto == null) {
                    exibirToastSucesso("Produto cadastrado com sucesso!");
                } else {
                    exibirToastSucesso("Produto ID: " + parametros.IdProduto + " alterado com sucesso!");
                }

                //Atualiza o Grid
                atualizarDataTable();
            }
            else { //Exibir mensagem de erro na modal
                $('#divPainelAguarde').hide();
                exibirToastErro(response);
            }
        },
        failure: function (response) {
            $('#divPainelAguarde').hide();
            exibirToastErro(response);
        },
        error: function (response) {
            $('#divPainelAguarde').hide();
            exibirToastErro(response);
        }
    });

}

//===============================================
// ATUALIZAR O DATATABLE   
//===============================================
function atualizarDataTable() {

    //Se a tabela ja existir devo destrui-la
    var tableExistente = $('#tabProduto').DataTable();
    if (tableExistente != null) {
        tableExistente.destroy();
    }


    var url = location.href; //pega endereço que esta no navegador
    url = url.split("/"); //quebra o endereço de acordo com a / (barra)
    var urlRaizSite = url[2];

    var table = $('#tabProduto').DataTable({
        pageLength: 25,
        responsive: true,
        dom: '<"html5buttons"B>lTfgitp',
        buttons: [
            { extend: 'excel', title: 'CadastroProduto' }
        ],

        ajax: "http://" + urlRaizSite + "/Produto/GETProdutoDataTable",
        type: "GET", // Método GET
        serverSide: false, //Renderização da tabela feita no cliente
        filter: true, //Permite Filtrar
        stateSave: false, //Volta a tabela ao estado original quando atualizar a página
        ordering: true, //Desabilita a ordenação, pois as datas ordenadas caga tudo

        //Traduzir os componentes da Tabela
        "oLanguage": {
            "sEmptyTable": "Nenhum registro encontrado",
            "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
            "sInfoFiltered": "(Filtrados de _MAX_ registros)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "Mostrar até _MENU_ resultados por página",
            "sLoadingRecords": "<img style='width:64px; height:64px;' src='/Content/Imagens/carregando.gif' />", /*-Substituir a mensagem de Carregando pela imagem de aguarde-*/
            "sProcessing": "Processando...", /*-Substituir a mensagem de Processando pela imagem de aguarde-*/
            "sZeroRecords": "Nenhum registro encontrado",
            "sSearch": "Pesquisar por: ",
            "oPaginate": {
                "sNext": "Próximo",
                "sPrevious": "Anterior",
                "sFirst": "Primeiro",
                "sLast": "Último"
            },
            "oAria": {
                "sSortAscending": ": Ordenar colunas de forma ascendente",
                "sSortDescending": ": Ordenar colunas de forma descendente"
            }
        },

        //Alinhamento das colunas
        'columnDefs': [
            {
                "targets": 0, // your case first column
                "className": "text-left",
                "width": "5pt"
            },
            {
                "targets": 1,
                "className": "text-center",
                "width": "15pt"
            },
            {
                "targets": 2,
                "className": "text-center",
                "width": "5pt"
            },
            {
                "targets": 3,
                "className": "text-center",
                "width": "5pt"
            },
            {
                "targets": 4,
                "className": "text-center",
                "width": "5pt"
            },
            {
                "targets": 5,
                "className": "text-center",
                "width": "5pt"
            },
            {
                "targets": 6,
                "className": "text-center",
                "width": "5pt"
            }
        ]
    });

};

//===============================================
// ATUALIZAR O DATATABLE FILTRO
//===============================================
function atualizarDataTableFiltro() {

    var IdCategoria = $('#ViewConsultaCategoria').val();

    //Se a tabela ja existir devo destrui-la
    var tableExistente = $('#tabProduto').DataTable();
    if (tableExistente != null) {
        tableExistente.destroy();
    }


    var url = location.href; //pega endereço que esta no navegador
    url = url.split("/"); //quebra o endereço de acordo com a / (barra)
    var urlRaizSite = url[2];

    var table = $('#tabProduto').DataTable({
        pageLength: 25,
        responsive: true,
        dom: '<"html5buttons"B>lTfgitp',
        buttons: [
            { extend: 'excel', title: 'CadastroProduto' }
        ],

        ajax: "http://" + urlRaizSite + "/Produto/GETProdutoDataTableFiltro?IdCategoria=" + IdCategoria,
        type: "GET", // Método GET
        serverSide: false, //Renderização da tabela feita no cliente
        filter: true, //Permite Filtrar
        stateSave: false, //Volta a tabela ao estado original quando atualizar a página
        ordering: true, //Desabilita a ordenação, pois as datas ordenadas caga tudo

        //Traduzir os componentes da Tabela
        "oLanguage": {
            "sEmptyTable": "Nenhum registro encontrado",
            "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
            "sInfoFiltered": "(Filtrados de _MAX_ registros)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "Mostrar até _MENU_ resultados por página",
            "sLoadingRecords": "<img style='width:64px; height:64px;' src='/Content/Imagens/carregando.gif' />", /*-Substituir a mensagem de Carregando pela imagem de aguarde-*/
            "sProcessing": "Processando...", /*-Substituir a mensagem de Processando pela imagem de aguarde-*/
            "sZeroRecords": "Nenhum registro encontrado",
            "sSearch": "Pesquisar por: ",
            "oPaginate": {
                "sNext": "Próximo",
                "sPrevious": "Anterior",
                "sFirst": "Primeiro",
                "sLast": "Último"
            },
            "oAria": {
                "sSortAscending": ": Ordenar colunas de forma ascendente",
                "sSortDescending": ": Ordenar colunas de forma descendente"
            }
        },

        //Alinhamento das colunas
        'columnDefs': [
            {
                "targets": 0, // your case first column
                "className": "text-left",
                "width": "5pt"
            },
            {
                "targets": 1,
                "className": "text-center",
                "width": "15pt"
            },
            {
                "targets": 2,
                "className": "text-center",
                "width": "5pt"
            },
            {
                "targets": 3,
                "className": "text-center",
                "width": "5pt"
            },
            {
                "targets": 4,
                "className": "text-center",
                "width": "5pt"
            },
            {
                "targets": 5,
                "className": "text-center",
                "width": "5pt"
            },
            {
                "targets": 6,
                "className": "text-center",
                "width": "5pt"
            }
        ]
    });

};

//===============================================
// FUNÇÃO PARA EXCLUIR PRODUTO
//===============================================
function excluirProduto(IdProdutoParam) {

    var parametros = {
        IdProduto: IdProdutoParam
    };

    //Pegar a raiz do site para passar o método de controler, se não vai passar na frente do atual e nao vai encontrar
    var url = location.href; //pega endereço que esta no navegador
    url = url.split("/"); //quebra o endereço de acordo com a / (barra)
    var urlRaizSite = url[2];

    $.ajax({
        url: "http://" + urlRaizSite + "/Produto/POSTDeletarProduto",
        type: 'POST',
        dataType: "json",
        contentType: 'application/json',
        data: JSON.stringify(parametros),

        beforeSend: function () { //Antes de enviar a requisição AJAX
            $('#divPainelAguarde').show(); //Exibir a Div de Aguarde
        },
        success: function (response) { //Se deu certo na requisição AJAX
            if (response == "OK") {
                $('#divPainelAguarde').hide();

                //Exibir o alerta de sucesso
                exibirToastSucesso("Produto ID: " + IdProdutoParam + " excluída com sucesso!");

                //Atualiza a tabela
                atualizarDataTable();

            }
            else { //Exibir mensagem de erro na modal
                $('#divPainelAguarde').hide();
                exibirToastErro(response);
            }
        },
        failure: function (response) {
            $('#divPainelAguarde').hide();
            exibirToastErro(response);
        },
        error: function (response) {
            $('#divPainelAguarde').hide();
            exibirToastErro(response);
        }
    });

}