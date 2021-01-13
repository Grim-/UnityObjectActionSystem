# UnityObjectActionSystem

Object Action System is a ScriptableObject based action system, allowing you to drag and drop actions, or a collection of actions to be performed in response to an event.
It is still work in progress, most of the basic features are finished but there is some polish and extra features I want to include.

# Definitions 
# Action
An Action Preformed on either SELF or TARGET.
# Reaction
A Named Collection of Actions to be called by the static event StartNamedReaction.
# ControllerComponent
Holds the reactions (a collection of actions) and the data needed by the actions to behave as they should. The controllers expose two methods StartReaction() and StartNamedReaction(string reactionName) which can be called by the event component on certain events.
# EventComponent
Triggers specific events, can filter targets based on tag.

There are different kinds of ActionController Components and EventComponents depending on the behaviour you are trying to achieve.

# ActionController
This is the base controller, this handles basic list of reactions. You can add actions to be preformed by the object with the controller.
![Example Set Up](https://i.imgur.com/NkMYTh2.png)


# EventMachine
This is the base EventMachine, it handles events related to Triggers and Colliders, it also contains a taglist, so you can filter targets based on their tag.

# Mouse_EventMachine
This EventMachine handles Mouse based events on the Object, Such as OnMouseButton, OnMouseEnter, OnMouseDrag etc. it can also filter targets based on their tag.

# Element_EventMachine - 90% Done (functional)
This EventMachine handles Trigger Events based on the other objects "Reactor" element, for example you could take a torch that has a Reaction for the "Fire" element, by lighting itself and also making itself an emitter of "Fire" enabling it to ignite things around it that react to the "Fire" reactor.

Simple Barrel Example
![Example Result](https://i.imgur.com/QeV81Vr.gif)
![Example Result 2](https://i.imgur.com/CZFuEVT.gif)
