using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Affector))]
public class Element_EventMachine : MonoBehaviour
{
    public UnityEvent<Element, GameObject, Vector3, Collider> OnAffector;
    private Affector affector;

    private void Start()
    {
        affector = this.GetComponent<Affector>();    
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Affector>() != null)
        {
            Element otherAffector = other.GetComponent<Affector>().GetElement();

            OnAffector.Invoke(otherAffector, other.gameObject, other.ClosestPoint(this.transform.position), other);
        }
    }
}
