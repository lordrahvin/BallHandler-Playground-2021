using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Grab : MonoBehaviour
{
    //references
    [SerializeField] private Transform grabTarget;
    [SerializeField] private Transform grabOrigin;
    [SerializeField] private Transform grabAnchor;
    [SerializeField] private MeshRenderer meshRenderer;
    
    //raycast variables
    private Vector3 start = default;
    private Vector3 end = default;
    private LayerMask ballLayer = default;
    private float maxRayDist = default;

    //grabber status
    private GameObject holding = null;
    private bool grabInProgress = false;

    //grab parameters
    private float grabDuration = 2f;
    
    //grabber positioning
    private float height = default;
    private float length = default;
    private Vector3 grabPos = default;
    
    
    void Start()
    {
        ballLayer = LayerMask.NameToLayer("Ball");
        ballLayer = ~ballLayer;
        grabTarget.SetParent(null, true);
        grabPos = grabTarget.position;
        length = Vector3.Distance(transform.position, grabPos);
        ReadyGrab();
        MoveGrabber();
    }

    void MoveGrabber()
    {
        grabPos = grabOrigin.position + (transform.forward * length);
        grabTarget.position = grabPos;
    }

    // Update is called once per frame
    void Update()
    {
        MoveGrabber();
        if (Controls.Instance.TryGrab && !grabInProgress)
        {
            grabInProgress = true;
            TryGrab();
        }
        
    }

    void TryGrab()
    {
        end = grabTarget.position;
        start = grabOrigin.position;
        maxRayDist = Vector3.Distance(start, end);
        
        RaycastHit hitInfo;
        bool grabbed = Physics.Raycast(start, end - start, out hitInfo, maxRayDist, ballLayer);

        
        if (grabbed)
        {
            GrabSuccess(hitInfo);
        }
        else
        {
            GrabFail();
        }
    }

    private void GrabSuccess(RaycastHit hitInfo)
    {
        ColorChange(Color.green);
        Invoke(nameof(ReadyGrab), grabDuration);
        
        IGrabbable grabbedObj = hitInfo.collider.gameObject.GetComponent<IGrabbable>();
        if (grabbedObj == null) { return;}

        holding = hitInfo.collider.gameObject;
        grabbedObj.Grab(grabAnchor);
    }

    private void GrabFail()
    {
        ColorChange(Color.red);
        Invoke(nameof(ReadyGrab), grabDuration);
    }

    private void ColorChange(Color color)
    {
        if (meshRenderer)
        {
            meshRenderer.material.SetColor("_Color", color);
        }
    }

    private void ReadyGrab()
    {
        ColorChange(Color.white);
        grabInProgress = false;
    }
}
