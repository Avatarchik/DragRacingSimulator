using UnityEngine;
using System.Collections;

public class CarBrakes : MonoBehaviour {


	public int SilaHamowania4Kola;
	public int SilaHamowaniaReczny;
	public WheelCollider Kp1;
	public WheelCollider Kp2;
	public WheelCollider Kt1;
	public WheelCollider Kt2;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Kp1==null||Kp2==null||Kt1==null||Kt2==null){
			Kp1 = gameObject.GetComponent<CarTransmission> ().K1p;
			Kp2 = gameObject.GetComponent<CarTransmission> ().K2p;
			Kt1 = gameObject.GetComponent<CarTransmission> ().K1;
			Kt2 = gameObject.GetComponent<CarTransmission> ().K2;
		}
		Kt1.brakeTorque = Kt2.brakeTorque = Input.GetAxis ("Brakes") * 0.25f * SilaHamowania4Kola;
		Kp1.brakeTorque = Kp2.brakeTorque = Input.GetAxis ("Brakes") * 0.75f * SilaHamowania4Kola;
		Kt1.brakeTorque = Kt2.brakeTorque = Input.GetAxis ("HandBrake") * SilaHamowaniaReczny;
	}
}
