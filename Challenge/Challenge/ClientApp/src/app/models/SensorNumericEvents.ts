export interface NumericEventData {
  timestamp: string,
  value: number
}

export interface SensorNumericEvents {
  sensorId: number,
  sensorTag: string,
  data: NumericEventData[]
};
