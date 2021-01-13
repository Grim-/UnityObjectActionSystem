using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance <= 0.1f) target = null;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 1f);
        }

    }
}
