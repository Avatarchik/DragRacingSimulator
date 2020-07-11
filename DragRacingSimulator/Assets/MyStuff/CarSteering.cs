using UnityEngine;
using System.Collections;

public class CarSteering : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public WheelCollider S1;
	public WheelCollider S2;
	public int MaxSkret;
	void Update () {
		S1.steerAngle = S2.steerAngle = Input.GetAxis ("Horizontal") * MaxSkret;
	}
}
