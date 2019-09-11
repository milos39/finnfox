var app = angular.module("racunovodstvenePromene", []);

app.controller('racunovodstvenePromeneController', function ($scope, $http) {

    function dinamicanNaslov(godina, mesec) {
        //console.log("dinamicki naslov parametar je: " + godina)
        var imenaMeseci = {
            1: "januaru",
            2: "februaru",
            3: "martu",
            4: "aprilu",
            5: "maju",
            6: "junu",
            7: "julu",
            8: "avgustu",
            9: "septembru",
            10: "oktobru",
            11: "novembru",
            12: "decembru",  
        };

        if (godina == 0) {
            $("#godina-naslov").html("<span id='prikazanaGodina' class='hidden'>0</span>Kompletna finansijska istorija");
        } else {
            if (mesec == 0) {
                $("#godina-naslov").html("Finansije u <span id='prikazanaGodina'>" + godina + "</span>");
            } else {
                $("#godina-naslov").html("Finansije u <span id='prikazaniMesec'>" + imenaMeseci.mesec + "</span> <span id='prikazanaGodina'>" + godina + "</span> godine");
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

        if (mesec === undefined) {
            $http.get("http://localhost:1091/RacunovodstvenaPromenas/promenePoGodini?godina=" + godina).then(function (result) {
                console.log(result);
                popuniTabelu(result, godina, mesec);
            });
        } else {
            $http.get("http://localhost:1091/RacunovodstvenaPromenas/promenePoMesecu?godina=" + godina + "&mesec=" + mesec).then(function (result) {
                console.log(result);
                popuniTabelu(result, godina, mesec);
                $scope.meseciZaDatuGodinu = result.data.meseciZaDatuGodinu;
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
        } else {
            return;
        }
        
    });

    $(document).on('click', '#meseci-dropdown li a', function () {
        event.preventDefault();
        event.stopPropagation();
        var kliknutiMesec = $(this).attr("class");
        var izabranaGodina = $("#prikazanaGodina").html();
        if (kliknutiMesec.length == 4 || kliknutiMesec == 0) {
            loadPromene(izabranaGodina, kliknutiMesec);
            //dinamicanNaslov(kliknutaGodina); HALP
        } else {
            return;
        }

    });
});