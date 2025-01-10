using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public float Speed
    {
        get
        {
            return forwardSpeed;
        }
    }

    public float forwardSpeed = 10f;
    public float rotateSpeed = 5f;


    private Rigidbody rb;
    private Animator animator;

    public readonly int hashMove = Animator.StringToHash("Move");

    public static readonly string horizontalAxisName = "Horizontal";
    public static readonly string verticalAxisName = "Vertical";

    private Plane plane = new Plane(Vector3.up, Vector3.zero);

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        float horz = Input.GetAxisRaw(horizontalAxisName);
        float verz = Input.GetAxisRaw(verticalAxisName);

        Vector3 deltaMove = new Vector3(horz, 0, verz).normalized * Speed * Time.deltaTime;
        animator.SetFloat(hashMove, deltaMove.magnitude);

        rb.MovePosition(rb.position + deltaMove);
    }

    private void Rotate()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out var distance))
        {
            var hitPoint = ray.GetPoint(distance);
            transform.LookAt(hitPoint);
        }
    }

}
