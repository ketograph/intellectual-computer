// Definition of the input pins
int PinA1 = 14;
int PinA2 = 15;
int PinA3 = 16;
int PinA4 = 17;
int PinA5 = 18;

// Definition of the variable storing the value
int ValueA1 = 0;
int ValueA2 = 0;
int ValueA3 = 0;
int ValueA4 = 0;
int ValueA5 = 0;

// Define the digital output pin for the LED
int WARNING_LAMP = 2; //WarningLamp
int ALARM_LAMP = 3; //AlarmLamp

long parsedStatusCodeWarning = 1; //Warning
long parsedStatusCodeAlarm = 2; //Alarm

void setup() {
  Serial.begin(9600);
  pinMode(WARNING_LAMP, OUTPUT);
  digitalWrite(WARNING_LAMP, HIGH);
  pinMode(ALARM_LAMP, OUTPUT); //
  digitalWrite(ALARM_LAMP, HIGH);//
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
  Serial.print("\n");
}

void ReadSerialPort(){
  if (Serial.available() > 0){
    // look for the next valid integer in the incoming serial stream:
    int tmpCode = Serial.parseInt();
    parsedStatusCode = tmpCode;
  }
}

void ToggleBuiltinLed(){
  if(parsedStatusCode==1){ //Chance lamp if warning or alarm
    digitalWrite(WARNING_LAMP, HIGH);
    digitalWrite(ALARM_LAMP, LOW);
  }
  else if(parsedStatusCode==2){
    digitalWrite(WARNING_LAMP, LOW);
    digitalWrite(ALARM_LAMP, HIGH);
  } else {
    digitalWrite(WARNING_LAMP, LOW);
    digitalWrite(ALARM_LAMP, LOW);
    }
}

void loop() {
  ReadAnalogPins();
  SendValuesSerialPort(); 
  ReadSerialPort();
  ToggleBuiltinLed();
  delay(1000);
}
