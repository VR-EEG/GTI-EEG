A virtual reality experiment to investigate the influence of tool knowledge on anticipatory eye fixations featuring eye tracking and EEG support. 

The Experiment application is used for the Gaze tool interaction Project of the Insitute of Cognitive Science 

## Feature Highlights:

### LSL data recording: 
The experiment uses LSL data recording, an external recording tool that do not pose additional pressure on the VR system for data recording and allows for adding additional data streams , like those of EEG.

### Realistic Interaction: 
The standard SteamVR Grasping system has been extended for the tools in question, and allow for a more realistic grasp positioning.

### Reworked Tools: 
Some tools of the previous iteration of the experiment have been replaced with custom made 3D models by Tino Sauerland(https://www.artstation.com/ragnar_hrodgarson) 

### Customizability and easy extension: 
The experiment  can be set to the custom needs of new experiments. Tools or block amount can be adjusted via the Unity editor. New Tools can be added by reusing the given structures for the already given tools. New experiment functions or process steps can be added by extending the NewExperiment logic.    



## Prerequisites 

In order to run the experiment either from the [ViveSR runtime](https://developer.vive.com/resources/vive-sense/sdk/vive-eye-tracking-sdk-sranipal/). 

Lab stream layer need to be installed in order to record data from the experiment.

SteamVR needs to be installed from Steam Store.

The Unity Project requires Unity Version XXXX. For running the application this is not required.



## Running the build 

The build can be obtained by looking inside repositories from the build folder.


A build of the experiment is available in the `Build` folder. In addition to the LeapMotion SDK and the ViveSR runtime, proper configuration files need to be present inside the `Configuration` folder in order to run the experiment. Default configuration files are provided. 





### Running from source 
The source Unity project can be found in the `Source` folder. For maximum compatibility, the same software revisions should be used, that were also used during development.  

- The project is realized with Unity version 2019.2.14f1 
- Additional APIs  
  - Present in this repository
    - [SteamVR plugin](https://github.com/ValveSoftware/steamvr_unity_plugin), version 2.5
  - Need to be installed manually (the target locations are marked in the `Assets` folder)
    - TextMesh Pro, version 2.0.1 (can be installed from the Unity Package Manager)
    - [ViveSR plugin from the Vive Eye Tracking SDK (SRanipal)](https://developer.vive.com/resources/vive-sense/sdk/vive-eye-tracking-sdk-sranipal/), version 1.1.0.1 



### Starting a measurement 

In order to start a measurement, first, the room and the table the participant will sit at need to be calibrated in virtual space. The calibration can be started via the `Calibrate Floor & Table` option in the main menu of the experiment. The calibration files will be stored in the `Calibration` folder that is located in the root of the experiment's file hierarchy and can be reloaded at a later time. Second, the participant's information needs to be provided using the `Set Subject Data` option in the main menu. 
The measurement can then be started using `Start Experiment` from the main menu. 





### Third-party asset credits 

#### 3D models
Some of the used 3D models were obtained from online ressources. They are all licenced under a [creative commons attribution 4.0](https://creativecommons.org/licenses/by/4.0/).

- [Adjustable Spannerwrench](https://sketchfab.com/3d-models/adjustable-spannerwrench-e13f98a9d7364510a65042d4c42e7a9c) by [CGWorker](https://sketchfab.com/CGWorker)
- [Emergency Stop Button](https://sketchfab.com/3d-models/emergency-stop-button-012e4809a41445ca9de17286f677fabb) by [Miljan Bojovic](https://sketchfab.com/phoenix-storms)
- [Gardening Trowel](https://sketchfab.com/3d-models/gardening-trowel-e6b0caf5e23547d88ebb458a5980e9b6) by [Matthew Meyers](https://sketchfab.com/darthobsidian)
- [Hammer](https://sketchfab.com/3d-models/hammer-2faa70b89da743d2924670ffe7d80163) by [FlukierJupiter](https://sketchfab.com/FlukierJupiter)
- [Old Work Bench](https://sketchfab.com/3d-models/old-work-bench-9fbc30ba31a546fe9370e6de2dcc0707) by [Oliver Triplett](https://sketchfab.com/OliverTriplett)
- [Screw Driver](https://sketchfab.com/3d-models/phillips-head-screw-driver-78c516b16ecc4b12bb2e6d90d031596e) by [KleenStudio](https://sketchfab.com/brandonh111121)
- [Tableware](https://sketchfab.com/3d-models/low-poly-tableware-7e3aeb6622ce4672968d8cabbb63cbd3) by [Anthony Yanez](https://sketchfab.com/paulyanez)

- [Spoke Wrench] created by Tino Sauerland (https://www.artstation.com/ragnar_hrodgarson) provided under CCO agreement
- [Paintbrush] created by Tino Sauerland (https://www.artstation.com/ragnar_hrodgarson) provided under CCO agreement
- [Daisy Grubber] created by Tino Sauerland (https://www.artstation.com/ragnar_hrodgarson) provided under CCO agreement
- [Flower cutter] created by Tino Sauerland (https://www.artstation.com/ragnar_hrodgarson) provided under CCO agreement

#### Texture 

- The texture [Planks Brown 10](https://texturehaven.com/tex/?c=wood&t=planks_brown_10) by Rob Tuytel is licensed under the [CC0](https://creativecommons.org/publicdomain/zero/1.0/) license.

#### Sound

- The sound [Beep in C](https://freesound.org/people/Hydranos/sounds/237706/) by [Hydranos](https://freesound.org/people/Hydranos/) is licensed under the [CC0](https://creativecommons.org/publicdomain/zero/1.0/) license.




### Vive Eye Tracking SDK notice
Distribution of the plugins in object code form is permitted via the SDK license agreement. Running the build or the code as provided here will collect facial feature data of the user for the purpose of eye tracking and the eye tracking data will be stored on the user's local machine.

### API Licenses

The [SteamVR Unity plugin](https://github.com/ValveSoftware/steamvr_unity_plugin) is licensed under the [BSD 3-Clause "New" or "Revised" License](https://github.com/ValveSoftware/steamvr_unity_plugin/blob/master/LICENSE).



