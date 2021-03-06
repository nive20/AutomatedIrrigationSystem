# Centralized Automated Irrigation System

## Contents

- [Centralized Automated Irrigation System](# Centralized Automated Irrigation System)
  - [Contents](#contents)
  - [Short description](#short-description)
    - [What's the problem?](#whats-the-problem)
    - [How can technology help?](#how-can-technology-help)
    - [The idea](#the-idea)
  - [Demo video](#demo-video)
  - [The architecture](#the-architecture)
  - [Project roadmap](#project-roadmap)
  - [Getting started](#getting-started)
  - [Built with](#built-with)
  - [Authors](#authors)

## Short description

### What's the problem?

If our advancement towards technology is progress, then underutilizing the same in sectors like Irrigation is regress.Water overflow OR Water scarcity due to poor manageability of water supply 
Poor forecasting techniques of weather and soil dampness.Lack of water source capacity management  and automation.Less adaption of technology towards irrigation


### How can technology help?

We can leverage the Cloud and IoT concepts in building the mentioend solution

### The idea

Water resource management using IoT and Cloud infrastructure which can minimize the wastage and distribute effectively and efficiently using forecast and sensor technologies 
This solution caters to Public and Private Irrigation sectors.High availability and seamless integration with water resources.Alert and Notification system which helps in sustainable distribution & management
Disaster Management and Farm Friendly 

## Demo video

[![Watch the video](https://github.com/nive20/AutomatedIrrigationSystem/blob/master/images/DemoCover.PNG)](https://youtu.be/mTmDkKi7YxU)

## The architecture

![Video transcription/translation app](https://github.com/nive20/AutomatedIrrigationSystem/blob/master/images/CAIS.PNG)

1. Soil Moisture Data is collected from Soil Moisture Sensor.
2. Weather Data about rain is colleted from IBM cloud Weather API.
3. Based on evalaution of both the value we decide if automatic irrigation is required or not.


## Project roadmap

The project currently does the following things.

Weather and Soil dampness calculations
Tap switching and channel identification

Future Roadmap

Capacity calculation in Units 
Reporting using AI / ML techniques
End user communication capability
Analytics at Control Center level
Automated outflow incase of water scarcity with proper channeling 


## Getting started

You need to do arduino setup with soil Moisture sensor , LCD and LED and execute the WaterAllocationConsole.exe from this to see the centralized control center

- [AutomatedIrrigationController](https://github.com/nive20/AutomatedIrrigationSystem/tree/master/AutomatedIrrigationController)

## Built with

- [IBM Cloud Event-Stream](https://cloud.ibm.com/catalog/services/event-streams) - Platform for getting weather data under event stream. 
- [IBM -Cloud-Hosted-Service](https://weatherapidata.eu-gb.mybluemix.net/api/values)-We have created our own WEB API service and hosted in IBM cloud.
- [IBM API Connect](https://api.weather.com/v3/wx/forecast/daily/7day?geocode=48.1545703,11.2616557&format=json&units=m&language=en-US&apiKey=xxxxx) - The Weather company API to get the required data.
- [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/)
- [Arduino UNO](https://visualstudio.microsoft.com/downloads/)

## Authors

- **Nivedita Parihar** - (https://github.com/nive20)
- **Sreenivas Rao Manda**- (https://github.com/sreenigithub23)
- **Chandra Mondal** -(https://github.com/Chandramondal)
- **Priyanka Chandrabose**  -(https://github.com/PriyankaChandrabose)

