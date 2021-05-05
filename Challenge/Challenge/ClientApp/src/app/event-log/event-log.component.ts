import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { SensorEventForDisplay } from '../models/SensorEventForDisplay';
import { environment } from '../../environments/environment'

@Component({
  selector: 'app-event-log.component',
  templateUrl: './event-log.component.html'
})
export class EventLogComponent implements OnInit{
  events: SensorEventForDisplay[] = [];
  lastUpdate: Date;
  errorOnFetchData = false;
  constructor(private http: HttpClient) { }


  private updateTable(httpClient: HttpClient) {
    httpClient.get<SensorEventForDisplay[]>(environment.apiBaseUrl + 'sensorEvent/all-events?from=' + this.lastUpdate.toISOString()).subscribe(next => {
      this.events.push(...next);
    }, _ => {
        this.errorOnFetchData = true;
    });
    this.lastUpdate = new Date();
  }

  ngOnInit() {
    this.lastUpdate = new Date();
    this.http.get<SensorEventForDisplay[]>(environment.apiBaseUrl + 'sensorEvent/all-events').subscribe(next => {
      this.events = next;
      setInterval(() => this.updateTable(this.http), 3000);
    }, _ => {
        this.errorOnFetchData = true;
    });


  }
}
