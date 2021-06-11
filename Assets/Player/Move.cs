using System;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.Animations;

public class Move : MonoBehaviour
{
    // Movement code provided from Renaissance Coders youtube tutorial

    private float velocity = 10;
    private float turnSpeed = 10;
    private float height = 0.5f;
    private float heightPadding = 0.15f;
    private float rotationThreshold = 1f;
    [SerializeField] private LayerMask ground;
    private float maxSlopeAngle = 120f;
    [SerializeField] private bool debug;
    private Rigidbody rb;
    
    private Vector2 input;
    private float angle;
    private float slopeAngle;

    private Quaternion targetRotation;
    private Transform cam;

    private Vector3 forward;
    private RaycastHit hitInfo;
    private bool grounded;

    private void Start()
    {
        cam = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        GetInput();
        CalculateDirection();
        CalculateForward();
        CalculateSlopeAngle();
        CheckGround();
        ApplyGravity();
        DrawDebugLines();
        Stand();
        Rotate();
        Walk();
        
    }

    private void GetInput()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
    }

    private void CalculateDirection()
    {
        angle = Mathf.Atan2(input.x, input.y);
        angle = Mathf.Rad2Deg * angle;
        angle += cam.eulerAngles.y;
    }

    private void Rotate()
    {
        if (Math.Abs(input.x) < 1 && Math.Abs(input.y) < 1) { return; }
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    private void Walk()
    {
        if (!grounded || slopeAngle > maxSlopeAngle || (Math.Abs(input.x) < 1 && Math.Abs(input.y) < 1) || Quaternion.Angle(targetRotation, transform.rotation) > rotationThreshold) 
        { 
            rb.velocity = Vector3.zero;
            return;
        }
        
        //transform.position += forward * velocity * Time.deltaTime;
        //rb.MovePosition(transform.position + forward * (velocity * Time.deltaTime));;
        float speedMultiplier = 0.6f;
        rb.velocity = forward * (velocity * speedMultiplier);
    }

    private void CalculateForward()
    {
        if (!grounded)
        {
            forward = transform.forward;
            return;
        }

        forward = Vector3.Cross(transform.right, hitInfo.normal);
    }

    private void CalculateSlopeAngle()
    {
        if (!grounded)
        {
            slopeAngle = 90f;
            return;
        }

        slopeAngle = Vector3.Angle(hitInfo.normal, transform.forward);
    }

    private void CheckGround()
    {
        if (Physics.Raycast(transform.position, -Vector3.up, out hitInfo, height + heightPadding, ground))
        {
            if (Vector3.Distance(transform.position, hitInfo.point) < height)
            {
                transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * height,
                    5f * Time.deltaTime);
            }
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    private void ApplyGravity()
    {
        if (!grounded)
        {
            transform.position += Physics.gravity * Time.deltaTime;
        }
    }

    private void DrawDebugLines()
    {
        if (!debug) { return; }
        Debug.DrawLine(transform.position, transform.position + forward * (height * 2f), Color.blue);
        Debug.DrawLine(transform.position, transform.position - Vector3.up * height, Color.green);
    }

    private void Stand()
    {
        transform.rotation = Quaternion.LookRotation(forward, hitInfo.normal);
    }
}
