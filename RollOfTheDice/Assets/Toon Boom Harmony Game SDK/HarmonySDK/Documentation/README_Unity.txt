Harmony Game Engine Integration in Unity
========================================

The integration of the Harmony Game SDK in Unity requires the use of a native c++ plugin that will handle all the mesh calculation and management of the Harmony assets. 

Disclaimer
----------

The Harmony Game SDK makes use of the following libraries:

- zlib (http://www.zlib.net/zlib_license.html)

The Harmony Game SDK integration in Unity makes use of the following libraries:

- DotNetZip (http://dotnetzip.codeplex.com/license)
- mono i18N (http://www.mono-project.com/docs/faq/licensing/)

Compatibility
-------------

- Mac OSX 32/64 
- Windows 32/64 
- iOS 32/64 
- Android arm/x86 

Project Setup
-------------

All projects folders exported from Toon Boom Harmony must be copied in the StreamingAssets directory.  The reason for this is that Assets
managed by Unity won't be available on the file system once the game is published.

Scene Setup
-----------

To create a single animated character, a game object must be added to the scene with the Harmony game object menu entry, which is in other words a game object to which the HarmonyRenderer component has been attached to.

In order to ease setup process, a wizard script is added to the Unity interface.  The "Harmony Object" wizard will create a game object with some properties already filed in.


Harmony Renderer
----------------

The HarmonyRenderer component is the main component required for the game object.  Properties like ’Resolution’ and ‘Current Clips’ can be changed to see the different exported resolutions or animations.  The ‘Starting Skin’ property is not mandatory, but can be used to select a skin for each character section defined in Toon Boom Harmony. 
 
The color of the layer can be changed using property ‘Color'.  The color rgba values are directly multiplied with the rendered texture rgba values. Though, it is not possible to modify the alpha value of the Harmony game object with this property.

The property 'discretization step' can be used to refine triangulation of the character.  This will only be visible for characters with game bone deformation.


Playing the animation
-----------------

In the HarmonyRenderer component, there is a section called Animator Tool.  To make the animation available and possible to play in Unity, you will need to update the Animator by clicking on the “Update Animator” button. once the Animator has been updated, you will need to setup the transitions between the animations in the Animator window.  The ‘Current Clip’ property in the HarmonyRenderer component defines the current animation the system is supposed to play.  

By default, every animation is attached to the Any State.  If you want to do animation sequence, you need to remove the transition from the any State because the animator will try to reenter the state when the sequence will change to the next state.


Harmony Prop and Anchor
-----------------------

The HarmonyProp and HarmonyAnchor components are defined through metadata in Toon Boom Harmony and can be used to dynamically assign prop objects to anchor points in the character's hierarchy.  A prop object has its own displayable frame that is distinct from the character animation.  

The HarmonyAnchor component can also be used to extract the transformation of a named bone inside the hierarchy.  The transformation will reflect the current clip in use and the frame of rendering.


