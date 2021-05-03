import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Sensor } from '../models/Sensor';
import { NgForm } from '@angular/forms';
import { SensorEventGenerator } from '../services/sensor-event-generator';

@Component({
  selector: 'app-counter-component',
  templateUrl: './add-sensor.component.html'
})
export class AddSensorComponent {
  sensor: Sensor = {
    id: null,
    country: "",
    region: "",
    name: ""
  };
  constructor(private http: HttpClient, private sensorEventGenerator: SensorEventGenerator) {}

  submit(f: NgForm) {
    if (f.submitted && f.invalid)
      return;

    this.http.post('http://localhost:14665/api/sensor', this.sensor).subscribe(_ => {
      this.sensorEventGenerator.addSensor(this.sensor.country + '.' + this.sensor.region + '.' + this.sensor.name)
    }, error => {
        alert(error.error);
    })
  }
}
