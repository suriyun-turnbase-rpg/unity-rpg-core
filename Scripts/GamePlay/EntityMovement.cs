using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    public enum EHeadDirection2D
    {
        Right,
        Left,
    }

    public float speed = 2.0f;
    public Vector3 target;
    public bool doNotRotate;
    public EHeadDirection2D HeadDirection2D { get; private set; }
    public bool IsMoving { get; private set; }
    public Vector3 DesiredVelocity { get; private set; }
    public Quaternion DesiredRotation { get; private set; }
    private float lastSqrMag;

    public void StartMove(Vector3 target, float speed)
    {
        this.speed = speed;
        this.target = target;
        // calculate directional vector to target
        var heading = target - transform.position;
        var directionalVector = heading.normalized;

        // reset lastSqrMag
        lastSqrMag = Mathf.Infinity;

        if (directionalVector.magnitude > 0)
        {
            DesiredVelocity = directionalVector * speed;
            DesiredRotation = Quaternion.LookRotation(directionalVector);
            IsMoving = true;
        }
    }

    public void StopMove()
    {
        IsMoving = false;
        DesiredVelocity = Vector3.zero;
    }

    private void Update()
    {
        if (!IsMoving)
            return;

        // check the current sqare magnitude
        var heading = target - transform.position;
        var sqrMag = heading.sqrMagnitude;

        // check this against the lastSqrMag
        // if this is greater than the last,
        // entity has reached target and is now moving past it
        if (sqrMag > lastSqrMag)
        {
            StopMove();
            return;
        }

        // make sure you update the lastSqrMag
        lastSqrMag = sqrMag;

        transform.position = transform.position + DesiredVelocity * Time.deltaTime;
        HeadTo(DesiredRotation);
    }

    public void HeadTo(Quaternion rotation)
    {
        if (!doNotRotate)
            transform.rotation = rotation;
        var dir = rotation * Vector3.forward;
        if (dir.normalized.x > 0)
            HeadDirection2D = EHeadDirection2D.Right;
        else
            HeadDirection2D = EHeadDirection2D.Left;
    }
}
