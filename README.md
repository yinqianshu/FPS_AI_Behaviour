# FPS_AI_Behaviour
 FPS AI Behaviour Unity C# Script
 
 This is my fist attemp to replicate CS:GO like AI behaviours. This AI behaviour script is suitable for free-for-all FPS game mode.
 
 FOR TESTING: Attach this script to an AI game object or a simple cube, sphere, cylinder,... representing AI, it will get a random target to follow and shoot that target if that target is in its shooting range.
 
 IMPORTANT:
 - Require Nav Mesh Agent component for path finding
 - Prefer a gun tip transform for raycast shooting (not require, can also use the game object transform)
 - Replace damage system with Debug.Log() for testing purpose or create a script with simple damage function (currentHealth -= damage;)
 
 Possible Update in Future (you can help!):
 - Rotation based on enviroment, not on target
 - Check if there are another targets appear while finding the initial target -> get random new target to chase after and shoot
 
 Encountered Problem:
 - IF AIs get too close to each other, they will become a merry-go-round
