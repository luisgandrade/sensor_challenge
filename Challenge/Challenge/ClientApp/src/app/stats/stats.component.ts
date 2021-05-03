import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import * as Highcharts from 'highcharts';
import { EventCountByTag } from '../models/EventCountByTag';
import { SensorNumericEvents } from '../models/SensorNumericEvents';

@Component({
  selector: 'app-counter-component',
  templateUrl: './stats.component.html'
})
export class StatsComponent {
  eventCountByTag: EventCountByTag[] = [];
  constructor(private http: HttpClient) {

    http.get<SensorNumericEvents[]>('http://localhost:14665/api/sensor/numeric-events-data').subscribe(next => {
      var chartOptions: any = {
        chart: {
          zoomType: 'x'
        },
        title: {
          text: 'Eventos numéricos por sensor'
        },
        xAxis: {
          type: 'datetime'
        },
        yAxis: {
          title: {
            text: 'Valor'
          }
        },
        legend: {
          enabled: true
        },
        series: next.map(n => {
          return {
            type: 'line',
            name: n.sensorId,
            data: n.data.map(n1 => [new Date(n1.timestamp).getTime(), n1.value])
          };
        })
      };

      Highcharts.chart('chart', chartOptions);




    }, error => { });

    http.get<EventCountByTag[]>('http://localhost:14665/api/sensor/event-count').subscribe(next => {
      this.eventCountByTag = next;

    }, error => {

    });
  }
}
