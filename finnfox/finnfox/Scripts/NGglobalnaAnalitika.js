var app = angular.module("globalnaAnalitika", []);

app.controller('globalnaAnalitikaController', function ($scope, $http) {
    console.log("ucitan kontroler");
    //$.get("http://localhost:1091/RacunovodstvenaPromenas/globalnePromeneMesecTip?godina=2019", function (data, status) {
    //    console.log(data);
    //});

    $scope.submit = function (broj) {
        alert(broj);
    };

    $scope.drawOrUpdateStackedBarChart = function (godina) {
        $http.get("http://localhost:1091/RacunovodstvenaPromenas/globalnePromeneMesecTip?godina=" + godina).then(function (data) {
            //console.log(data);
            if ($("#globalChartCanvas").hasClass("chartjs-render-monitor")) {
                stackedBarChart.destroy();
            }
            drawStackedChart(data);
        });
    };

    function drawStackedChart(data) {
        console.log(data.data.meseci);
        console.log(data.data.kategorije);
        console.log(data.data.vrednostiPoKategoriji);
        
        var stackedBarCanvas = document.getElementById('globalChartCanvas');
        var nizBoja = [
            '#caf270',
            '#45c490',
            '#008d93',
            '#2e5468',
            '#ffe900',
            '#ff0039',
            '#a1ff66'
        ];
        var stackedBarChart = new Chart(stackedBarCanvas, {
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

        function addDataSet(chart, label, color, data) {
            chart.data.datasets.push({
                label: label,
                backgroundColor: color,
                data: data
            });
            chart.update();
        };

        for (var i = 0; i < data.data.kategorije.length; i++) {
            addDataSet(stackedBarChart, data.data.kategorije[i], nizBoja[i], data.data.vrednostiPoKategoriji[i]);
        }
    };

});