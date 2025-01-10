using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform followTarget;
    public float speed = 8f;
    public Vector3 offset = new Vector3(0, 10, 10);

    void LateUpdate()
    {
        Vector3 targetPosition = followTarget.position+ offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
        
    }
}
