import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { HighchartsChartModule } from 'highcharts-angular';
/*import { NgbModule } from '@ng-bootstrap/ng-bootstrap';*/

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { StatsComponent } from './stats/stats.component';
import { AddSensorComponent } from './add-sensor/add-sensor.component';
import { EventLogComponent } from './event-log/event-log.component';
import { SensorEventGenerator } from './services/sensor-event-generator';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    StatsComponent,
    AddSensorComponent,
    EventLogComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    HighchartsChartModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'add-sensor', component: AddSensorComponent },
      { path: 'event-log', component: EventLogComponent },
      { path: 'stats', component: StatsComponent },
    ])
  ],
  providers: [SensorEventGenerator],
  bootstrap: [AppComponent]
})
export class AppModule { }
