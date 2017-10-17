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
int PinD2 = 2;

long parsedStatusCode = 1; 

void setup() {
  Serial.begin(9600);
  pinMode(PinD2, OUTPUT);
  digitalWrite(PinD2, HIGH);
  pinMode(LED_BUILTIN, OUTPUT);
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
  if(parsedStatusCode){
    digitalWrite(LED_BUILTIN, HIGH);
  }
  else{
    digitalWrite(LED_BUILTIN, LOW);
  }
}

void loop() {
  ReadAnalogPins();
  SendValuesSerialPort(); 
  ReadSerialPort();
  ToggleBuiltinLed();
  delay(1000);
}
