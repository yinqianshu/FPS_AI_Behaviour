# FPS_AI_Behaviour
 FPS AI Behaviour Unity C# Script
 
 This is my fist attemp to replicate CS:GO like AI behaviours. This AI behaviour script is currently suitable for free-for-all FPS game mode. AIs will currently shoot whatever tagged with "Target". AIs can headshot and body shot.
 
 FOR TESTING: Attach this script to an AI game object or a simple cube, sphere, cylinder,... representing AI, it will get a random target to follow and shoot that target if that target is in its shooting range.
 
 IMPORTANT:
 - Require Nav Mesh Agent component for path finding
 - Prefer a gun tip transform for raycast shooting (by no means required, you can also use the original game object transform)
 - Replace damage system with Debug.Log() for testing purpose or create a script with simple damage function (ex: currentHealth -= damage;)
 - Prefer 2 separate colliders for the head and the body of AI for calculating headshot and body shot.
 - Remove Animator reference in the script if you don't have animation for AI
 
 Possible Update in Future (you can help!):
 - AI will be able to move by path following and steering
 - Check if there are another targets appear while finding the initial target -> get random new target to chase after and shoot (this behaviour in the script has been commented out since Debug.Log() didn't return anything and it keeps getting error)
 
 Encountered Problem:
 - IF AIs get too close to each other, they will become a merry-go-round
 - Getting null errors when trying to get new target to chase after
 
 MIT License
