# UnityObjectActionSystem

Object Action System is a ScriptableObject based action system, allowing you to drag and drop actions, or a collection of actions to be performed in response to an event.

The ObjectActionSystem (OAS) is made up of two parts, the __EventMachine__ and a __ActionController__.  

__EventMachine__

This handles the calling of events, there are 3 types of EventMachines so far, with most using unity's built-in tag system for filtering events. 


![EventMachine_Screenshot](https://i.imgur.com/PYKybPT.png)




1. __EventMachine__

This is the most basic EventMachine it handles Trigger events and Collider events, it also has a __allowedTags__ array field allowing you to specify which Unity tags can trigger events.
Events For EventMachine
Triggers : OnEnterTrigger, OnStayTrigger, OnExitTrigger
Colliders : OnEnterCollision, OnStayCollision, OnExitCollision


2. __Mouse_EventMachine__

An EventMachine for handling GameObject Mouse events, like the __EventMachine__ it has a __allowedTags__ field.

Events For Mouse_EventMachine
1. OnMouseAsButton 
2. OnDragMouse 
3. OnEnterMouse
4. OnExitMouse
5. OnUpMouse
6. OnDownMouse
7. OnOverMouse

3. __Element_EventMachine__

This EventMachine requires the GameObject to also have the Affector Component, it fires an event when another trigger collider enters it own trigger collider, who's Affector it has a reaction for.

Events For Element_EventMachine
OnAffector : When another Trigger collider enters the EventMachine's GameObject, with an Affector element for which there is a defined reaction. 


__ActionController__

The Controllers come in two different types, the ActionController and ElementActionController, these handle the execution and storing of per-object data for the actions.

1. __ActionController

The ActionController is the default controller, it's provides two functions to call from the EventMachine events

*StartNamedReaction(string name)* - Finds a reaction by it's name then executes it.
*StartReaction* - Executes the first reaction in it's list, regardless of name.

