import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Sensor } from '../models/Sensor';
import { NgForm } from '@angular/forms';

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
  constructor(private http: HttpClient) {}

  submit(f: NgForm) {
    if (f.submitted && f.invalid)
      return;

    this.http.post('http://localhost:14665/api/sensor', this.sensor).subscribe(x => console.log(x))
  }
}
