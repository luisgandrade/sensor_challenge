CREATE DATABASE SensorLog;
GO
USE SensorLog;
GO

CREATE TABLE Sensor(
	id INT IDENTITY(1, 1) PRIMARY KEY,
	country NVARCHAR(20) NOT NULL,
	region NVARCHAR(20) NOT NULL,
	[name] NVARCHAR(20) NOT NULL,
	UNIQUE(country, region, [name]));
	
CREATE TABLE SensorEvent(
	id INT IDENTITY(1, 1) PRIMARY KEY,
	sensorId INT NOT NULL REFERENCES Sensor(id),
	[timestamp] DATETIME2,
	[error] BIT NOT NULL DEFAULT 0,
	[value] NVARCHAR(30),
	valueType INT NOT NULL);
GO