#include "everytime.h"

// Definition of the input pins
int PinA1 = 15;
int PinA2 = 16;
int PinA3 = 17;
int PinA4 = 18;
int PinA5 = 19;

int PinD13 = 13; // Set Control Pin

// Definition of the variable storing the value
int ValueA1 = 0;
int ValueA2 = 0;
int ValueA3 = 0;
int ValueA4 = 0;
int ValueA5 = 0;
bool ValueD13 = false;

// Define the digital output pin for the LED
int WARNING_LAMP = 2; //WarningLamp
int ALARM_LAMP = 3; //AlarmLamp

long STATUS_CODE_GOOD = 1; // Everything is good
long STATUS_CODE_WARNING = 2; //Warning
long STATUS_CODE_ALARM = 3; //Alarm

long parsedStatusCode = 0;

bool BuiltinLedState = false;

void setup() {
  Serial.begin(9600);
  pinMode(PinD13, INPUT);
  pinMode(LED_BUILTIN, OUTPUT);
  pinMode(WARNING_LAMP, OUTPUT);
  pinMode(ALARM_LAMP, OUTPUT); 

  // Blink at startup
  digitalWrite(WARNING_LAMP, HIGH);
  digitalWrite(ALARM_LAMP, HIGH);
  delay(500);
  digitalWrite(WARNING_LAMP, LOW);
  digitalWrite(ALARM_LAMP, LOW);
  delay(500);
  digitalWrite(WARNING_LAMP, HIGH);
  digitalWrite(ALARM_LAMP, HIGH);
  delay(500);
  digitalWrite(WARNING_LAMP, LOW);
  digitalWrite(ALARM_LAMP, LOW);
}

void ReadAnalogPins(){
  ValueA1 = analogRead(PinA1);
  ValueA2 = analogRead(PinA2);
  ValueA3 = analogRead(PinA3);
  ValueA4 = analogRead(PinA4);
  ValueA5 = analogRead(PinA5);
}

void SendValuesSerialPort(){
  Serial.flush(); // Waits for the transmission of outgoing serial data to complete
  Serial.print(ValueA1);
  Serial.print(";");
  Serial.print(ValueA2);
  Serial.print(";");
  Serial.print(ValueA3);
  Serial.print(";");
  Serial.print(ValueA4);
  Serial.print(";");
  Serial.print(ValueA5);
  Serial.print(";");
  // Set Parameter 1?
  Serial.print(ValueD13);
  // Set all other Parameters? No!
  Serial.print(";0;0;0;0;");
  Serial.print("\n");
}

void SendSetValue(){
  Serial.flush();
  Serial.print("SET;");
  Serial.print("1;"); // number of the parameter
  Serial.print(ValueA1);
  Serial.println(";");
}

void ReadSerialPort(){
  if (Serial.available() > 0){
    // look for the next valid integer in the incoming serial stream:
    parsedStatusCode = Serial.parseInt();
  }
}

void SetStatusLeds(){
  if(parsedStatusCode == STATUS_CODE_ALARM){ //Chance lamp if warning or alarm
    digitalWrite(WARNING_LAMP, HIGH);
    digitalWrite(ALARM_LAMP, LOW);
  }
  else if(parsedStatusCode == STATUS_CODE_WARNING){
    digitalWrite(WARNING_LAMP, LOW);
    digitalWrite(ALARM_LAMP, HIGH);
  } 
  else if(parsedStatusCode == STATUS_CODE_GOOD){
    digitalWrite(WARNING_LAMP, LOW);
    digitalWrite(ALARM_LAMP, LOW);
  }
}

void BuiltinLedOn(){
  digitalWrite(LED_BUILTIN, HIGH);
  BuiltinLedState = true;
}

void ToggleBuiltinLed(){
  digitalWrite(LED_BUILTIN, !BuiltinLedState);
  BuiltinLedState = !BuiltinLedState;
}

void loop() {
  if(parsedStatusCode == STATUS_CODE_GOOD){
    BuiltinLedOn();
  }
  else if (parsedStatusCode == STATUS_CODE_WARNING){
    every(300){ToggleBuiltinLed();}
  }
  else if (parsedStatusCode == STATUS_CODE_ALARM){
    every(100){ToggleBuiltinLed();}
  }
 
  every(1000){
    ReadAnalogPins();
    SendValuesSerialPort(); 
    ReadSerialPort();
    SetStatusLeds();
  }
}
