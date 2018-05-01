# 2018-Hestia-IOS
![(Logo here)][logo]
A home automation system.

## General
This repository exists out of the iOS clientside code of the __Hestia__ home automation system. The home automation system consists out of various parts, below is a list with hyperlinks to the respective repositories containing them.
- iOS application
- [Android application](https://github.com/RUGSoftEng/2017-Hestia-Client)
- [Server](https://github.com/RUGSoftEng/2017-Hestia-Server)
- [Web server](https://github.com/RUGSoftEng/2018-Hestia-Web)

The android and iOS app both try to stay withing the look and feel of their respective host' system. This way there usage will be intuitive and stylish.
For additional information on how interaction and rest requests are handled and how the code works refer to the respective parts of the system.

## Deployment
Because the application is still in alpha/beta fase the iOS application is not yet available in the app store and is only deployable through a mac with a developer account. Furthermore when using the system the server must be run in a local network or must be run through the web server which makes it accessible throughout the world. For more on registering your local server on the website please refer to the web server' repository.

## Usage
When opening the app the user will be presented with a first time login screen which allows the user to set a pin code/login information. When using the application an active server must be available either through the web server or a localy hosted server. Once the user enters a valid server they will be presented with a list of devices that are on the server and can edit and change these as they see fit. The devices are created using plugins, for more on this please refer to previosly stated repositories. Currently it is only possible to turn devices on and off or use a slider. e.g. to change the brightness of a light.

[logo]: /docs/images/logo_transparent.png "Hestia logo"