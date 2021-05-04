import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import * as Highcharts from 'highcharts';
import { EventCountByTag } from '../models/EventCountByTag';
import { SensorNumericEvents } from '../models/SensorNumericEvents';

@Component({
  selector: 'app-stats-component',
  templateUrl: './stats.component.html'
})
export class StatsComponent implements OnInit{
  eventCountByTag: EventCountByTag[] = [];
  hasData = true;
  errorOnFetchChartData = false;
  errorOnFetchCountData = false;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.http.get<SensorNumericEvents[]>('http://localhost:14665/api/sensor/numeric-events-data').subscribe(next => {
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
      this.hasData = this.hasData && next && next.length > 0;



    }, _ => this.errorOnFetchChartData = true);

    this.http.get<EventCountByTag[]>('http://localhost:14665/api/sensor/event-count').subscribe(next => {
      this.eventCountByTag = next;
      this.hasData = this.hasData &&  next && next.length > 0;
    }, _ => this.errorOnFetchCountData = true);
  }


}
