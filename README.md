A virtual reality experiment to investigate the influence of tool knowledge on anticipatory eye fixations featuring eye tracking and EEG support. 

The Experiment application is used for the Gaze tool interaction Project of the Insitute of Cognitive Science 

## Feature Highlights:

### Lab stream layer (LSL) data recording: 
The experiment uses LSL data recording, an external recording tool that do not pose additional pressure on the VR system for data recording and allows for adding additional data streams , like those of EEG.

### Realistic Interaction: 
The standard SteamVR Grasping system has been extended for the tools in question, and allow for a more realistic grasp positioning.

### Reworked Tools: 
Some tools of the previous iteration of the experiment have been replaced with custom made 3D models by Tino Sauerland(https://www.artstation.com/ragnar_hrodgarson) 

### Customizability and easy extension: 
The experiment  can be set to the custom needs of new experiments. Tools or block amount can be adjusted via the Unity editor. New Tools can be added by reusing the given structures for the already given tools. New experiment functions or process steps can be added by extending the NewExperiment logic.    




https://user-images.githubusercontent.com/43908801/199967693-0f019628-6a2b-43ca-9b44-5aa03b27b53d.mp4




## Prerequisites 

In order to run the experiment either from the [ViveSR runtime](https://developer.vive.com/resources/vive-sense/sdk/vive-eye-tracking-sdk-sranipal/). 

Lab stream layer need to be installed in order to record data from the experiment [Lab stream layer](https://labstreaminglayer.readthedocs.io/info/intro.html).

SteamVR needs to be installed from [Steam Store](https://store.steampowered.com/app/250820/SteamVR/).

The Unity Project requires Unity Version 2019.2.14.f1. For running the application this is not required.



## Running the build 

The build can be obtained by looking inside repositories from the build folder or downloading the "gti_index.zip" from the available [download files](https://github.com/VR-EEG/GTI-EEG/releases/tag/1.0)


Before starting the application, please make sure you did a Standing only calibration in Steam VR.

Upon start of the application, sranipal will require access, please allow this for proper functional eye tracking.


### starting LSL measurement

once the application is started, it is recommend to access the LSL streams from the application and "register" to them via the LSL system. If the application needs to be stopped, it is recommend to also finish the recording, and restart, to access again the the streams of the application.

### application use procedure


#### table calibration
Once in menu, please head over to the Table Calibration, which allows to move the participant. Move the participant adjusting the offset vector in the menu. This can be done by changing the values manually. 1 unit refers to 1 meter in reality. Change the values accordingly. The application will save the setting for the next startup of the application.

After table calibration the real experiment can start. head back to the main menu, and start the experiment.


The "Start Experiment" will automatically start a tutorial session, that repeats the spawning of a tool until the participant is ready. Only the experimenter can continue the experiment by pressing "continue".


### eye tracking calibration
After that, the experiment is set in the Inbetween block (or in the before-the-actual-experiment phase). Here The user can calibrate the eyetracking. The eyetracking will start the standard Sranipal eyetracking, and afterwards a custom validation. The results of the validation indicate the error offset in degree. everything below 1° is acceptable for proper eyetracking recording. The eyetracking also offers a baseline check for synchronizing later data streams with EEG that can be performed. It initialize a 3 second period with a time stamp begin and time stamp of it's end.



In addition to the LeapMotion SDK and the ViveSR runtime, proper configuration files need to be present inside the `Configuration` folder in order to run the experiment. Default configuration files are provided. 

### Starting a measurement 

After setting up the eye tracking, it is possible to run the actual experiment. randomized, all tools will be displayed in all orientations and with all tasks combinations. For each tool, the participant has to follow the experiment instructions according the experiment paragadime. Whenever the participant interacts with the button the former trial is finished and a new begins. In the standard setting the block consists of 48 trials. The procedure of the blocks is repeated for 6 blocks in total in it's standard setting. After all trials have been completed, the experimenter input is blocked completely. 


With Alt+F4 the experiment can be closed.



## Working with the Project

The project can be added inside the Unity Hub and from there directly launched. It requires unity Version 2019.2.14f1.

If only minor changes are required, please look for the ExperimentManager(or the script `NewExperimentManager`) in the Scene Objects, and see if required changes can already be done in the inspector.
The project should already allow to add or remove  tools, or tasks just from the inspector

If bigger changes are required, script manipulation utlizing C# is required. It is advisable to create additional scripts, that are then  merged in the execution order inside the `NewExperimentManager`.

Code that orignates from the project and is not part of  3rd Party assets or plugins(please notice the Third-Party asset notice below), can be used in other projects. An notice about the origin of the code is favorable but not required.

## Third-party asset credits 

#### 3D models
Some of the used 3D models were obtained from online ressources. The following are licenced under a [creative commons attribution 4.0](https://creativecommons.org/licenses/by/4.0/).

- [Adjustable Spannerwrench](https://sketchfab.com/3d-models/adjustable-spannerwrench-e13f98a9d7364510a65042d4c42e7a9c) by [CGWorker](https://sketchfab.com/CGWorker)
- [Emergency Stop Button](https://sketchfab.com/3d-models/emergency-stop-button-012e4809a41445ca9de17286f677fabb) by [Miljan Bojovic](https://sketchfab.com/phoenix-storms)
- [Gardening Trowel](https://sketchfab.com/3d-models/gardening-trowel-e6b0caf5e23547d88ebb458a5980e9b6) by [Matthew Meyers](https://sketchfab.com/darthobsidian)
- [Hammer](https://sketchfab.com/3d-models/hammer-2faa70b89da743d2924670ffe7d80163) by [FlukierJupiter](https://sketchfab.com/FlukierJupiter)
- [Old Work Bench](https://sketchfab.com/3d-models/old-work-bench-9fbc30ba31a546fe9370e6de2dcc0707) by [Oliver Triplett](https://sketchfab.com/OliverTriplett)
- [Screw Driver](https://sketchfab.com/3d-models/phillips-head-screw-driver-78c516b16ecc4b12bb2e6d90d031596e) by [KleenStudio](https://sketchfab.com/brandonh111121)
- [Tableware](https://sketchfab.com/3d-models/low-poly-tableware-7e3aeb6622ce4672968d8cabbb63cbd3) by [Anthony Yanez](https://sketchfab.com/paulyanez)

- [Spoke Wrench] created by Tino Sauerland (https://www.artstation.com/ragnar_hrodgarson)
- [Paintbrush] created by Tino Sauerland (https://www.artstation.com/ragnar_hrodgarson)
- ![Daisy Grubber](https://user-images.githubusercontent.com/43908801/199970114-45948417-2d59-415b-b25c-585c3979b803.png)
 created by Tino Sauerland (https://www.artstation.com/ragnar_hrodgarson)
- [Flower cutter] created by Tino Sauerland (https://www.artstation.com/ragnar_hrodgarson)

#### Texture 

- The texture [Planks Brown 10](https://texturehaven.com/tex/?c=wood&t=planks_brown_10) by Rob Tuytel is licensed under the [CC0](https://creativecommons.org/publicdomain/zero/1.0/) license.

#### Sound

- The sound [Beep in C](https://freesound.org/people/Hydranos/sounds/237706/) by [Hydranos](https://freesound.org/people/Hydranos/) is licensed under the [CC0](https://creativecommons.org/publicdomain/zero/1.0/) license.




### Vive Eye Tracking SDK notice
Distribution of the plugins in object code form is permitted via the SDK license agreement. Running the build or the code as provided here will collect facial feature data of the user for the purpose of eye tracking and the eye tracking data will be stored on the user's local machine.

### API Licenses

The [SteamVR Unity plugin](https://github.com/ValveSoftware/steamvr_unity_plugin) is licensed under the [BSD 3-Clause "New" or "Revised" License](https://github.com/ValveSoftware/steamvr_unity_plugin/blob/master/LICENSE).



