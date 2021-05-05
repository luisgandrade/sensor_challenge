"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.SensorEventGenerator = void 0;
var SensorEventGenerator = /** @class */ (function () {
    function SensorEventGenerator(http) {
        this.http = http;
        this.random = new RandomSource();
    }
    SensorEventGenerator.prototype.generateEvent = function (tag) {
        var unixTimestamp = new Date().getTime() / 1000 | 0;
        var isError = Math.random() < .3;
        var value = null;
        if (!isError)
            value = Math.random() < .5 ? "string event" : (Math.random() * 100).toString();
        var newSensorEvent = {
            tag: tag,
            timestamp: unixTimestamp,
            value: value
        };
      this.http.post('http://localhost:8080/api/sensorEvent', newSensorEvent);
        //send event
    };
    SensorEventGenerator.prototype.addSensor = function (tag) {
        var _this = this;
        this.sensorsLoops[tag] = setInterval(function () { return _this.generateEvent(tag); }, 1000);
    };
    return SensorEventGenerator;
}());
exports.SensorEventGenerator = SensorEventGenerator;
//# sourceMappingURL=sensor-event-generator.js.map
