using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : MonoBehaviour
{
    public float lookRadius;

    private Transform target;
    

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerController.playerInstance.transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            
        }
    }
}
