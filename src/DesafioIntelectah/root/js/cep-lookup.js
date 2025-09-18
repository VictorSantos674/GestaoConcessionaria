// Script para consulta de CEP via AJAX usando ViaCEP
$(document).ready(function() {
    function preencherEndereco(dados) {
        if (!('erro' in dados)) {
            $("#Endereco").val(dados.logradouro);
            $("#Cidade").val(dados.localidade);
            $("#Estado").val(dados.uf);
        }
    }

    $("#CEP").on('blur', function() {
        var cep = $(this).val().replace(/\D/g, '');
        if (cep.length === 8) {
            $.getJSON("https://viacep.com.br/ws/" + cep + "/json/", function(dados) {
                preencherEndereco(dados);
            });
        }
    });
});
