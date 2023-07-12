using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class movePlatform : MonoBehaviour
{
    //get the positions of the platform, startposition, and end position
    [SerializeField] private Transform platform;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    private Vector2 target;

    int direction = 1;
    float speed = 0.5f;

    private void Start()
    {
        Invoke("changeDirection", 3);
    }
    private void Update()
    {
        // get the current target location
        target = currentTargetLocation();


        platform.position = Vector2.Lerp(platform.position, target, speed*Time.deltaTime);

        changeDirection();


    }

    // this will return the current target location
    Vector2 currentTargetLocation()
    {
        if (direction == 1)
        {
            return startPoint.position;
        }
        else
        {
            return endPoint.position;
        }
    }

    private void OnDrawGizmos()
    {
        if (platform!=null && startPoint!=null && endPoint!=null) 
        {
            Gizmos.DrawLine(platform.position, startPoint.position);
            Gizmos.DrawLine(platform.position, endPoint.position);
        }
    }
    private void changeDirection()
    {
        /* always make the platform go to the opposite point.
        This is done by multiplying the direction by -1 which will always be the opposite no matter if its 1 or -1*/
        float distance = (target - (Vector2)platform.position).magnitude;

        if (distance < 0.1f)
        {
            direction *= -1;
        }

    }
}    
