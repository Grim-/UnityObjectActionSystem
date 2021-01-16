using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Affector : MonoBehaviour
{
    public Element element;

    public UnityEvent<Element, Element> OnAffectorChanged;

    private void Start()
    {
        CheckForTriggerCollider();
    }

    public void CheckForTriggerCollider()
    {
        Collider[] colliders = this.GetComponents<Collider>();
        bool triggerColliderFound = false;

        foreach (var collider in colliders)
        {
            if (triggerColliderFound) break;
            if (collider.isTrigger == true)
            {
                triggerColliderFound = true;
                break;
            }
            else continue;           
        }
        if (!triggerColliderFound)
        {
            Debug.Log("No Collider with isTrigger Could be found on " + this.transform.name + " this is required to fire event and recieve events.");
        }
    }

    public void SetElement(Element newElement)
    {     
        if (newElement != element)
        {
            element = newElement;
            OnAffectorChanged.Invoke(element, newElement);
        }
    }

    public Element GetElement()
    {
        return element;
    }
}
