import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { Sensor } from '../models/Sensor';
import { NgForm } from '@angular/forms';
import { SensorEventGenerator } from '../services/sensor-event-generator';
import { environment } from '../../environments/environment'

@Component({
  selector: 'app-add-sensor-component',
  templateUrl: './add-sensor.component.html'
})
export class AddSensorComponent {
  sensor: Sensor = {
    id: null,
    country: '',
    region: '',
    name: ''
  };
  sensorAlreadyExists = false;

  constructor(private http: HttpClient, private sensorEventGenerator: SensorEventGenerator) {}

  submit(f: NgForm) {
    if (f.submitted && f.invalid)
      return;
    this.sensorAlreadyExists = false;
    this.http.post(environment.apiBaseUrl + 'sensor', this.sensor).subscribe(_ => {
      this.sensorEventGenerator.addSensor(this.sensor.country + '.' + this.sensor.region + '.' + this.sensor.name);
      alert('Sensor registrado com sucesso');
      this.sensor.country = '';
      this.sensor.region = '';
      this.sensor.name = '';
    }, error => {
        this.sensorAlreadyExists = true;
    })
  }
}
