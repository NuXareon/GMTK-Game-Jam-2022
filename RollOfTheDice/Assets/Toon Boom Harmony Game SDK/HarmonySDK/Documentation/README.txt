Harmony Game Engine Export
==========================

The Harmony Game Engine feature proposes a data structure to render sprite sheet based animated characters in a wide range of devices.  The drawing and the animation is done in Harmony and exported to an xml data structure that can be then used in multiple game engines through our plugins.  

For more information about:
- The Harmony Game SDK
- How to create your assets
- How to export them 

you can go to the following url : https://docs.toonboom.com/go/help/harmony-gaming


Basic Guideline for Harmony
---------------------------

While using Harmony, the user must avoid using rendering effects in his animation or complex composition in the network. The easiest way to create a valid composition is by creating the character rig with the Timeline view.

During export, the order in which layers will be sorted is dependant on the order of layers in the timeline and their z offset. So, depending on its network composition, the user might have to reorder its layers on the z axis to get the proper result.

As for effects, they cannot be used currently, except for the cutter.  The sprite sheets used in the game engine are rendered using the initial drawing saved on disk (i.e. tvg file).  The effects cannot be currently generated dynamically through the game engine itself.  If the user needs an effect to be visible, then this effect will have to be backed in the drawing.

To generate several animation sequences of a same character, it is suggested to make use of the versioning capability of Harmony.  As long as a character maintains the same hierarchy (i.e. the same timeline composition in Harmony), it will reuse the same skeleton in the game engine and share its drawings in the sprite sheet.


Data Structure
--------------

The exported data structure contains the following files and folders:

  XmlExport/
    +- stage.xml
    +- animation.xml
    +- skeleton.xml
    +- drawingAnimation.xml
    +- spriteSheets.xml
    +- spriteSheets/
       +- (...)
    +- stages/
       +- (...)

the stage.xml file is the main entry point for all animation.  It maps all versions of the user Harmony scenes (e.g. walk cycle, run cycle...) to a stage clip with their respective skeleton (skeleton.xml), animation (animations.xml) and drawing animation (drawingAnimations.xml).

A second more optimized level of the data structure is also available.  The binary format is the actual data structure used during rendering and can be streamed in and out with ease.  For a stage clip containing one or more animation, a single binary file is generated.  Only the binary format can be kept at export if needed.  If so, the files skeleton.xml, animation.xml and drawingAnimation.xml can be removed.

This new version of the HarmonySDK remove the rendering from the native code and use the unity mesh instead.  You will need to export from Harmony 15.0.5 or higher version.


Possible Errors
--------------

Old files will not work right away.  The spritesheets.xml need to know the width and height of their texture.  Just add width="" and height="" with the right values the spritesheet node in the spritesheets.xml file.