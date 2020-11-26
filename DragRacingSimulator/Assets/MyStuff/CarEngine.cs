using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class CarEngine : MonoBehaviour
{

	// Use this for initialization
	public bool Klacz2 = false;
	public float RPM;
	public AnimationCurve TorqueCurve;
	public float OporNaSamymSilniku;
	public float AktualnyOporNaKolach;
	public int MaxObroty;
	public int MinObroty;
	public int MaxPredkosc;
	public int MaxHP;
	public GameObject ALS;
	public GameObject wydech;
	public float OstatniStrzalChwila;
	public float OstatniStrzal;
	public float OMomentTemu;
	public float ObrotyMomentTemu;
	public bool ALSTurnedOn = false;
	public float Odciecie = 1;
	void Start()
	{
	}

	// Update is called once per frame
	IEnumerator Odcinam()
	{
		Odciecie = 0.01f;
		yield return new WaitForSeconds(0.01f);
		if (RPM < MaxObroty)
		{
			Odciecie = 1f;
		}
	}

	public float AdditionalParameter = 5;//poniewaz bez tego maksymalna prędkosc przy 600KM to 85
	public float AdditionalParameter2 = 5;//poniewaz bez tego maksymalna prędkosc przy 600KM to 85

	void Update()
	{
		if (!GameObject.Find("Main Camera").GetComponent<MainMenu>().PauseMenu && GameObject.Find("Main Camera").GetComponent<MainMenu>().LoggedIn)
		{
			if (RPM > MaxObroty - 10)
			{
				StartCoroutine(Odcinam());
			}
			/*if(gameObject.GetComponent<MyCar>().Mnoznik>1.0f){
				ALSTurnedOn = true;
			}*/

			if (ALSTurnedOn == true)
			{
				OstatniStrzalChwila = Random.Range(0.05f, 0.5f);
				if (OMomentTemu < Time.time + 0.1f)
				{
					OMomentTemu = Time.time;
					if (ObrotyMomentTemu > RPM)
					{
						if (OstatniStrzal + OstatniStrzalChwila < Time.time)
						{
							OstatniStrzal = Time.time;
							GameObject Bum = (GameObject)Instantiate(ALS, wydech.transform.position, transform.rotation);
							Bum.transform.parent = gameObject.transform;
						}
					}
					ObrotyMomentTemu = RPM;
				}
			}

			if (RPM <= MinObroty)
			{
				Klacz2 = true;
			}
			if (Input.GetAxis("Throttle") > 0f)
			{
				Klacz2 = false;
			}

			if (this.GetComponent<CarTransmission>().ClutchEngagement == 0 || Klacz2 == true)
			{
				this.GetComponent<CarTransmission>().TorqueToApply = 0;
			}
			else
			{
				if (RPM <= MinObroty)
				{
					this.GetComponent<CarTransmission>().TorqueToApply = 1000;
				}
				else
				{
					this.GetComponent<CarTransmission>().TorqueToApply = 0;
					float torque = 0;
					if (RPM > 0 && RPM < MaxObroty)
					{
						float RPM_procent = (RPM / MaxObroty);
						torque = /*(*//*IloscKoni*/ /** RPM_procent)*/TorqueCurve.Evaluate(RPM_procent) * Odciecie * AdditionalParameter;
					}
					this.GetComponent<CarTransmission>().TorqueToApply = torque * Input.GetAxis("Throttle");
				}
			}


			//GET RPM
			if (this.GetComponent<CarTransmission>().ClutchEngagement == 0 || Klacz2 == true || this.GetComponent<CarTransmission>().CurrentGear == 1)
			{
				Debug.Log("RPM "+1);
				//float RPM1 = MinObroty + ((MaxObroty - MinObroty) * GetTrottlePosition());
				float RPM1 = Mathf.Lerp(RPM, MinObroty + ((MaxObroty - MinObroty) * GetTrottlePosition()), 1f);
				if (RPM1 >= 0)
				{
					RPM = RPM1;
				}
				else
				{
					RPM = RPM1 * -1;
				}
			}
			else
			{
				Debug.Log("RPM " + 2);
				float RPM1 = this.GetComponent<CarTransmission>().GetDirvetrainRPM() / AdditionalParameter2;
				if (RPM1 >= 0)
				{
					RPM = RPM1;
				}
				else
				{
					RPM = RPM1 * -1;
				}
			}
		}
	}

	float GetTrottlePosition() {
		float input = 0;		
        if (Input.GetAxis("Throttle") > 0) 
		{ 
			input = Input.GetAxis("Throttle"); 
		}
		return input;
	}
}
