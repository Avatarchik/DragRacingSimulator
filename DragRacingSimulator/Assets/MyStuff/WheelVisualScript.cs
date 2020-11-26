using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelVisualScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    Vector3 StartPosition;

    /*bool start_rot_taken = false;
    Quaternion start_rot;*/

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (transform.GetChild(i).transform.localScale.x > 0)
            {
                transform.GetChild(i).transform.localRotation = Quaternion.Inverse(Quaternion.Euler(0, this.GetComponent<WheelCollider>().steerAngle, 0));
            }
            else 
            {
                transform.GetChild(i).transform.localRotation = Quaternion.Euler(0, this.GetComponent<WheelCollider>().steerAngle, 0);
            }
            if (!float.IsNaN(this.GetComponent<WheelCollider>().rpm))
            {
                transform.GetChild(i).transform.Rotate(Vector3.right * this.GetComponent<WheelCollider>().rpm * 2 * Mathf.PI / 6.0f * Time.deltaTime * Mathf.Rad2Deg);
            }
            if (StartPosition == null) 
            {
                StartPosition = transform.GetChild(i).transform.localPosition;
            }
            transform.GetChild(i).transform.localPosition = StartPosition + new Vector3(0, this.GetComponent<WheelCollider>().contactOffset, 0);
        }
    }
}
