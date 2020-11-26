using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    [SerializeField]
    public GameObject[] cameraObjects;

    private GameObject current_camera;
    private GameObject Current_camera
    {
        get 
        {
            return current_camera; 
        }
        set 
        { 
            current_camera = value; 
            Current_camera_changed(); 
        }
    }

    private void Current_camera_changed()
    {
        foreach (var camera in GameObject.FindObjectsOfType<Camera>())
        {
            camera.gameObject.GetComponent<Camera>().enabled = false;
            if (camera.gameObject.GetComponent<AudioListener>() != null)
            {
                camera.gameObject.GetComponent<AudioListener>().enabled = false;
            }
        }
        Current_camera.gameObject.GetComponent<Camera>().enabled = true;
        if (Current_camera.gameObject.GetComponent<AudioListener>() != null)
        {
            Current_camera.gameObject.GetComponent<AudioListener>().enabled = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Current_camera = cameraObjects[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (CarSteering.ControlACar)
            {

                for (int i = 0; i < cameraObjects.Length; i++)
                {
                    if (cameraObjects[i].GetComponent<CarCamera>()!=null)
                    {
                        Current_camera = cameraObjects[i].gameObject;
                    }
                }
                CarCamera.InCar = !CarCamera.InCar;
            }
            else
            {
                Debug.Log("Pressed C");
                int CurrenCameraId = 0;
                for (int i = 0; i < cameraObjects.Length; i++)
                {
                    Debug.Log("for i " + i);
                    if (cameraObjects[i] == Current_camera)
                    {
                        CurrenCameraId = i;
                        Debug.Log("for CurrenCameraId " + CurrenCameraId);
                    }
                }
                if (CurrenCameraId == cameraObjects.Length - 1)
                {
                    Current_camera = cameraObjects[0].gameObject;
                }
                else
                {
                    Current_camera = cameraObjects[CurrenCameraId + 1].gameObject;
                }
            }
        }
    }
}
