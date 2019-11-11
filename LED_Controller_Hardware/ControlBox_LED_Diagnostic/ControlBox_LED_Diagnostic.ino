
// Pin 13 has an LED connected on most Arduino boards.
// give it a name:
const int max_LED = 13;
const int min_LED = 10;

// the setup routine runs once when you press reset:
void setup() {                
  // initialize the digital pin as an output.
  for (int led = min_LED; led < max_LED; led++){
    pinMode(led, OUTPUT);
    digitalWrite(led, LOW);
  }   
}

// the loop routine runs over and over again forever:
void loop() {
  
  for (int led = min_LED; led <= max_LED; led++){
    digitalWrite(led, HIGH);
    delay(50);
    digitalWrite(led, LOW);
    delay(50);
  }
}

