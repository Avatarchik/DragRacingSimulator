using UnityEngine;
using System.Collections;

public class CarEngine : MonoBehaviour {

	// Use this for initialization
	public bool Klacz=false;
	public bool Klacz2=false;
	public float RPM;
	public int IloscKoni;
	public float OporNaSamymSilniku;
	public float AktualnyOporNaKolach;
	public float NaKola;
	public int MaxObroty;
	public int MinObroty;
	public WheelCollider K1;
	public WheelCollider K2;
	public WheelCollider K1p;
	public WheelCollider K2p;
	public string AWDFWDRWD;
	public int IloscBiegow;
	public int MaxPredkosc;
	public float AktualnePrzelozenie;
	public int AktualnyBieg;
	public GameObject ALS;
	public GameObject wydech;
	public float OstatniStrzalChwila;
	public float OstatniStrzal;
	public float OMomentTemu;
	public float ObrotyMomentTemu;
	public bool ALSTurnedOn=false;
	public float Odciecie=1;
	void Start () {
	}
	
	// Update is called once per frame
	IEnumerator Odcinam(){
		Odciecie = 0.9f;
		yield return new WaitForSeconds (0.1f);
		Odciecie = 1f;
	}
	void Update () {
		if(RPM>MaxObroty-10){
			StartCoroutine (Odcinam());
		}
		/*if(gameObject.GetComponent<MyCar>().Mnoznik>1.0f){
			ALSTurnedOn = true;
		}*/
		if (AktualnyBieg > 0) {
			AktualnePrzelozenie = 1;
		} else if (AktualnyBieg == 0) {
			AktualnePrzelozenie = 0;
		} else {
			AktualnePrzelozenie = -1;
		}
		if(ALSTurnedOn==true){
			OstatniStrzalChwila = Random.Range (0.05f,0.5f);
			if(OMomentTemu<Time.time+0.1f){
				OMomentTemu = Time.time;
				if(ObrotyMomentTemu>RPM){
					if (OstatniStrzal+OstatniStrzalChwila<Time.time) {
						OstatniStrzal = Time.time;
						GameObject Bum = (GameObject)Instantiate (ALS, wydech.transform.position, transform.rotation);
						Bum.transform.parent = gameObject.transform;
					}
				}
				ObrotyMomentTemu = RPM;
			}
		}

		if(Input.GetKeyDown(KeyCode.A)&AktualnyBieg<IloscBiegow){
			AktualnyBieg++;
		}
		if(Input.GetKeyDown(KeyCode.Z)&AktualnyBieg>-1){
			AktualnyBieg--;
		}

		if (Input.GetKeyDown (KeyCode.X)) {
			Klacz = true;
		}
		if (Input.GetKeyUp (KeyCode.X)) {
			Klacz = false;
		}
		RPM =MinObroty+(MaxObroty-MinObroty) * (Input.GetAxis ("Vertical")*Odciecie); 

		if(RPM<=MinObroty){
			Klacz2 = true;
		}
		if (Input.GetAxis("Vertical")>0f) {
			Klacz2 = false;
		}

		if (Klacz == true||Klacz2==true) {
			NaKola= 0;
		} else {
			int AdditionalParameter=5;//poniewaz bez tego maksymalna prędkosc przy 600KM to 85
			NaKola= (IloscKoni * (RPM/MaxObroty)*AdditionalParameter)*AktualnePrzelozenie;
		}
		if (AWDFWDRWD == "RWD") {
			K1p.motorTorque = K2p.motorTorque = 0;
			K1.motorTorque = K2.motorTorque = NaKola;
		} else if(AWDFWDRWD == "FWD") {
			K1p.motorTorque = K2p.motorTorque = NaKola;
			K1.motorTorque = K2.motorTorque = 0;
		}else if(AWDFWDRWD == "AWD"){
			K1p.motorTorque = K2p.motorTorque = NaKola/2;
			K1.motorTorque = K2.motorTorque = NaKola/2;
		}
	}
}
