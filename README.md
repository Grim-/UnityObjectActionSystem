# UnityObjectActionSystem

Object Action System is a ScriptableObject based action system, allowing you to drag and drop actions, or a collection of actions to be performed in response to an event.


## What it's For 

OAS is designed to be a simple drag, drop and tweak collection of Components, Editors, Scripts and ObjectActions allowing you to quickly add simple interactive elements and objects to your game and/or project. 

From simple examples such as, playing a particle effect on left clicking a GameObject, or scaling the object up.

<img src="https://i.imgur.com/rmVfad4.gif">

Or Starting a fire 
[Fire Example on Imgur](https://imgur.com/WIaHxtK)


Lets take a quick look at the Element_ActionController for the burning barrels example..

This is how a typical reaction might look for the Element_ActionController, it takes the element that triggers it, and then a list of actions to execute.

<img src="https://i.imgur.com/AmSAmtw.png">

If you switch an action to the data tab, you can see what data it will use to perform it's actions.

<img src="https://i.imgur.com/mY4qIXi.png">

1. SpawnPrefab - This spawns the fire particle system with a 30 second timer component on the prefab, after which the fire will destroy itself.
2. WaitForRandomRange - Wait For a time randomly picked between WaitMin and WaitMax
3. SetAffector - Sets this object's Affector component element to the chosen Element scriptable object, allowing the Object to now trigger *other* objects Fire reactions.
4. WaitFor - Waits For 5 seconds
5. LerpShaderGraphFloatValue - This action Lerps the PropertyName value to the chosen Value over LerpTime. For this example I created a simple dissolve shader and exposed the "DissolveValue" property in order to make the barrels fade out.
6. DestroyGameObject - You can set either SELF or TARGET as the target of this action, most actions default to SELF.

## How it Works

The ObjectActionSystem (OAS) is made up of two main parts, the __EventMachine__ and a __ActionController__, there is also a custom Editor for the ActionControllers to make designing new reactions more pain free than through the standard unity UI.


__EventMachine__

This handles the calling of events, there are 3 types of EventMachines so far, with most using unity's built-in tag system for filtering events. 





__EventMachine__

This is the most basic EventMachine it handles Trigger events and Collider events, it also has a __allowedTags__ array field allowing you to specify which Unity tags can trigger events.

Events For __EventMachine__

*Triggers :*

1. OnEnterTrigger
2. OnStayTrigger
3. OnExitTrigger

*Colliders :*
1. OnEnterCollision
2. OnStayCollision
3. OnExitCollision


 __Mouse_EventMachine__

An EventMachine for handling GameObject Mouse events, like the __EventMachine__ it has a __allowedTags__ field.

Events For __Mouse_EventMachine__

1. OnMouseAsButton 
2. OnDragMouse 
3. OnEnterMouse
4. OnExitMouse
5. OnUpMouse
6. OnDownMouse
7. OnOverMouse

__Element_EventMachine__

This EventMachine requires the GameObject to also have the Affector Component, it fires an event when another trigger collider enters it own trigger collider, who's Affector it has a reaction for.

Events For __Element_EventMachine__

1. OnAffector : When another Trigger collider enters the EventMachine's GameObject, with an Affector element for which there is a defined reaction. 


__ActionController__

The Controllers come in two different types, the ActionController and ElementActionController, these handle the execution and storing of per-object data for the actions.

<img src="https://i.imgur.com/z2fNjCf.png">

1. __ActionController

The ActionController is the default controller, it's provides two functions to call from the EventMachine events

 1. *StartNamedReaction(string name)* - Finds a reaction by it's name then executes it.

 2. *StartReaction* - Executes the first reaction in it's list, regardless of name.

