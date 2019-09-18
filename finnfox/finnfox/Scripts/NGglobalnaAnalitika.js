var app = angular.module("globalnaAnalitika", []);

app.controller('globalnaAnalitikaController', function ($scope, $http) {
    //$scope.submit = function (broj) {
    //    alert(broj);
    //};

    //STACKED BAR CHART
    var stackedBarChart;
    var stackedLineChart;
    var nizBoja = [
        '#caf270',
        '#45c490',
        '#008d93',
        '#2e5468',
        '#fff054',
        '#ff0039',
        '#729e7d',
        '#ff8e72',
        '#481d24',
        '#4c243b',
        '#54457f'
    ];

    function konvertorNizaMeseciUslova (nizBrojnihMeseci) {
        var nizSlovnihMeseci = ["jan", "feb", "mar", "apr", "maj", "jun", "jul", "avg", "sep", "okt", "nov", "dec"]
        for (var i = 0; i < nizBrojnihMeseci.length; i++) {
            nizBrojnihMeseci[i] = nizSlovnihMeseci[nizBrojnihMeseci[i] - 1];
        }
    }

    function drawOrUpdateStackedBarChart (godina) {
        $http.get("http://localhost:1091/RacunovodstvenaPromenas/globalnePromeneMesecTip?godina=" + godina).then(function (data) {
            if ($("#globalBarChartCanvas").hasClass("chartjs-render-monitor")) {
                stackedBarChart.destroy();
            }
            konvertorNizaMeseciUslova(data.data.meseci);
            drawStackedChart(data);
        });
    };
    $scope.currentYear = new Date().getFullYear();

    $(document).on('click', '#godine-dropdown-global li a', function ($scope) {
        event.preventDefault();
        event.stopPropagation();
        var kliknutaGodina = $(this).attr("class");
        $("#selected-year").html(kliknutaGodina);
        drawOrUpdateStackedBarChart(kliknutaGodina);
    });
    

    drawOrUpdateStackedBarChart($scope.currentYear);

    function addDataSet(chart, label, color, data, fillBool) {
        chart.data.datasets.push({
            label: label,
            backgroundColor: color,
            borderColor: color,
            data: data,
            fill: fillBool
        });
        chart.update();
    };

    function drawStackedChart(data) {
        //console.log(data.data.meseci);
        //console.log(data.data.kategorije);
        //console.log(data.data.vrednostiPoKategoriji);
        
        var stackedBarCanvas = document.getElementById('globalBarChartCanvas');

        stackedBarChart = new Chart(stackedBarCanvas, {
            type: 'bar',
            data: {
                labels: data.data.meseci,
                datasets: []
            },
            options: {
                scales: {
                    xAxes: [{
                        stacked: true
                    }],
                    yAxes: [{
                        stacked: true
                    }]
                }
            }
        });

        for (var i = 0; i < data.data.kategorije.length; i++) {
            addDataSet(stackedBarChart, data.data.kategorije[i], nizBoja[i], data.data.vrednostiPoKategoriji[i], true);
        }
    };

    $http.get("http://localhost:1091/RacunovodstvenaPromenas/godineSviKorisnici").then(function (result) {
        $scope.godineSviKorisnici = result.data;
    });

    //LINE CHART

    function drawOrUpdateStackedLineChart() {
        $http.get("http://localhost:1091/RacunovodstvenaPromenas/globalniProcentiUstede").then(function (response) {
            console.log(response.data);
            if ($("#globalLineChartCanvas").hasClass("chartjs-render-monitor")) {
                stackedBarChart.destroy();
            }
            konvertorNizaMeseciUslova(response.data.meseci);
            drawLineChart(response.data);
        });
    };
    drawOrUpdateStackedLineChart();

    function drawLineChart(data) {
        //console.log(data.data.meseci);
        //console.log(data.data.kategorije);
        //console.log(data.data.vrednostiPoKategoriji);

        var stackedLineCanvas = document.getElementById('globalLineChartCanvas');

        stackedLineChart = new Chart(stackedLineCanvas, {
            type: 'line',
            data: {
                labels: data.meseci,
                datasets: []
            },
            options: {
                title: {
                    display: true,
                    text: 'Prosečna štednja FinnFox korisnika po mesecima (u %)'
                }
            }
        });

        for (var i = 0; i < data.kategorije.length; i++) {
            addDataSet(stackedLineChart, data.kategorije[i], nizBoja[i], data.vrednostiPoKategoriji[i], false);
        }
    };


});