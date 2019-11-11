/*
  Analog input, analog output, serial output
 
 Reads an analog input pin, maps the result to a range from 0 to 255
 and uses the result to set the pulsewidth modulation (PWM) of an output pin.
 Also prints the results to the serial monitor.
 
 The circuit:
 * potentiometer connected to analog pin 0.
   Center pin of the potentiometer goes to the analog pin.
   side pins of the potentiometer go to +5V and ground
 * LED connected from digital pin 9 to ground
 
 created 29 Dec. 2008
 modified 9 Apr 2012
 by Tom Igoe
 
 This example code is in the public domain.
 
 */

// These constants won't change.  They're used to give names
// to the pins used:
const int analogInPin0 = A3;  // Analog input pin that the potentiometer is attached to
const int analogInPin1 = A2;
const int analogInPin2 = A1;
const int analogInPin3 = A0;

const char deviceID = 'A';
const char channelID0 = '0';
const char channelID1 = '1';
const char channelID2 = '2';
const char channelID3 = '3';


//push button inputs (DigitalInputs)
int buttonState0;
int lastButtonState0 = LOW;
int buttonState1;
int lastButtonState1 = LOW;
int buttonState2;
int lastButtonState2 = LOW;
int buttonState3;
int lastButtonState3 = LOW;

// the following variables are long's because the time, measured in miliseconds,
// will quickly become a bigger number than can be stored in an int.
long lastDebounceTime0 = 0;  // the last time the output pin was toggled
long lastDebounceTime1 = 0;  // the last time the output pin was toggled
long lastDebounceTime2 = 0;  // the last time the output pin was toggled
long lastDebounceTime3 = 0;  // the last time the output pin was toggled
long debounceDelay = 50;    // the debounce time; increase if the output flickers

String inputString = "";
boolean stringComplete = false;


//smoothing
const int averagingWindow = 50;
int index0 = 0;
long total0 = 0;
int average0 = 0;
int readings0[averagingWindow];
int index1 = 0;
long total1 = 0;
int average1 = 0;
int readings1[averagingWindow];
int index2 = 0;
long total2 = 0;
int average2 = 0;
int readings2[averagingWindow];
int index3 = 0;
long total3 = 0;
int average3 = 0;
int readings3[averagingWindow];
//caching
int previousValue0 = -1;
int previousValue1 = -1;
int previousValue2 = -1;
int previousValue3 = -1;

//int sensorValue0 = 0;        // value read from the pot
int ledValue0 = 0;
int ledValue1 = 0;
int ledValue2 = 0;
int ledValue3 = 0;
//int outputValue = 0;        // value output to the PWM (analog out)

void setup() {
  // initialize serial communications at 9600 bps:
  Serial.begin(9600); 
  
  inputString.reserve(200);
  
  for (int thisReading = 0; thisReading < averagingWindow; thisReading++)
    readings0[thisReading] = 0; 
     for (int thisReading = 0; thisReading < averagingWindow; thisReading++)
    readings1[thisReading] = 0; 
     for (int thisReading = 0; thisReading < averagingWindow; thisReading++)
    readings2[thisReading] = 0; 
     for (int thisReading = 0; thisReading < averagingWindow; thisReading++)
    readings3[thisReading] = 0; 
    
    pinMode(0, INPUT);  //LED0 shutter
    pinMode(1, INPUT);  //LED1 shutter (590 nm)
    pinMode(2, INPUT);  //LED2 shutter (470 nm)
    pinMode(3, INPUT);    //LED3 shutter (not installed on BioLED device)
    digitalWrite(0, HIGH);
    digitalWrite(1, HIGH);
    digitalWrite(2, HIGH);
    digitalWrite(3, HIGH);
    
    pinMode(10, OUTPUT);  //LED0 shutter indicator
    pinMode(11, OUTPUT);    //LED1 shutter indicator
    pinMode(12, OUTPUT);  //LED2 shutter indicator
    pinMode(13, OUTPUT);    //LED3 shutter indicator
    

    
    digitalWrite(10, LOW);
    digitalWrite(11, LOW);
    digitalWrite(12, LOW);
    digitalWrite(13, LOW);
}

void loop() {

//digitalWrite(13, HIGH);
//delay(100);
//digitalWrite(13, LOW);
//delay(100);


//read the serial port for messages coming in

if (stringComplete)
{
  int ledpin = 13;    //this needs to be gotten from the inputString "A0" = pin 13, "A1" = pin 12
  int ledState = HIGH;
  digitalWrite(ledpin, ledState);
  
  inputString = "";
  stringComplete = false;
}


  // read the analog in value:
  
  total0 = total0 - readings0[index0];
  
  readings0[index0] = analogRead(analogInPin0); 
  total0 = total0 + readings0[index0];  

  index0 = index0 + 1;
  if (index0 >= averagingWindow)
    index0 = 0;
    
   average0 = total0 / averagingWindow;

  ledValue0 = map(average0, 0, 1023, 0 , 255);
           
  if (previousValue0 != average0)
  {
    PrintOutput(channelID0, average0);
    previousValue0 = average0;  
  }
  
  total1 = total1 - readings1[index1];
  
  readings1[index1] = analogRead(analogInPin1); 
  total1 = total1 + readings1[index1];  

  index1 = index1 + 1;
  if (index1 >= averagingWindow)
    index1 = 0;
    
   average1 = total1 / averagingWindow;

  ledValue1 = map(average1, 0, 1023, 0 , 255);
           
  if (previousValue1 != average1)
  {
    PrintOutput(channelID1, average1);
    previousValue1 = average1;  
  }
  
  total2 = total2 - readings2[index2];
  
  readings2[index2] = analogRead(analogInPin2); 
  total2 = total2 + readings2[index2];  

  index2 = index2 + 1;
  if (index2 >= averagingWindow)
    index2 = 0;
    
   average2 = total2 / averagingWindow;

  ledValue2 = map(average2, 0, 1023, 0 , 255);
           
  if (previousValue2 != average2)
  {
    PrintOutput(channelID2, average2);
    previousValue2 = average2;  
  }
  
  total3 = total3 - readings3[index3];
  
  readings3[index3] = analogRead(analogInPin3); 
  total3 = total3 + readings3[index3];  

  index3 = index3 + 1;
  if (index3 >= averagingWindow)
    index3 = 0;
    
   average3 = total3 / averagingWindow;

  ledValue3 = map(average3, 0, 1023, 0 , 255);
           
  if (previousValue3 != average3)
  {
    PrintOutput(channelID3, average3);
    previousValue3 = average3;  
  }
  
  
  //digial inputs  (see http://arduino.cc/en/Tutorial/Debounce )
  
  int reading0 = digitalRead(0);  //590 nm
  int reading1 = digitalRead(1);  //470 nm
  int reading2 = digitalRead(2);  //400 nm
  int reading3 = digitalRead(3);  //not installed
  
//  PrintButtonState(channelID0, reading0);
//  PrintButtonState(channelID1, reading1);
//  PrintButtonState(channelID2, reading2);
//  PrintButtonState(channelID3, reading3);
  
 
  if (reading0 != lastButtonState0) {
    // reset the debouncing timer
    lastDebounceTime0 = millis();
  } 
  if ((millis() - lastDebounceTime0) > debounceDelay) {
    if (reading0 != buttonState0) {
      buttonState0 = reading0;
      PrintButtonState(channelID0, buttonState0);   
    }
  }
  lastButtonState0 = reading0;
  
  if (reading1 != lastButtonState1) {
    // reset the debouncing timer
    lastDebounceTime1 = millis();
  }
  if ((millis() - lastDebounceTime1) > debounceDelay) {
    if (reading1 != buttonState1) {
      buttonState1 = reading1;
      PrintButtonState(channelID1, buttonState1);   
    }
  }
  lastButtonState1 = reading1;
  if (reading2 != lastButtonState2) {
    // reset the debouncing timer
    lastDebounceTime2 = millis();
  }
  if ((millis() - lastDebounceTime2) > debounceDelay) {
    if (reading2 != buttonState2) {
      buttonState2 = reading2;
      PrintButtonState(channelID2, buttonState2);   
    }
  }
  lastButtonState2 = reading2;
  if (reading3 != lastButtonState3) {
    // reset the debouncing timer
    lastDebounceTime3 = millis();
  }
  if ((millis() - lastDebounceTime3) > debounceDelay) {
    if (reading3 != buttonState3) {
      buttonState3 = reading3;
      PrintButtonState(channelID3, buttonState3);   
    }
  }
  lastButtonState3 = reading3;

  // wait 2 milliseconds before the next loop
  // for the analog-to-digital converter to settle
  // after the last reading:
  delay(5);                     
}

void serialEvent() {
  while (Serial.available()) {
    // get the new byte:
    char inChar = (char)Serial.read(); 
    // add it to the inputString:
    inputString += inChar;
    
    Serial.print(inputString);
    // if the incoming character is a newline, set a flag
    // so the main loop can do something about it:
    if (inChar == '\n') {
      stringComplete = true;
      digitalWrite(10, HIGH);
    } 
  }
}

void PrintButtonState(char channelID, int Value)
{
  if (Value == LOW){
    return;
  }
  Serial.print(deviceID);
  Serial.print(channelID); 
  Serial.print("\tB");
  Serial.println(Value); 
  

      //digitalWrite(channelID, Value);
}

void PrintOutput(char channelID, int Value)
{
     
  // print the results to the serial monitor:
  Serial.print(deviceID);
  Serial.print(channelID); 
  Serial.print("\t");
  Serial.println(Value);      

}
