using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    private Quaternion originalRotation;
    public Transform originalTransform;

    private void Awake()
    {
        originalRotation = transform.rotation;
    }
    public Quaternion getOriginalRotation()
    {
        return originalRotation;
    }

    public Transform getOriginalTransform()
    {
        return originalTransform;
    }
}
