# 2018-Hestia-IOS
![(Logo here)][logo]  
A home automation system.

## Description
This repository contains the iOS client-side code of the __Hestia__ home automation system. The home automation system consists of various parts, below is a list with hyperlinks to the respective repositories containing them.
- iOS application
- [Android application](https://github.com/RUGSoftEng/2017-Hestia-Client)
- [Server](https://github.com/RUGSoftEng/2017-Hestia-Server)
- [Web server](https://github.com/RUGSoftEng/2018-Hestia-Web)

The Android and iOS app both try to stay within the look and feel of their respective host system. This way the usage of the app will feel native.
For additional information on how interaction between client and server takes place, how REST requests are handled and how the code works, refer to the respective parts of the system.

## Installation
Because the application is still in alpha/beta phase, the iOS application is not yet available in the App Store and is only deployable through a Mac for testing purposes. This is done using [free provisiong](https://docs.microsoft.com/en-us/xamarin/ios/get-started/installation/device-provisioning/free-provisioning). If the app is deployed in this way, it only works for one week. Furthermore, when using the system, the server must be run in a local network or must be run through the Web server which makes it accessible throughout the world. For more information on registering your local server on the Web server please refer to the [Web server repository](https://github.com/RUGSoftEng/2018-Hestia-Web).

## Usage
When opening the app for the first time, the user will be presented with an orange screen in which he can choose local or global. 
Choosing local allows the user to connect to a local server. Hence, an active locally hosted server must be available. After tapping global, the user is presented with the Auth0 login page of the Web server, where he can login using Google. In case of succesful global login, the user sees a list of local servers that are available on his Web server account. He can select the servers that he wants to use. Additionally, he can add or remove local servers.
Once the user enters a valid server or has tapped done in the server select screen, they will be presented with a list of devices that are on the local server(s) and can edit and change these as they see fit. The devices are created using plugins, for more on this please refer to previously stated repositories. Currently, it is only possible control devices using a switch (turning a devices on or off) and a slider, e.g. to change the brightness of a light.

The app also supports voice control. The commands that can be used in the devices main screen are as follows:
- To set boolean activator to true:  “activate” +  <Device name>  or “turn on” + <Device name>
- To set boolean activator to false: “deactivate” +  <Device name>  or “turn off” + <Device name>
- To go to add device screen: “add device” or “new device”
- To go to edit device screen: “edit” + <Device name>
- To go to remove a device : “remove” + <Device name> or “delete” + <Device name>

##Credits
The chevron icon (>) on the server select screen is made by [Freepik](http://www.freepik.com) from [www.flaticon.com](https://www.flaticon.com/) is licensed by [CC 3.0 BY](http://creativecommons.org/licenses/by/3.0/)

[logo]: /docs/images/logo_transparent.png "Hestia logo"