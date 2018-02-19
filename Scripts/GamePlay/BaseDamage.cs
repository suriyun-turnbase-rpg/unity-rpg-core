using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(TargetingRigidbody))]
public abstract class BaseDamage : MonoBehaviour
{
    private Transform tempTransform;
    public Transform TempTransform
    {
        get
        {
            if (tempTransform == null)
                tempTransform = GetComponent<Transform>();
            return tempTransform;
        }
    }
    private Rigidbody tempRigidbody;
    public Rigidbody TempRigidbody
    {
        get
        {
            if (tempRigidbody == null)
                tempRigidbody = GetComponent<Rigidbody>();
            return tempRigidbody;
        }
    }
    private TargetingRigidbody tempTargetingRigidbody;
    public TargetingRigidbody TempTargetingRigidbody
    {
        get
        {
            if (tempTargetingRigidbody == null)
                tempTargetingRigidbody = GetComponent<TargetingRigidbody>();
            return tempTargetingRigidbody;
        }
    }
}
