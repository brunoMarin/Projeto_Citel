//===============================================
//             INICIAR O FORM   
//===============================================
$(document).ready(function () {
    atualizarDataTable();
});

//===============================================
// ABRIR MODAL CADASTRAR CATEGORIA
//===============================================
function abrirModalCadastrarCategoria(acao, IdCategoria, DcCategoria, ocultarRodape) {

    var divExcluirCadastroCategoria = document.getElementById("divExcluirCadastroCategoria");
    var divRodapeCadastroCategoria = document.getElementById("divRodapeCadastroCategoria");

    if (acao == "I") {
        document.getElementById('txtCategoriaCodigoConsultar').value = "0";
        document.getElementById('txtCategoriaDescricao').value = "";

        //Ocultar o Excluir
        divExcluirCadastroCategoria.style.display = "none";

    } else if (acao == "A") {

        //Exibir o Excluir
        divExcluirCadastroCategoria.style.display = "block";

        //Na alteração passo os valores recebidos pelo controller
        document.getElementById('txtCategoriaCodigoConsultar').value = IdCategoria;
        document.getElementById('txtCategoriaDescricao').value = DcCategoria;
    }

    if (ocultarRodape == "1") {
        divRodapeCadastroCategoria.style.display = "none";
    } else {
        divRodapeCadastroCategoria.style.display = "block";
    }

    $('#ViewCadastrarCategoriaMdl').modal('show');
}

//===============================================
// FUNÇÃO PARA SALVAR/ALTERAR A CATEGORIA
//===============================================
function inserirCadastroCategoria() {

    var parametros = {
        IdCategoria: $("#txtCategoriaCodigoConsultar").val(),
        DcCategoria: $("#txtCategoriaDescricao").val(),
    };


    if (parametros.DcCategoria == null || parametros.DcCategoria == '') {
        exibirToastErro('Informe a descrição da categoria.');
        return;
    }

    //Pegar a raiz do site para passar o método de controler, se não vai passar na frente do atual e nao vai encontrar
    var url = location.href; //pega endereço que esta no navegador
    url = url.split("/"); //quebra o endereço de acordo com a / (barra)
    var urlRaizSite = url[2]; 

    $.ajax({
        url: "http://" + urlRaizSite + "/Categoria/POSTInserirCategoria",
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

                $('#ViewCadastrarCategoriaMdl').modal('hide');

                //Exibir o alerta de sucesso
                if (parametros.IdCategoria == 0 || parametros.IdCategoria == null) {
                    exibirToastSucesso("Categoria cadastrada com sucesso!");
                } else {
                    exibirToastSucesso("Categoria ID: " + parametros.IdCategoria + " alterada com sucesso!");
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
    var tableExistente = $('#tabCategoria').DataTable();
    if (tableExistente != null) {
        tableExistente.destroy();
    }


    var url = location.href; //pega endereço que esta no navegador
    url = url.split("/"); //quebra o endereço de acordo com a / (barra)
    var urlRaizSite = url[2]; 

    var table = $('#tabCategoria').DataTable({
        pageLength: 25,
        responsive: true,
        dom: '<"html5buttons"B>lTfgitp',
        buttons: [
            { extend: 'excel', title: 'CadastroCategoria' }
        ],

        ajax: "http://" + urlRaizSite + "/Categoria/GETCategoriaDataTable",
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
            }
        ]
    });

};

//===============================================
// FUNÇÃO PARA EXCLUIR CATEGORIA
//===============================================
function excluirCategoria(IdCategoriaParam) {

    var parametros = {
        IdCategoria: IdCategoriaParam
    };

    //Pegar a raiz do site para passar o método de controler, se não vai passar na frente do atual e nao vai encontrar
    var url = location.href; //pega endereço que esta no navegador
    url = url.split("/"); //quebra o endereço de acordo com a / (barra)
    var urlRaizSite = url[2]; 

    $.ajax({
        url: "http://" + urlRaizSite + "/Categoria/POSTDeletarCategoria",
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
                exibirToastSucesso("Categoria ID: " + IdCategoriaParam + " excluída com sucesso!");

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