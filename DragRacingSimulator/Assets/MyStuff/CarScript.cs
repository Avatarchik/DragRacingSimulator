using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Text;
using JetBrains.Annotations;

public class CarScript : MonoBehaviour
{

    public class Wheel
    {
        public GameObject model;
        public GameObject wheelObject;
        public WheelCollider wheelCollider;
        public float wheelRadius = 15;
        public float tireRadius = 2;
        public float wheelWidth = 7;
        public float tireWidth = 7;
        public float grip = 0.7f;
        public float psi = 40f;
        public float mass = 7;

        void DestroyWheel()
        {
            Destroy(model);
            model = null;
            wheelCollider = null;
            wheelRadius = 0;
            tireRadius = 2;
            wheelWidth = 7;
            tireWidth = 7;
            grip = 0;
        }

        public Wheel Create(string p_model = "0", float p_grip = 0.7f, float p_psi = 40f, float p_wheelRadius = 15, float tireRadius = 2, float wheelWidth = 7, float tireWidth = 7)
        {
            wheelObject = new GameObject();
            wheelCollider = wheelObject.AddComponent<WheelCollider>();
            wheelCollider.radius = p_wheelRadius + tireRadius;
            //wheelCollider.
            return this;
        }
    }
    public class WheelBase
    {
        //Wheels
        public Wheel FL_WHEEL = new Wheel().Create();
        public Wheel FR_WHEEL = new Wheel().Create();
        public Wheel RL_WHEEL = new Wheel().Create();
        public Wheel RR_WHEEL = new Wheel().Create();

        public float WheelBaseLength;
        public float FrontWidth;
        public float RearWidth;

        public bool FL_BrokenAxle = false;
        public bool FR_BrokenAxle = false;
        public bool RL_BrokenAxle = false;
        public bool RR_BrokenAxle = false;

        //Suspension------------------------------------------------------
        public float FrontSprings;
        public float FrontShocks;
        public AnimationCurve FrontWheelTravelRotation;
        public float FrontWheelsMaxHeight;
        public float FrontWheelsMinHeight;

        public float RearSprings;
        public float RearShocks;
        public AnimationCurve RearWheelTravelRotation;
        public float RearWheelsMaxHeight;
        public float RearWheelsMinHeight;

        //PowerTrain------------------------------------------------------
        //public float DiffSlip;        
        //public string DriveTrainType;//RWD FWD AWD 4WD

        public float FrontDiffSlip;
        public float RearDiffSlip;

        public float PowerToFront;
        public float PowerToRear;

        public float AxleBreakTorque;
        public float ClutchSlipTorque;

        //Brakes...------------------------------------------------------
        public float FrontBrakesPower;
        public float FrontBrakesCoolingSpeed;

        //Steering...------------------------------------------------------
        public float SteeringAngle;

        //Functions------------------------------------------------------
        public float AVGRPM()
        {
            float Sum = 0;
            int NumberOfPoweredWheels = 0;
            if (PowerToFront > 0)
            {
                Sum += FL_WHEEL.wheelCollider.rpm;
                Sum += FR_WHEEL.wheelCollider.rpm;
                NumberOfPoweredWheels += 2;
            }
            if (PowerToRear > 0)
            {
                Sum += RL_WHEEL.wheelCollider.rpm;
                Sum += RR_WHEEL.wheelCollider.rpm;
                NumberOfPoweredWheels += 2;
            }
            if (NumberOfPoweredWheels > 0)
            {
                return Sum / NumberOfPoweredWheels;
            }
            else
            {
                return 0;
            }
        }
        public void ApplyTorque(float torque)
        {
            float CurrentClutchPercentage = ((Clutch * -1) + 1);

            torque = torque * CurrentClutchPercentage;

            float FL_torque = 0;
            float FR_torque = 0;
            float RL_torque = 0;
            float RR_torque = 0;

            //ClutchSlipping
            if (torque > ClutchSlipTorque * CurrentClutchPercentage)
            {
                torque = 0;// torque / 2;
                //Do obsługi przy obrotach silnika
                ClutchIsSlipping = true;
            }

            //Mnożnik w którą stronę skręcamy
            if (FL_WHEEL.wheelCollider.rpm > FR_WHEEL.wheelCollider.rpm)
            {

                FL_torque = torque * PowerToFront;
                FR_torque = torque * PowerToFront * FrontDiffSlip;
            }
            else
            {
                FL_torque = torque * PowerToFront * FrontDiffSlip;
                FR_torque = torque * PowerToFront;
            }

            if (RL_WHEEL.wheelCollider.rpm > RR_WHEEL.wheelCollider.rpm)
            {
                RL_torque = torque * PowerToRear;
                RR_torque = torque * PowerToRear * RearDiffSlip;
            }
            else
            {
                RL_torque = torque * PowerToRear * RearDiffSlip;
                RR_torque = torque * PowerToRear;
            }

            //BrokenAxles------------------------------------------------------
            if (FL_BrokenAxle)
            {
                FL_torque = 0;
                FR_torque = torque * PowerToFront * FrontDiffSlip;
            }
            else if (FR_BrokenAxle)
            {
                FR_torque = 0;
                FL_torque = torque * PowerToFront * FrontDiffSlip;
            }
            else
            {
                FL_torque = 0;
                FR_torque = 0;
            }

            if (RL_BrokenAxle)
            {
                RL_torque = 0;
                RR_torque = torque * PowerToRear * RearDiffSlip;
            }
            else if (RR_BrokenAxle)
            {
                RR_torque = 0;
                RL_torque = torque * PowerToRear * RearDiffSlip;
            }
            else
            {
                RL_torque = 0;
                RR_torque = 0;
            }

            //TorqueAppying------------------------------------------------------
            FL_WHEEL.wheelCollider.motorTorque = FL_torque;
            FR_WHEEL.wheelCollider.motorTorque = FR_torque;
            RL_WHEEL.wheelCollider.motorTorque = RL_torque;
            RR_WHEEL.wheelCollider.motorTorque = RR_torque;
        }
        public void AdjustSuspension()
        {
            //Ustawienie pozycji kół
            FR_WHEEL.wheelObject.transform.localPosition.Set(FrontWidth / 2, 0, WheelBaseLength / 2);
            FL_WHEEL.wheelObject.transform.localPosition.Set(-FrontWidth / 2, 0, WheelBaseLength / 2);

            RR_WHEEL.wheelObject.transform.localPosition.Set(RearWidth / 2, 0, -WheelBaseLength / 2);
            RL_WHEEL.wheelObject.transform.localPosition.Set(-RearWidth / 2, 0, -WheelBaseLength / 2);
            //Ustawienie rotacji kół
            FR_WHEEL.wheelObject.transform.localEulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
            FL_WHEEL.wheelObject.transform.localEulerAngles = new Vector3(270.0f, 0.0f, 0.0f);

            RR_WHEEL.wheelObject.transform.localEulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
            RL_WHEEL.wheelObject.transform.localEulerAngles = new Vector3(270.0f, 0.0f, 0.0f);
            //Załadowanie modelu felg ????

            //Cokolwiek oprócz wyglądu koła ?
            //i jakoś przekazać żeby skrypt od opon widział, ze koła są pochylone (w negatywie/pozytywie)
            //...
            //Tutaj chyba można pominąć fakt, że koła się kręcą. 
            //To się wrzuci w  jakimś miniSkrypciku(bo jeszcze nie wiem czy rotacja czy animacje...) bezpośrednio na koło podczas tworzenia i on będzie sobie żył bez komunikacji z nikim.
            // ++++ Taki sam miniSkrypcik sprawdzający  siłę hamowania i liczący sobie  temperatury i wyświtlający grzanie tarczy i  zmniejszający siłę hamowania przy gotującym się płynie hamulcowym + wysyłajacy  jakieś alerty do reszty żeby pedał bardziej wpadał, albo jakieś powiadomienia itd...
            //...
            //Wysokość koła
            //Camber
            //Skręcenie
            // Zaraz  a Camber z w zależności od skrętu
            // wyliczyć z 2 AnimationCurve
            // Mając już wyliczone lepiej przekazać ten kąt jakoś do tego skryptu z oponą....
        }
        public void Steer(float anglePercentage)
        {
            FL_WHEEL.wheelCollider.steerAngle = anglePercentage;
            FR_WHEEL.wheelCollider.steerAngle = anglePercentage;
        }
    }

    public float Throttle;
    public float Torque;
    public float WheelsRPM;
    public float EngineRPM;
    public float[] GearRatio;
    public int CurrentGear;
    public AnimationCurve RPM_Torque;
    public WheelBase MyWheelBase;
    public static float Clutch;
    public bool ClutchPressed;
    public static bool ClutchIsSlipping;
    public float Steering;
    public float LastSteering;

    // Use this for initialization
    void Start()
    {
        //Creating your Vechicle (Load section)
        //Download model data  (or model)
        //Build Model,parts,paint...
        //Download engine,wheelbase data...
        //DownloadData();
        //Build Wheelbase
        MyWheelBase = new WheelBase();
        MyWheelBase.AdjustSuspension();

        //
        GearRatio = new float[] { -3.382f, 3.321f, 1.902f, 1.308f, 1f, 0.759f };
    }

    // Update is called once per frame
    void Update()
    {
        //Obsługa
        //Wciskasz gaz to jedzie
        Throttle = Input.GetAxis("Throttle");
        Clutch = Input.GetAxis("Clutch");
        Steering = Input.GetAxis("Steering");
        //Torque current rpm (curve value) current gear
        //Wykres torque dla obrotów
        //Zakres obrotów dla biegu i przełozenie
        // Throttle =0.5 wtedy dajemy 0.5  * Max torque dla tych obrotów
        //Torque = Throttle * wykres(rpmSilnika);
        //rpmSilnika  na podstawie rpmKol i  przełożenia aktualnego biegu
        //Odcinanie  zapłonu  /  wybuch silnika(bardziej skomplikowane...)
        //Gaśnięcie przy  zbyt niskich obrotach (samo się zrobi =>)(+ brak torque na niskich obrotach wykresu....)
        //Torque = differential_type = wheels

        //WheelsRPM zwykła średnia dla napędowych(pominiemy w ten sposób różnice między dyferencjałami różnego typu)

        //EngineRPM
        //Na podstawie WheelsRPM  * GearRatio[CurrentGear];
        /*
SR20DET (5 speed)
1st: 3.321
2nd: 1.902
3rd: 1.308
4th: 1.000
5th: 0.759
Reverse : 3.382
Final Drive: 4.083 (S13)
         */
        if (MyWheelBase != null && GearRatio != null && GearRatio.Length > 0)
        {
            if (Clutch > 0.9)
            {
                ClutchPressed = true;
            }
            else
            {
                ClutchPressed = false;
            }

            if (!ClutchPressed || ClutchIsSlipping)
            {
                WheelsRPM = MyWheelBase.AVGRPM();
                EngineRPM = WheelsRPM * GearRatio[CurrentGear];
            }
            else
            {
                EngineRPM = Throttle * 1000 * Time.deltaTime;
            }

            Torque = Throttle * RPM_Torque.Evaluate(EngineRPM);
            //Torque to Wheels trought differential
            MyWheelBase.ApplyTorque(Torque);
            if (LastSteering != Steering)
            {
                MyWheelBase.Steer(Steering);
                LastSteering = Steering;
            }

            MyWheelBase.AdjustSuspension();
        }
    }

    /*
    void CreateWheelBase() {
        //create wheels and position them inside me
    }    

    void CreateYourWheels() {
        //Stworzenie
        //Ustawienie parametrów jak np grip (pobieranych)

        //ustawienie w przestrzeni
    }*/
    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 200, 20), "Throttle: " + Throttle);
        GUI.Label(new Rect(0, 20, 200, 20), "Clutch: " + Clutch);
        GUI.Label(new Rect(0, 40, 200, 20), "Steering: " + Steering);
        GUI.Label(new Rect(0, 60, 200, 20), "WheelsRPM: " + WheelsRPM);
        GUI.Label(new Rect(0, 80, 200, 20), "EngineRPM: " + EngineRPM);
        GUI.Label(new Rect(0, 100, 200, 20), "CurrentGear: " + CurrentGear);
        if (GearRatio != null && GearRatio.Length > 0)
        {
            GUI.Label(new Rect(0, 120, 200, 20), "GearRatio[CurrentGear]: " + GearRatio[CurrentGear]);
        }
        GUI.Label(new Rect(0, 140, 200, 20), "Torque: " + Torque);
        GUI.Label(new Rect(0, 160, 200, 20), "MyWheelBase: " + (MyWheelBase == null ? "null0" : "null1"));
        if (MyWheelBase != null)
        {
            GUI.Label(new Rect(0, 180, 200, 20), "FR_WHEEL: " + (MyWheelBase.FR_WHEEL == null ? "null0" : "null1"));
            if (MyWheelBase.FR_WHEEL != null)
            {
                GUI.Label(new Rect(0, 200, 200, 20), "wheelObject: " + (MyWheelBase.FR_WHEEL.wheelObject == null ? "null0" : "null1"));
            }
        }
    }
}
