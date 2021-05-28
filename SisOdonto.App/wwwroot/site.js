function SetModal() {

    $(document).ready(function () {
        $(function () {
            $.ajaxSetup({ cache: false });
            $(document).on('click', 'a[data-modal]', function (e) {
                $('#myModalContent').load(this.href,
                    function () {
                        $('#myModal').modal({
                                keyboard: true
                            },
                            'show');
                        bindForm(this);
                    });
                return false;
            });
        });
    });
}

function bindForm(dialog) {
    $('form', dialog).submit(function () {
        $("#loadingoverlay").fadeIn();
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                $("#loadingoverlay").fadeOut();
                if (result.success) {
                    toastr.success(result.messageText);
                    $('#myModal').modal('hide');
                    $.get(result.url);
                    location.reload();
                } else {
                    $('#myModalContent').html(result);
                    toastr.error('Dados Inválidos!');
                    bindForm(dialog);
                }
            }
        });

        SetModal();
        return false;
    });
}


    function GetExpertise(Sle) {
        $.ajax({
            url: "/Scheduling/GetExpertise",
            data: { "dentistId": Sle.value },
            type: "GET",
            success: function (result) {
                $("#TypeExpertise").val(result.data);

            },
            error: function (data) {
                alert(data);
            }
        });
    }
 

function GetDataPacient() {
        $(document).ready(function () {

            function limpa_formulário_pacient() {
                $("#Patient_Name").val("");
                $("#Patient_BirthDate").val("");
                $("#Patient_Street").val("");
                $("#Patient_Number").val("");
                $("#Patient_Complement").val("");
                $("#Patient_District").val("");
                $("#Patient_City").val("");
                $("#Patient_State").val("");
            }

            $("#Patient_Cpf").blur(function (e) {

                var cpf = $(this).val().replace(/\D/g, '');

                if (cpf != "") {

                    var validacpf = /^[0-9]{11}$/;

                    if (validacpf.test(cpf)) {

                        $("#Patient_Name").val("...");
                        $("#Patient_BirthDate").val("...");
                        $("#Patient_Street").val("...");
                        $("#Patient_Number").val("...");
                        $("#Patient_Complement").val("...");
                        $("#Patient_District").val("...");
                        $("#Patient_City").val("...");
                        $("#Patient_State").val("..");


                        $.ajax({
                            url: "Scheduling/DetailsByCpf",
                            type: "GET",
                            data: { cpf: cpf },
                            success: function (result) {
                                if (result.success) {
                                    $("#Patient_Id").val(result.data.id);
                                    $("#Patient_Name").val(result.data.name);
                                    $("#Patient_BirthDate").val(result.data.birthDate);
                                    $("#Patient_Street").val(result.data.street);
                                    $("#Patient_Number").val(result.data.number);
                                    $("#Patient_Complement").val(result.data.complement);
                                    $("#Patient_District").val(result.data.district);
                                    $("#Patient_City").val(result.data.city);
                                    $("#Patient_State").val(result.data.state);
                                } else {
                                    limpa_formulário_pacient();
                                    toastr.error("CPF não encontrado.");
                                }
                            },
                            error: function (jqXHR, textStatus) {
                                limpa_formulário_pacient();
                                toastr.error("Erro ao consultar CPF");
                            }
                        });

                        e.stopImmediatePropagation();

                    } else {
                        limpa_formulário_pacient();
                        toastr.error("CPF em formato inválido");
                    }
                }
                else {
                    limpa_formulário_pacient();
                }
            });
        });
}

function BuscaCep() {
    $(document).ready(function () {

        function limpa_formulário_cep() {
            // Limpa valores do formulário de cep.
            $("#Street").val("");
            $("#District").val("");
            $("#City").val("");
            $("#State").val("");
        }

        //Quando o campo cep perde o foco.
        $("#Cep").blur(function () {

            //Nova variável "cep" somente com dígitos.
            var cep = $(this).val().replace(/\D/g, '');

            //Verifica se campo cep possui valor informado.
            if (cep != "") {

                //Expressão regular para validar o CEP.
                var validacep = /^[0-9]{8}$/;

                //Valida o formato do CEP.
                if (validacep.test(cep)) {

                    //Preenche os campos com "..." enquanto consulta webservice.
                    $("#Street").val("...");
                    $("#District").val("...");
                    $("#City").val("...");
                    $("#State").val("...");

                    //Consulta o webservice viacep.com.br/
                    $.getJSON("https://viacep.com.br/ws/" + cep + "/json/?callback=?",
                        function (dados) {

                            if (!("erro" in dados)) {
                                //Atualiza os campos com os valores da consulta.
                                $("#Street").val(dados.logradouro);
                                $("#District").val(dados.bairro);
                                $("#City").val(dados.localidade);
                                $("#State").val(dados.uf);
                            } //end if.
                            else {
                                //CEP pesquisado não foi encontrado.
                                limpa_formulário_cep();
                                alert("CEP não encontrado.");
                            }
                        });
                } //end if.
                else {
                    //cep é inválido.
                    limpa_formulário_cep();
                    alert("Formato de CEP inválido.");
                }
            } //end if.
            else {
                //cep sem valor, limpa formulário.
                limpa_formulário_cep();
            }
        });
    });
}



$(document).ready(function () {

   
    $("#msg_box").fadeOut(2500);
});