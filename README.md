# UnityObjectActionSystem

Object Action System is a ScriptableObject based action system, allowing you to drag and drop actions, or a collection of actions to be performed in response to an event.

ObjectActionSystem uses DoTween for tweening. http://dotween.demigiant.com/

## What it's For 

OAS is designed to be a simple drag, drop and tweak collection of Components, Editors, Scripts and ObjectActions allowing you to quickly add simple interactive elements and objects to your game and/or project. 

From simple examples such as, playing a particle effect on left clicking a GameObject, or scaling the object up.

<img src="https://i.imgur.com/rmVfad4.gif">

Or Starting a fire 
[Fire Example on Imgur](https://imgur.com/WIaHxtK)

<img src="https://i.imgur.com/ACW14x6.mp4">

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

## So What actions are available? 
Currently there are only a small selection of actions available, of which over the coming weeks I will list and document, but you can expect most methods from common Components to be added, for now you can check the Actions folder!

That being said, it is easy to implement your own custom ObjectAction.

1. Extend From *ObjectAction* then override *Init* to add default values if they are needed, it's important to note, these are default values, that are used by the custom editor to dictate how the data tab is displayed, the actual values used are the ones you set in the inspector, but you still need these! 

Secondly, Overriding *Execute* is where you write your custom action behaviour using data.GetStringValue(string name), GetFloatValue(string name), GetIntValue(string name), GetVectorValue(string name), GetPrefabValue(string name), GetBoolValue(string name), GetSOValue(string name) to retrieve the values set in the inspector for the action, make sure to *yield break* as the very last thing done!

Below is how LerpShaderGraphFloatValue is written.

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [CreateAssetMenu(fileName = "Lerp ShaderGraph Float Value", menuName = scriptObjectPath + "Lerp ShaderGraph Float Value")]
public class LerpShadergraphFloatValue : ObjectAction
{
    public override void Init()
    {
        base.Init();
        //Adds a Default String Value to the Scriptable Object, that can then be changed in the inspector to suit the particular need by the user.
        AddDefaultStringValue("PropertyName", "DissolveValue");
        AddDefaultFloatValue("Value", 0f);
        AddDefaultFloatValue("LerpTime", 1f);
    }

    public override IEnumerator Execute(BaseController _controller, ActionData data, GameObject target, Vector3 hitpoint)
    {
        Renderer targetObject = null;
        //some actions may target either the object running the controller or the target that triggered it.
        //You can choose to implement this or not
        switch (data.targetType)
        {
            case ActionData.GameObjectActionTarget.SELF:
                targetObject = _controller.GetComponent<Renderer>();
                break;
            case ActionData.GameObjectActionTarget.TARGET:
                targetObject = target.GetComponent<Renderer>();
                break;
        }
        //This is one way to retrieve the value set by the user to be used at run time
        //string PropertyNameToChange = data.GetStringValue("PropertyName");
        yield return LerpFloatValue(targetObject, data.GetStringValue("PropertyName"), data.GetFloatValue("Value"), data.GetFloatValue("LerpTime"));


        yield break;
    }


    private IEnumerator LerpFloatValue(Renderer targetObject, string propertyName, float value, float lerpTime)
    {
        float timer = 0;
        float currentValue = targetObject.material.GetFloat(propertyName);

        while (timer < lerpTime)
        {
            timer += Time.deltaTime;

            targetObject.material.SetFloat(propertyName, Mathf.Lerp(currentValue, value, timer / lerpTime));

            yield return null;
        }

        timer = 0f;

        yield break;
    }
}
```


## How do I install and use it?

Download the Repo and drag the "Object Action System" folder into your project, you will need to also install DoTween (Which is excellent), if you don't already have it! 

Once everything is imported and ready - 

Add a __EventMachine__ to an Object followed by an __ActionController__, once this is done add an event from the list of __EventMachine__ events that you want to fire on, drag the Object you want to react into the object slot then in the function drop down, select the ActionController and then which kind of reaction method you wish to call.

__*StartNamedReaction(string name)*__

If you want to call a particular named reaction this is useful for object which have multiple similar but different events, such as a light, which may have a reaction to Toggle Lights on/off, but another two reactions specifically for TurningOff the light and TurningOn respectively. 

You may want the light to toggle on when it is clicked, but you may also want a button that can turn off all attached lights and another to turn them back on. So you would have three reactions "Toggle", "TurnOn" and "TurnOff".

You must pass the name of the reaction you wish to start in the inspector.

__*StartReaction*__

A much simpler method for when an object will have only one reaction, this dynamically passes variables to StartReaction(GameObject target, Vector3 hitPosition, Collider targetCollider), this always calls the first reaction in the list.

## How it Works

The ObjectActionSystem (OAS) is made up of two main parts, a __EventMachine__ and a __ActionController__, there is also a custom Editor for the ActionControllers to make designing new reactions more pain free than through the standard unity UI.


__EventMachine__

This handles the calling of events, there are 3 types of EventMachines so far, with most using unity's built-in tag system for filtering events. 


__EventMachine__

<img src="https://i.imgur.com/PYKybPT.png">

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

1. OnAffector : When another Trigger collider enters the __EventMachine__'s GameObject, with an Affector element for which there is a defined reaction. 


__ActionController__

The Controllers come in two different types, the __ActionController__ and __ElementActionController__, these handle the execution and storing of per-object data for the actions.

<img src="https://i.imgur.com/z2fNjCf.png">

1. __ActionController

The __ActionController__ is the default controller, it's provides two functions to call from the EventMachine events

 1. *StartNamedReaction(string name)* - Finds a reaction by it's name then executes it.

 2. *StartReaction* - Executes the first reaction in it's list, regardless of name.
 
 2. __Element_ActionController__ is the second type of controller, identical to the first except in that, this one uses reactions defined by "Element" (ScriptableObject) instead of string names.


### TO DO
1. Custom Editor For the Event Machines!
2. More Actions!
