using UnityEngine;
using System.Collections;

public class MyVelocity : MonoBehaviour {

	// Use this for initialization
	public GUIStyle Tachometer;
	public float V= 0.000f;
	public float rpmy= 0.000f;
	public float CG= 0.000f;
	public float DolnaGranica;
	public float GornaGranica;
	public float WielkoscSektora = 22.000f;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		V = gameObject.GetComponent<Rigidbody>().velocity.magnitude*3.6014674269f;
		DolnaGranica = gameObject.GetComponent<CarEngine>().MinObroty;
		GornaGranica = gameObject.GetComponent<CarEngine>().MaxObroty;
		rpmy = gameObject.GetComponent<CarEngine>().RPM;
		//CG = gameObject.GetComponent<UnityStandardAssets.Vehicles.Car.CarController>().m_GearNum;

		rotAngle = -110.0f+(220.0f*(rpmy/10000.0f));
		rotAngle2 = -110+(-22*4)+(220*(GornaGranica/10000));
		rotAngle3 = -110+(-22*5)+(220*(GornaGranica/10000));
	}
	public float rotAngle = 0;
	public float rotAngle2 = 0;
	public float rotAngle3 = 0;
	private Vector2 pivotPoint;
	public Texture2D t1;
	public Texture2D t2;
	public Texture2D t3;
	public Texture2D t4;
	public Texture2D t5;
	public Texture2D t6;
	void OnGUI(){
		float Velo;
		Velo = Mathf.Round(V * 1);
		GUI.Label (new Rect(Screen.width-400+50, Screen.height-400+100, 400, 400),t2);
		GUI.Label (new Rect(Screen.width-400+50, Screen.height-400+100, 400, 400),t3);
		pivotPoint = new Vector2(Screen.width-200+50, Screen.height-200+100);
		GUI.Label (new Rect(Screen.width-200, Screen.height-70, 100, 40),""+Velo,Tachometer);
		GUIUtility.RotateAroundPivot(rotAngle, pivotPoint);
		GUI.Label (new Rect(Screen.width-400+50, Screen.height-400+100, 400, 400),t4);
		GUIUtility.RotateAroundPivot(rotAngle*-1, pivotPoint);
		GUIUtility.RotateAroundPivot(rotAngle2, pivotPoint);
		GUI.Label (new Rect(Screen.width-400+50, Screen.height-400+100, 400, 400),t5);
		GUIUtility.RotateAroundPivot(rotAngle2*-1, pivotPoint);
		GUIUtility.RotateAroundPivot(rotAngle3, pivotPoint);
		GUI.Label (new Rect(Screen.width-400+50, Screen.height-400+100, 400, 400),t6);

	}
}
