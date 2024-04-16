using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraMover : MonoBehaviour
{
    public Transform objectToRotate; // The object you want to rotate
    public Vector3 centerPoint; // The center point around which the rotation occurs
    public Vector3 lookAtPoint;
    public float rotationSpeed = 1f; // Speed of rotation

    private void Update()
    {
        // Calculate the position relative to the center point
        Vector3 relativePos = objectToRotate.position - centerPoint;

        // Calculate the angle of rotation in radians
        float angle = Mathf.Atan2(relativePos.z, relativePos.x);

        // Add the rotation speed to the angle
        angle += rotationSpeed * Time.deltaTime;

        // Calculate the new position after rotation
        Vector3 newPos = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * relativePos.magnitude;

        // Apply the rotation
        objectToRotate.position = centerPoint + newPos;
        objectToRotate.LookAt(lookAtPoint);
    }
}
