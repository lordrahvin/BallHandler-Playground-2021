using System;
using UnityEngine;

public class Move : MonoBehaviour
{
    // Movement code provided from Renaissance Coders youtube tutorial

    private float velocity = 10;
    private float turnSpeed = 5;
    private float height = 0.5f;
    private float heightPadding = 0.15f;
    [SerializeField] private LayerMask ground;
    private float maxSlopeAngle = 120f;
    [SerializeField] private bool debug; 
    
    
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
        
        if (Math.Abs(input.x) < 1 && Math.Abs(input.y) < 1) { return; }
       
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
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    private void Walk()
    {
        if (slopeAngle > maxSlopeAngle) { return;}
        transform.position += forward * velocity * Time.deltaTime;
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
}
