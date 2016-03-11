/*
 * File:   Arduino_Program.ino 
 * 
 * Author: Joshua Bowerman (bowermaj@uwindsor.ca)
 * Date:   March 2016
 * 
 * Program Responsibilities:
 *    1. Calculate motor throttle position.
 *    2. Read 2 speed sensors/tachometers.
 *    3. Read fuel level sensor.
 *    4. Listen to ECU over serial connection.
 *    5. Send information to dashboard via serial over USB.
 *    
 * Summary of File:
 *    
 *    This program is written for the arduino mega 2560 in UWindsor's formula hybrid car. 
 *    It reads the two speed sensors connected to the two front wheels of the car and from
 *    that it calculates the speed of the vehicle. It reads the fuel level sensor to determine 
 *    the amount of fuel left. It also relays the serial data from the car's ECU after decoding 
 *    it and sends that to the dashboard controller in a more friendly format. It then sends 
 *    all gathered data to the dashboard via serial communications over the USB cable. It uses
 *    all gathered data as well as past data to calculate the required motor throttle position.
 *    
 * NOTE: TIMERONE FUNCTIONALITY HAS BEEN DISABLED AS WE ONLY CAN RECEIVE INFORMATION SO FAST, THERE IS NO POINT
 * Important Note:  DO NOT USE PINS NUMBERED 11,12, OR 13! THESE ARE TIMER PINS!
 * 
 *    
 * Note: 
 * 
 *    The excessive amount of commenting in this file isn't because I actually think that all 
 *    that needs to be commented its to keep everything orginized. If you make any changes please,
 *    please please please update all relevant comments, document what you do, and add your name as
 *    another author leaving mine and whoever else is on the header.
 *    
 * 
 * See Also : 
 *    1. ECU serial information  --->  http://www.ecotrons.com/files/Ecotrons%20Broadcast%20data%20list%20on%20RS232.pdf
 *    2. Timer Interupts Library --->  https://github.com/PaulStoffregen/TimerOne
 */

//#include "TimerOne.h"

//Constants
const int    NumSpokes = 36;            // The Number of sensor spokes on the wheel, or the number of pulses per one revolution of the wheel
const int    Sensor1Pin = 2;            // The Pin Connected to the First Wheel Speed Sensor. Must be one of 2, 3, 18, 19, 20, 21 as these can be used with interupts
const int    Sensor2Pin = 3;            // The Pin Connected to the Second Wheel Speed Sensor. Must be one of 2, 3, 18, 19, 20, 21 as these can be used with interupts
//const int    DashUpdateFreq = 25;       // The Time Between Sending Data to the Dash in MilliSeconds. Do not set to anything higher than 20 seconds or there could be overflow.
const long   USBSerialBaud = 115200L;   // The baud rate of the connection to the raspberry pi over the usb cable
const long   RS232SerialBaud = 115200L; // The baud rate of the connection to the ECU over the rs232 cable
const float  WheelCirc = 163.5827;      // The circumfrence of the wheels, or the distance travelled after one rotation of the wheels in cm (circ = 2 * pi * (radius))
const int    ThrottlePin = 9;           // The pin that will output the throttle position as a PWM. must be a pwm pin, any other pin will not work.
const int    FuelSensorPin = A0;        // The pin that the Pressure Level Sensor is connected to, Note: this must be an analog input pin eg. A0,A1,A2,A3 ... A15
const byte   FuelEmptyValue = 0;        // The value read from the Pressure Level Sensor when the fuel tank is empty.
const byte   FuelFullValue = 255;       // The value read from the Pressure Level Sensor when the fuel tank is full.
const byte   Motor_Speed_Cutoff = 60;   // The speed(in KPH) at which we should no longer use the motor for anything.
const String OpMode_Header = "OP:";     // The header that will be received on the message indicating we need to change operation modes.
const String DataBurst_Header = "DB:";  // The header we send to the dash that indicates that we are sending it data information.
const int    TimeToFullThrottle = "


//Sensor and Serial Communication Variables
         String SerialUSB_Buffer = "";     // The buffer of received Bytes from the USB serial connection with the Dashboard.
         String SerialRs232_Buffer = "";   // The buffer of received Bytes from the Rs232 serial connection with the ECU.
volatile int    Sensor1TCount = 0;         // Number of ticks of the tachometer. The Left Wheel Sensor. Counted by an interupt. Used in the Data Broadcast Functions.
volatile int    Sensor2TCount = 0;         // Number of ticks of the tachometer. The Right Wheel Sensor. Counted by an interupt. Used in the Controller functions.
volatile int    Current_Speed = 0;         // The current speed of the vehicle in KPH at a factor of 0.01, eg: 12.33KM/H = 1233 stored
volatile double MotorThrottlePos = 0.0;    // The current throttle position as decided by the motor controls. This values is a percent, min = 0, max = 100
volatile int    Serial_TempCoolant = 0;    // The Coolant temp as sent to us by the ECU, stored at a factor of 0.01, eg: 1.23C = 123 Stored
volatile int    Serial_TempIntake = 0;     // The Intake Air Temperature, sent by the ECU, stored at a factor of 0.01, eg: 1.23C = 123 Stored
volatile int    Serial_EngineThrottle = 0; // The engine throttle position as a percentage, sent by the ECU. stored at a factor of 0.0001. eg: 12.3456% = 123456 Stored
volatile int    Serial_ExhaustOxygen = 0;  // The oxygen emissions of the IC engine, stored in volts. factor of 0.001. eg: 2.34V = 2340 Stored
volatile int    Serial_EngineRPM = 0;      // The RPM of the IC engine, stored in Revs per Minute. Factor of 1. eg: 2000RPM = 2000 Stored
volatile byte   Motor_Op_Mode = B0;        // The running mode of the motor; 1 is normal, 2 is motor off, and 3 is Motor only.
volatile String Serial_Error = "";         // This is sent to the dashboard, it should be simple sentance or two with no new line characters. Note: NO NEW LINES

//Motor Controller Functionality Variables
bool LastRPM = 0;      //The previous engine RPM value, updated every 100ms.
bool LastThrottle = 0; // The Last throttle position value received by the ecu, updated every 100ms.



/*
 * void setup()
 * 
 * Summary:
 *    This function is called one time when the arduino is turned on. The purpose of this
 *    function is to initiate all of the pins used and to setup serial communication. It
 *    also sets up the interupts for the tachometer sensors.
 */
void setup() {
  //Setup Serial Communications
  Serial.begin(USBSerialBaud);
  Serial1.begin(RS232SerialBaud);
  Serial.println("Begin");
  //Setup Pin Modes
  pinMode(Sensor1Pin,INPUT);
  pinMode(Sensor2Pin,INPUT);
  pinMode(ThrottlePin,OUTPUT);

  //Setup Interupts For Sensor Reading
  attachInterrupt(digitalPinToInterrupt(Sensor1Pin),Sensor1T,RISING);
  attachInterrupt(digitalPinToInterrupt(Sensor2Pin),Sensor2T,RISING);

  //Setup Interupts For Serial Broadcasts
  //Timer1.initialize(1000 * DashUpdateFreq); // 1000 microseconds in a millisecond
  //Timer1.attachInterrupt(DashUpdateTick);

  //Reserve Communication Buffers
  SerialUSB_Buffer.reserve(200);
  SerialRs232_Buffer.reserve(200);
}
void loop(){
  
}
/*
 * void ControllerTick()
 * 
 * Summary:
 *    Runs every time we receive new data, its purpose is to determine the correct throttle position,
 *    as well as make sure we do not exceed the speed cutoff for the motor.
 *    
 */
void ControllerTick(){
  
}

/*
 * void DashUpdateTick()
 * 
 * 
 * NOTE: TIMERONE FUNCTIONALITY HAS BEEN DISABLED AS WE ONLY CAN RECEIVE INFORMATION SO FAST, THERE IS NO POINT
 * NOTE: THIS IS NOW CALLED BY THE SERIAL MESSAGE RECEIVED FUNCTION
 * 
 * Summary:
 *    Runs at the specified interval set by the constant DashUpdateFreq. It's purpose
 *    is to transmit all of the gathered data to the dashboard using the Serial port
 *    on the USB cable.
 *    
 * Note: It is attached to the interrupt using Timer1
 */
void DashUpdateTick() {
  noInterrupts(); // do not interupt the trnamission
  String Message = "DB," + String(Serial_EngineRPM) + "," + String(SensorToSpeed(Sensor1TCount_A,DashUpdateFreq)) + "," + String(Serial_EngineThrottle) + ",";
  Message += String(MotorThrottlePos) + ",0," + String(Serial_TempCoolant) + "," + String(Serial_TempIntake) + "," + String(Motor_Op_Mode) + "," + Serial_Error;
  Serial.println(Message);
  
}


/*
 * void USBSerialReceived(String Message)
 * 
 * Summary:
 * 
 *    Called when we receive a line of text over the USB serial connection. It then
 *    Interprets the line and decides what action to take.
 */
void USBSerialReceived(String Message){
  
}

/*
 * void Rs232SerialReceived(String Message)
 * 
 * Summary:
 * 
 *    Called when we receive a line of text over the RS232 serial connection. It then
 *    Interprets the line and decides what action to take.
 */
void Rs232SerialReceived(String Message){
  //Disect Message
  noInterrupts();

  interrupts();
  
  //Update Speed
  SensorToSpeed();
  
  //Motor Controller Functionality
  ControllerTick();
  
  //Data Broadcast
  DashUpdateTick();
}

/*
 * void SerialEvent()
 * 
 * Summary:
 *    
 *    Is called everytime we receive a byte over the USB serial connection, it then
 *    adds it to the buffer and calls the proper message received function when it  
 *    receives the new line character.
 */
void SerialEvent(){
  while (Serial.available()) {
    char NewChar = (char)Serial.read();
    if (NewChar == '\n') {
      USBSerialReceived(SerialUSB_Buffer);  // Process Message
      SerialUSB_Buffer = "";                // Empty Buffer
    }else{
      SerialUSB_Buffer += NewChar;
    } 
  }
}
/*
 * void Serial1Event()
 * 
 * Summary:
 *    
 *    Is called everytime we receive a byte over the RS232 serial connection, it then
 *    adds it to the buffer and calls the proper message received function when it  
 *    receives the new line character.
 */
void Serial1Event(){
  while (Serial1.available()) {
    char NewChar = (char)Serial1.read();
    if (NewChar == '\n') {
      Rs232SerialReceived(SerialRs232_Buffer);    // Process Message
      SerialRs232_Buffer = "";                    // Empty Buffer
    }else{
      SerialRs232_Buffer += NewChar;
    } 
  }
}

/*
 * void SensorToSpeed()
 * 
 * Summary:
 *    This converts the number of ticks over a period to Meters Per Hour, It uses the wheel
 *    Information Variables to accomplish this, it aproximates pi to 3.1415
 *    
 */
 void SensorToSpeed(){
  noInterrupts();
  double Wheel1DistTrav = ((double)Sensor1TCount / NumSpokes) * WheelCirc;
  double Wheel2DistTrav = ((double)Sensor2TCount / NumSpokes) * WheelCirc;

  double WheelTravAvg = (Wheel1DistTrav + Wheel2DistTrav) / 2;

  const  WheelAvgKPH = WheelTravAvg * 10 * 60 * 60 * 100 * 1000; // Assuming 100ms each time this is called, value in km per hour

  Sensor1TCount = 0;
  Sensor2TCount = 0;
  
  Current_Speed = 100 * WheelAvgKPH;
  interrupts();
 }
/*
 * void Sensor1T()
 * 
 * Summary:
 *    Runs everytime the pin for Sensor 2 moves from LOW to HIGH state.
 *    This is used to calculate the speed at each of the wheels.
 *    
 * Note: Is attached to an interupt on Sensor1Pin Rising
 */
void Sensor1T(){
  noInterrupts();
  Sensor1TCount_A++;
  Sensor1TCount_B++;
  interrupts();
}

/*
 * void Sensor2T()
 * 
 * Summary:
 *    Runs everytime the pin for Sensor 2 moves from LOW to HIGH state.
 *    This is used to calculate the speed at each of the wheels.
 *    
 * Note: Is attached to an interupt on Sensor2Pin Rising
 */
void Sensor2T(){
  noInterrupts();
  Sensor2TCount_A++;
  Sensor2TCount_B++;
  interrupts();
}

