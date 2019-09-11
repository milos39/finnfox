var app = angular.module("racunovodstvenePromene", []);

app.controller('racunovodstvenePromeneController', function ($scope, $http) {

    function dinamicanNaslov(godina) {
        //console.log("dinamicki naslov parametar je: " + godina)
        if (godina == 0) {
            $("#godina-naslov").html("Kompletna finansijska istorija");
        } else {
            $("#godina-naslov").html("Finansije u " + godina);
        }
    }

    function loadPromene(godina) {
        godina = godina || 0;
        $http.get("http://localhost:1091/RacunovodstvenaPromenas/promenePoGodini?godina=" + godina).then(function (result) {
            console.log(result);
            var tabela = $("#promeneTabela");
            $scope.promenePoGodini = result.data;
            $("#promeneTabela").bootstrapTable({ data: result.data.racunovodstvenePromene });
            tabela.bootstrapTable('load', result.data.racunovodstvenePromene);
            drawOrUpdateChart(godina);
            dinamicanNaslov(godina);
        });
 
    }
    loadPromene();




    $(document).on('click', '#godine-dropdown li a', function () {
        event.preventDefault();
        event.stopPropagation();
        var kliknutaGodina = $(this).attr("class");
        if (kliknutaGodina.length == 4 || kliknutaGodina == 0) {
            loadPromene(kliknutaGodina);
            //drawOrUpdateChart(kliknutaGodina);
            dinamicanNaslov(kliknutaGodina);
        } else {
            return;
        }
        
    });
});