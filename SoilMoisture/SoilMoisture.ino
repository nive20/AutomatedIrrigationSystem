#include "DHT.h"// including the library of DHT11 temperature and humidity sensor
 #include<Wire.h>
 #include<LiquidCrystal_I2C.h>
 
 LiquidCrystal_I2C lcd=LiquidCrystal_I2C(0x27,16,2);
 boolean isConnected = false;
 String inputString = "";         // a string to hold incoming data
boolean stringComplete = false;  // whether the string is complete
String commandString = "";
int sensorPin = A0; 
int sensorValue; 

 int WETPIN=4;
 int DRYPIN=3;
 
 void setup ( ) {
   Serial.begin ( 9600 ) ;
   lcd.init();
   lcd.backlight();
   initDisplay();
   pinMode(DRYPIN,OUTPUT);
   pinMode(WETPIN,OUTPUT);
 }

  void initDisplay()
{
  lcd.begin(16, 2);
  lcd.print("Ready to connect");
}


 void loop ( ) {
  
    if(stringComplete)
    {
      stringComplete = false;
      getCommand();
      
      if(commandString.equals("START"))
      {
        lcd.clear();
      }
      if(commandString.equals("STOP"))
      {
        lcd.clear();
        lcd.print("Ready to connect");    
      }
      else if(commandString.equals("SOIL"))
      {
         lcd.setCursor(0,0);
        String moistureStae = getMoistureInfo();
       
      }
      inputString = "";
    }

 }


String getMoistureInfo()
{
    delay(2000);
    lcd.clear();
    lcd.setCursor(0,0);
    sensorValue = analogRead(sensorPin); 
    sensorValue=sensorValue/10;
    lcd.print("MOISTURE: ");    
if(sensorValue>50)
{
digitalWrite(WETPIN,HIGH);
digitalWrite(DRYPIN,LOW);
}
else
{
digitalWrite(WETPIN,LOW);
digitalWrite(DRYPIN,HIGH);
}
    
     Serial.println (sensorValue) ;
      lcd.setCursor(0,1);
     lcd.print(sensorValue);
     delay(2000);
}

void getCommand()
{
  if(inputString.length()>0)
  {
     commandString = inputString.substring(1,5);
  }
}

void serialEvent() {
  while (Serial.available()) {
    // get the new byte:
    char inChar = (char)Serial.read();
    // add it to the inputString:
    inputString += inChar;
    // if the incoming character is a newline, set a flag
    // so the main loop can do something about it:
    if (inChar == '\n') {
      stringComplete = true;
    }
  }
}
