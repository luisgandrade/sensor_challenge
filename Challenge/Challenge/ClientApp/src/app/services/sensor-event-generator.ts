import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SensorEvent } from '../models/SensorEvent';

@Injectable({
  providedIn: 'root'
})
export class SensorEventGenerator {
  sensorsLoops = {};
  
  constructor(private http: HttpClient) { }


  private generateEvent(tag: string) {
    var unixTimestamp = new Date().getTime() / 1000 | 0;
    var isError = Math.random() < .3;
    var value = null;
    if (!isError)
      value = Math.random() < .5 ? "string event" : (Math.random() * 100).toString();

    var newSensorEvent: SensorEvent = {
      tag: tag,
      timestamp: unixTimestamp,
      value: value
    };

    this.http.post('http://localhost:14665/api/sensorEvent', newSensorEvent).subscribe(next => {
      console.log(next);
    }, error => {
        console.log(error);
    });
    //send event
  }

  addSensor(tag: string) {
    this.sensorsLoops[tag] = setInterval(() => this.generateEvent(tag), 1000);
  }

}
