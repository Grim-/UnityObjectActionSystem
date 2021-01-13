using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventMachine : MonoBehaviour
{
    #region Inspector Variables

    public string[] triggerTags;
    public string[] ignoreTags;

    public UnityEvent<GameObject, Vector3, Collider> OnEnterTrigger;

    public UnityEvent<GameObject, Vector3, Collider> OnStayTrigger;

    public UnityEvent<GameObject, Vector3, Collider> OnExitTrigger;

    public UnityEvent<GameObject, Vector3, Collider> OnEnterCollision;

    public UnityEvent<GameObject, Vector3, Collider> OnStayCollision;

    public UnityEvent<GameObject, Vector3, Collider> OnExitCollision;

    public UnityEvent<GameObject, Vector3> OnCollisionParticle;
    #endregion

    /// <summary>
    /// Checks if a GameObject has atleast one valid tag, defaults to true when no tags are present in list
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private bool CheckTags(GameObject target)
    {
        if (triggerTags.Length == 0) return true;

        foreach (var triggerTag in triggerTags)
        {          
            if (target.tag == triggerTag) return true;
            else continue;               
        }    
        return false;
    }

    private void OnParticleCollision(GameObject other)
    {
        List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

        int numCollisionEvents = this.GetComponent<ParticleSystem>().GetCollisionEvents(other, collisionEvents);

        for (int i = 0; i < numCollisionEvents; i++)
        {
            for (int x = 0; x < triggerTags.Length; x++)
            {
                if (CheckTags(other.gameObject))
                {
                    OnCollisionParticle.Invoke(other.gameObject, collisionEvents[i].intersection);
                    break;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (CheckTags(collision.gameObject))
        {
            if (collision.contactCount > 0)
            {
                OnEnterCollision.Invoke(collision.gameObject, collision.contacts[0].point, collision.collider);
            }
            else
            {
                OnEnterCollision.Invoke(collision.gameObject, collision.transform.position, collision.collider);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (CheckTags(collision.gameObject))
        {
            if (collision.contactCount > 0)
            {
                OnStayCollision.Invoke(collision.gameObject, collision.contacts[0].point, collision.collider);
            }
            else
            {
                OnStayCollision.Invoke(collision.gameObject, collision.transform.position, collision.collider);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (CheckTags(collision.gameObject))
        {
            if (collision.contactCount > 0)
            {
                OnExitCollision.Invoke(collision.gameObject, collision.contacts[0].point, collision.collider);
            }
            else
            {
                OnExitCollision.Invoke(collision.gameObject, collision.transform.position, collision.collider);
            }
        }            
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CheckTags(other.gameObject))
        {
            OnEnterTrigger.Invoke(other.gameObject, other.ClosestPoint(this.transform.position), other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (CheckTags(other.gameObject))
        {
            OnStayTrigger.Invoke(other.gameObject, other.ClosestPoint(this.transform.position), other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (CheckTags(other.gameObject))
        {
            OnExitTrigger.Invoke(other.gameObject, other.ClosestPoint(this.transform.position), other);
        }            
    }
}