using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class Ball : MonoBehaviour, IGrabbable
{
    private bool grabbed = false;
    private Transform anchor = default;
    private float followSpeed = 20f;
    private float maxAnchorDistance = 2.5f;
    private float anchorDistance = default;
    private const float closeEnough = 0.5f;
    [SerializeField] private Rigidbody rb;

    public void Grab(Transform anchor)
    {
        grabbed = true;
        this.anchor = anchor;
        transform.position = anchor.position;
        anchorDistance = 0f;
        rb.useGravity = false;
    }

    public void Release()
    {
        grabbed = false;
        anchor = null;
        rb.useGravity = true;
    }

    private void Update()
    {
        if (!grabbed) { return; }

        anchorDistance = Vector3.Distance(transform.position, anchor.position);

        if (anchorDistance > closeEnough)
        {
            //transform.position = Vector3.MoveTowards(transform.position, anchor.position, followSpeed * Time.deltaTime);
            //rb.velocity = Vector3.MoveTowards(transform.position, anchor.position, followSpeed);
            float speedMultiplier = 100f;
            rb.velocity = (anchor.position - transform.position).normalized * (followSpeed * speedMultiplier * Time.deltaTime);
        }
        else
        {
            rb.velocity = Vector3.zero;
            rb.MovePosition(anchor.position);
        }
        
        if (anchorDistance > maxAnchorDistance)
        {
            Release();
        }

    }
}
