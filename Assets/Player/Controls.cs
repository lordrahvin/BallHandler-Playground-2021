using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public static Controls Instance = null;
    private const float threshold = 0.1f;

    public bool WalkForward { get; private set; } = false;
    public bool RotateRight { get; private set; } = false;
    public bool RotateLeft { get; private set; } = false;
    public bool TryGrab { get; private set; } = false;
    
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }

    private void ResetInput()
    {
        WalkForward = false;
        RotateRight = false;
        RotateLeft = false;
        TryGrab = false;
    }

    
    void Update()
    {
        ResetInput();
        if (Input.GetAxis("Vertical") > threshold)
        {
            WalkForward = true;
        }
        
        if (Input.GetAxis("Horizontal") > threshold)
        {
            RotateRight = true;
        }

        if (Input.GetAxis("Horizontal") < -threshold)
        {
            RotateLeft = true;
        }

        if (Input.GetButton("Fire1") || Input.GetKey(KeyCode.Space))
        {
            TryGrab = true;
        }
        
    }
}
