var app = angular.module("racunovodstvenePromene", []);

app.controller('racunovodstvenePromeneController', function ($scope, $http) {

    function dinamicanNaslov(godina, mesec) {
        //console.log("dinamicki naslov parametar je: " + godina)
        var imenaMeseci = ["januaru", "februaru", "martu", "aprilu", "maju", "junu", "julu", "avgustu", "septembru", "oktobru", "novembru", "decembru"];
        if (godina == 0) {
            $("#godina-naslov").html("<span id='prikazanaGodina' class='hidden'>0</span>Kompletna finansijska istorija");
        } else {
            if (mesec === undefined || mesec == 0) {
                console.log("mesec je" + mesec);
                $("#godina-naslov").html("Finansije u <span id='prikazanaGodina'>" + godina + "</span>");
            } else {
                $("#godina-naslov").html("Finansije u <span id='prikazaniMesec'>" + imenaMeseci[mesec-1] + "</span> <span id='prikazanaGodina'>" + godina + "</span> godine");
            }
        }
    }

    function loadPromene(godina, mesec) {
        godina = godina || 0;
        function popuniTabelu(result, godina, mesec) {
            var tabela = $("#promeneTabela");
            $scope.promenePoGodini = result.data;
            $("#promeneTabela").bootstrapTable({ data: result.data.racunovodstvenePromene });
            tabela.bootstrapTable('load', result.data.racunovodstvenePromene);
            drawOrUpdateChart(godina, mesec);
            dinamicanNaslov(godina, mesec);
        };

        if (mesec === undefined || mesec == 0) {
            $http.get("http://localhost:1091/RacunovodstvenaPromenas/promenePoGodini?godina=" + godina).then(function (result) {
                console.log(result);
                popuniTabelu(result, godina, mesec);
            });
        } else {
            $http.get("http://localhost:1091/RacunovodstvenaPromenas/promenePoMesecu?godina=" + godina + "&mesec=" + mesec).then(function (result) {
                console.log(result);
                popuniTabelu(result, godina, mesec);
            });
        };
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
            $http.get("http://localhost:1091/RacunovodstvenaPromenas/meseciZaGodinu?godina=" + kliknutaGodina).then(function (result) {
                $scope.meseciZaDatuGodinu = result.data;
            });
        } else {
            return;
        }
        
    });

    $(document).on('click', '#meseci-dropdown li a', function () {
        event.preventDefault();
        event.stopPropagation();
        var kliknutiMesec = $(this).attr("class");
        var izabranaGodina = $("#prikazanaGodina").html();
        console.log(izabranaGodina);
        if (kliknutiMesec.length > 0 && kliknutiMesec.length < 3 ) {
            loadPromene(izabranaGodina, kliknutiMesec);
            //dinamicanNaslov(kliknutaGodina); HALP
        } else {
            return;
        }

    });
});