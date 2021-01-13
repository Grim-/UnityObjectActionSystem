using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Mouse_EventMachine : MonoBehaviour
{
    public string[] triggerTags;
    public string[] ignoreTags;

    public UnityEvent<GameObject, Vector3, Collider> OnMouseAsButton;
    public UnityEvent<GameObject, Vector3, Collider> OnDragMouse;

    public UnityEvent<GameObject, Vector3, Collider> OnEnterMouse;
    public UnityEvent<GameObject, Vector3, Collider> OnExitMouse;

    public UnityEvent<GameObject, Vector3, Collider> OnUpMouse;
    public UnityEvent<GameObject, Vector3, Collider> OnDownMouse;
    public UnityEvent<GameObject, Vector3, Collider> OnOverMouse;

    private void OnMouseDrag()
    {
        if (CheckTags(this.gameObject))
        {
            OnDragMouse.Invoke(this.gameObject, this.transform.position, this.GetComponent<Collider>());
        }
    }

    private void OnMouseUpAsButton()
    {
        if (CheckTags(this.gameObject))
        {
            OnMouseAsButton.Invoke(this.gameObject, this.transform.position, this.GetComponent<Collider>());
        }       
    }

    private void OnMouseEnter()
    {
        if (CheckTags(this.gameObject))
        {
            OnEnterMouse.Invoke(this.gameObject, this.transform.position, this.GetComponent<Collider>());
        }
    }

    private void OnMouseDown()
    {
        if (CheckTags(this.gameObject))
        {
            OnDownMouse.Invoke(this.gameObject, this.transform.position, this.GetComponent<Collider>());
        }
    }

    private void OnMouseExit()
    {
        if (CheckTags(this.gameObject))
        {
            OnExitMouse.Invoke(this.gameObject, this.transform.position, this.GetComponent<Collider>());
        }
    }

    private void OnMouseOver()
    {
        if (CheckTags(this.gameObject))
        {
            OnOverMouse.Invoke(this.gameObject, this.transform.position, this.GetComponent<Collider>());
        }
    }

    private void OnMouseUp()
    {
        if (CheckTags(this.gameObject))
        {
            OnUpMouse.Invoke(this.gameObject, this.transform.position, this.GetComponent<Collider>());
        }
    }

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
}
