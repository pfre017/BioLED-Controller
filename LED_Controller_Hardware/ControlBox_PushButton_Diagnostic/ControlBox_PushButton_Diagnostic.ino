/*
  DigitalReadSerial
 Reads a digital input on pin 2, prints the result to the serial monitor 
 
 This example code is in the public domain.
 */

// digital pin 2 has a pushbutton attached to it. Give it a name:
const int max_button = 3;      //the pin of the last input (button) to read
const int max_LED = 13;
const int min_LED = 10;

// the setup routine runs once when you press reset:
void setup() {
  // initialize serial communication at 9600 bits per second:
  Serial.begin(9600);
  // make the pushbutton's pin an input:
    for(int button = 0; button <= max_button; button++){
      pinMode(button, INPUT);
      digitalWrite(button, HIGH);  //this sets the pullup resistor (i think).
    }
    for (int led = min_LED; led < max_LED; led++){
      pinMode(led, OUTPUT);
      digitalWrite(led, LOW);
    }  
}

// the loop routine runs over and over again forever:
void loop() {
  // read the input pin:
  
  for(int button = 0; button <= max_button; button++){
    int state = digitalRead(button);
    

      digitalWrite(13 - button, state);

    
    Serial.print("button ");
    Serial.print(button);
    Serial.print(" = ");
    Serial.print(state);
    Serial.print("\t");
  }
  
  Serial.println("");
  
  delay(10);        // delay in between reads for stability
}
