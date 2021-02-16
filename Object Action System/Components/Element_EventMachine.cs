using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Affector))]
public class Element_EventMachine : MonoBehaviour
{
    public List<ExTag> triggerTags;
    public UnityEvent<Element, GameObject, Vector3, Collider> OnAffector;
    private Affector affector;

    private void Start()
    {
        affector = this.GetComponent<Affector>();    
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Affector>() != null)
        {
            Element otherAffector = other.GetComponent<Affector>().GetElement();

            if (otherAffector != null && CheckTags(other.gameObject))
            {
                OnAffector.Invoke(otherAffector, other.gameObject, other.ClosestPoint(this.transform.position), other);
            }                      
        }
    }


    /// <summary>
    /// Checks if a GameObject has atleast one valid tag, defaults to true when no tags are present in list
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private bool CheckTags(GameObject target)
    {
        Tag targetTag = target.GetComponent<Tag>();

        if (triggerTags.Count == 0) return true;

        foreach (var triggerTag in triggerTags)
        {          
            if (targetTag != null && targetTag.HasTag(triggerTag)) return true;
            else continue;
        }
        return false;
    }

}
