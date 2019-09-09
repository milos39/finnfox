var app = angular.module("racunovodstvenePromene", []);

app.controller('racunovodstvenePromeneController', function ($scope, $http) {
    function loadPromene(godina) {
        godina = godina || 0;

        $http.get("http://localhost:1091/RacunovodstvenaPromenas/promenePoGodini?godina=" + godina).then(function (result) {
            console.log(result);
            var tabela = $("#promeneTabela");
            $scope.promenePoGodini = result.data;
            $("#promeneTabela").bootstrapTable({ data: result.data.racunovodstvenePromene });
            tabela.bootstrapTable('load', result.data.racunovodstvenePromene);

            if (godina !== 0) {
                $("#godina-naslov").html("Finansije u " + godina);
            } else {
                $("#godina-naslov").html("Kompletna finansijska istorija");
            }
        });
        
    }
    loadPromene();
    //$("#godine-dropdown").click(function (event) {
    $(document).on('click', '#godine-dropdown li a', function () {
        event.preventDefault();
        event.stopPropagation();
        var kliknutaGodina = $(this).attr("class");
        if (kliknutaGodina.length == 4 || kliknutaGodina == 0) {
            console.log("uspeh jer je kliknut: " + kliknutaGodina + ' !');
            loadPromene(kliknutaGodina);
            //drawOrUpdateChart(kliknutaGodina);
        } else {
            console.log("neuspeh jer je kliknut: " + kliknutaGodina);
            console.log(kliknutaGodina.length);
            return;
        }
        

        console.log("clicked " + kliknutaGodina);
        
    });
});