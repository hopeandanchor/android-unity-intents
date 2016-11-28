# Launch Unity app using Android Intents

## Intro
Intents are a great way to pass information between apps on an android device. Recently I've been working on a project that required a unity app to be launched via an intent with a piece of data attached (an Intent 'extra'). This is a stripped down version of that project - I hope someone finds it useful!

## Setup
Download and install Android Studio and make sure you have the sdk installed - take a look in the SDK manager (you can launch this via android studio) and make sure you have the build and sdk tools.

## Steps for creating the Unity Project
Create a new Unity project
Create the android plugin directory (Assets/Plugins/Android)
Create a new xml file called AndroidManifest.xml in the Android folder (this is important- anywhere else and Unity won't pick it up). You can find an example xml file [here link](https://github.com/hopeandanchor/android-unity-intents/blob/master/UnityProject/Intents/Assets/Plugins/Android/AndroidManifest.xml)
Make sure to change the package name to match the bundle identifier in the project settings.
Now save the file and we'll create the plugin.

## Steps for creating the plugin from scratch (NB I'm using a Mac.)
Open Android Studio

### Create Android Studio project
File > New > New Project
Add Application name and domain
Click Next
Select the sdk you want to use and click Next
Select Add No Activity
Click Finish

### Create plugin module
File > New > New Module
Select Android Library
Click Next
Add the library name you wish to use (I used Plugin)
Click Finish

### Add dependencies
File > New > New Module
Import jar/aar package
  Filename: /Applications/Unity/PlaybackEngines/AndroidPlayer/Variations/mono/Release/Classes/classes.jar
  Name: unitydependencies (suggested)
Right click in the Project panel (found on the left)
Select Open Module Settings
Click on your plugin module
Click on the dependencies tab
Click the + 
Select Module dependency
Select unityclasses

### Clean up
Right click in the Project panel (found on the left)
Select Open Module Settings
Select 'app'
Click - (at the top left) and remove module

### Create an Activity class
A Unity android app uses UnityPlayerActivity as the main activity class when launching. To use a custom class, we need to extend UnityPlayerActivity and grab the Intent extra that we're passing in. You can find an example of this [here link](https://github.com/hopeandanchor/android-unity-intents/blob/master/AndroidPlugin/UnityPlugins/plugin/src/main/java/digital/haa/plugin/MainActivity.java).
We'll also need to grab the intent and save the data so it can be accessed by Unity. I've overridden the onCreate function and added the following:
```
Intent intent = getIntent();
if(intent != null && intent.hasExtra(sessionIdIntent)) {
  sessionId = getIntent().getStringExtra(sessionIdIntent);
}
```
Then I added a static function to be accessed by Unity:

```
public static String GetSessionId()
{
  return sessionId;
}
```

Remember the name and package of the class you created - you'll need it later.

### Create jar file
In the project in this repository, I've added a bash script called createjar.sh
Open a terminal window and run that script (sh createjar.sh). You might have to amend it to run on your setup
This script runs the 'clean' and 'build' gradle tasks before creating a jar from the classes created. It then copies that jar to the correct place in the Unity project.
Go back to your Unity project. You should have a jar file called unityintents.jar in the Assets/Plugins/Android folder. 

### Tell Unity to use the new class
Open up AndroidManifest.xml again. Typically, there is one main activity listed in this xml file and it tends to look like this:

```
<activity android:name="com.unity3d.player.UnityPlayerActivity" android:label="@string/app_name">
    <intent-filter>
        <action android:name="android.intent.action.MAIN" />
        <category android:name="android.intent.category.LAUNCHER" />
    </intent-filter>
    <meta-data android:name="unityplayer.UnityActivity" android:value="true" />
</activity>
```

This is telling android to call the com.unity3d.player.UnityPlayerActivity class when it receives an intent for the package id you've specified. We need to change this to point to the new class contained in the plugin - replace com.unity3d.player.UnityPlayerActivity with the relevant package and class name (you can see the one I've used below).

```
<activity android:name="digital.haa.plugin.MainActivity" android:label="@string/app_name">
    <intent-filter>
        <action android:name="android.intent.action.MAIN" />
        <category android:name="android.intent.category.LAUNCHER" />
    </intent-filter>
    <meta-data android:name="unityplayer.UnityActivity" android:value="true" />
</activity>
```


### Grab the intent extra from Unity

Now we just need to grab the intent extra we've saved. We do this by accessing the java class (using the fully qualified class name) and calling the GetSessionId function.

```
AndroidJavaClass pluginClass = new AndroidJavaClass("digital.haa.plugin.MainActivity");
Debug.Log("*" + pluginClass.CallStatic<string>("GetSessionId") + "*");
```

Now build and run the project.

When it launches on the project, that debug line will print "**". This is because the unity app was not launched using the intent, so the expected data will be empty.

NB. You can see the output using "adb logcat" from the terminal (I added "~/Library/Android/sdk/platform-tools/" to my path in my .bash_profile for ease).

You can now test the available intents for your app using this command (make sure your device is still connected)
```
adb shell pm dump [your.unity.packageid.here] | grep -A 1 MAIN
```

This should output something like :
```
        android.intent.action.MAIN:
          699b36b digital.haa.unityintent/digital.haa.plugin.MainActivity
```

Which should equate to [unity bundle id]/[your unityplayeractivity override]
If you're still getting com.unity3d.player.UnityPlayerActivity as the activity listed, make sure you have your manifest file in the correct folder (has to be Assets/Plugins/Android - Assets/Plugins isn't good enough)

You can now launch your app using the adb shell command

```
adb shell am start -n "digital.haa.unityintent/digital.haa.plugin.MainActivity" -e sessionId '"get this session id!"'
```

If you see the following warning, make sure you close the app on your device and run the command again
Warning: Activity not started, its current task has been brought to the front

You should now see "get this session id!" in adb logcat (or displayed in a textfield if you're using the demo project).



