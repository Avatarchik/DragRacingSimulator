using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTransmission : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public WheelCollider K1;
    public WheelCollider K2;
    public WheelCollider K1p;
    public WheelCollider K2p;
    public string AWDFWDRWD;
    public float TorqueToApply;
    public float FinalTorque;
    public int CurrentGear;
    public float NumberOfGears;
    public float[] GearRatios;
    public float CurrentGearRatio;
    public float ClutchEngagement;
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.X))
        {
            ClutchEngagement = 0f;
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            ClutchEngagement = 1f;
        }

        //NumberOfGears (1 is neutral)
        if (Input.GetKeyDown(KeyCode.A) & CurrentGear < NumberOfGears)
        {
            CurrentGear++;
        }
        if (Input.GetKeyDown(KeyCode.Z) & CurrentGear > 0)
        {
            CurrentGear--;
        }

        CurrentGearRatio = GearRatios[CurrentGear];
        if (CurrentGearRatio != 0)
        {
            FinalTorque = TorqueToApply / CurrentGearRatio;
        }
        else 
        {
            FinalTorque = 0;
        }

        if (AWDFWDRWD == "RWD")
        {
            K1p.motorTorque = K2p.motorTorque = 0;
            K1.motorTorque = K2.motorTorque = FinalTorque;
        }
        else if (AWDFWDRWD == "FWD")
        {
            K1p.motorTorque = K2p.motorTorque = FinalTorque;
            K1.motorTorque = K2.motorTorque = 0;
        }
        else if (AWDFWDRWD == "AWD")
        {
            K1p.motorTorque = K2p.motorTorque = FinalTorque / 2;
            K1.motorTorque = K2.motorTorque = FinalTorque / 2;
        }

        float WheelRPM = 0;
        if (AWDFWDRWD == "RWD")
        {
            if (!float.IsNaN(K1.rpm))
            {
                WheelRPM = K1.rpm;
            }
        }
        else if (AWDFWDRWD == "FWD")
        {
            if (!float.IsNaN(K1.rpm))
            {
                WheelRPM = K1p.rpm;
            }
        }
        else if (AWDFWDRWD == "AWD")
        {
            if (!float.IsNaN(K1.rpm))
            {
                WheelRPM = K1p.rpm;
            }
        }
        WheelsRPM = WheelRPM;
    }

    public float WheelsRPM;

    public float GetDirvetrainRPM() 
    {
        return WheelsRPM / CurrentGearRatio;
    }
}
