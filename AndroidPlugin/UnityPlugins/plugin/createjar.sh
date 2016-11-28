#!/bin/bash

gradle clean
gradle build
cd ./build/intermediates/classes/release/
jar cvf ../../../../../../../UnityProject/Intents/Assets/Plugins/Android/unityintents.jar digital/haa/plugin/*.class
