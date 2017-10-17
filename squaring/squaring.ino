void setup() {
  Serial.begin(9600);
  Serial.println("Start Input");
}

void loop() {
  int x1, x0, result;
  if (Serial.available() > 0){
    x1 = Serial.read()-48; 
    x0 = Serial.read()-48;
    Serial.print("Number received: ");
    Serial.print(x1);
    Serial.println(x0);
    
    result = x1*10+x0;
    Serial.print("Result: ");
    Serial.println(result * result);
    
    // Flush the other bytes of the input
    do {
      x0 = Serial.read();
    } while (x0 >0);
  }
  delay(1000);
}
