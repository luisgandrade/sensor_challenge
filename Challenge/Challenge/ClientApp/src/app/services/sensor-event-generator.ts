import { HttpClient } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { Sensor } from '../models/Sensor';
import { SensorEventForInsertion } from '../models/SensorEventForInsertion';

@Injectable({
  providedIn: 'root'
})
export class SensorEventGenerator{
  sensorsLoops = {};
  private errorRate = 0.3;
  private numericSuccessfulEventRate = 0.5;

  constructor(private http: HttpClient) {
    this.http.get<Sensor[]>('http://localhost:14665/api/sensor').subscribe(sensors => {
      for (let sensor of sensors) {
        let tag = sensor.country + '.' + sensor.region + '.' + sensor.name;
        this.sensorsLoops[tag] = setInterval(() => this.generateEvent(tag), 3000);
      }

    })

  }

  private generateEvent(tag: string) {
    var unixTimestamp = new Date().getTime() / 1000 | 0;
    var isError = Math.random() < this.errorRate;
    var value = null;
    if (!isError)
      value = Math.random() < this.numericSuccessfulEventRate ? (Math.random() * 100).toString() : "string event";

    var newSensorEvent: SensorEventForInsertion = {
      tag: tag,
      timestamp: unixTimestamp,
      value: value
    };

    this.http.post('http://localhost:14665/api/sensorEvent', newSensorEvent).subscribe();
    //  _ => {
      
    //}, error => {
    //    console.log(error);
    //});
  }

  addSensor(tag: string) {
    this.sensorsLoops[tag] = setInterval(() => this.generateEvent(tag), 3000);
  }

}
