using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCamera : MonoBehaviour
{
    public static bool InCar = false;

    public GameObject Target;

    public Vector3 OffsetPosition;
    public Vector3 TargetPosition;
    public float height;
    public float rotation_smoothness;
    public float height_smoothness;

    Vector3 velocity = Vector3.zero;

    void Update()
    {
        if (InCar)
        {
            this.transform.position = Target.transform.GetComponentInChildren<DriverHead>().gameObject.transform.position;
            this.transform.rotation = Target.transform.GetComponentInChildren<DriverHead>().gameObject.transform.rotation;
        }
        else 
        { 
            if (Target == null)
            {
                if (this.transform.parent.GetComponent<CarEngine>() != null)
                {
                    Target = this.transform.parent.gameObject;
                }
            }
            else
            {
                float wantedRotationAngle = Target.transform.eulerAngles.y;
                float wantedHeight = Target.transform.position.y + 2;

                float currentRotationAngle = transform.eulerAngles.y;
                float currentHeight = transform.position.y;

                // Damp the rotation around the y-axis
                currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotation_smoothness * Time.deltaTime);

                // Damp the height
                currentHeight = Mathf.Lerp(currentHeight, wantedHeight, height_smoothness * Time.deltaTime);

                // Convert the angle into a rotation
                var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
                transform.position = Target.transform.position - currentRotation * (OffsetPosition * -1);

                this.transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

                this.transform.LookAt(Target.transform.TransformPoint(-OffsetPosition + new Vector3(0, height, 0)));
            }
        }
    }
}
