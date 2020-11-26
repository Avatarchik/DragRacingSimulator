using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    MainMenu MainMenu;
    // Update is called once per frame
    void Update()
    {
        if (MainMenu == null)
        {
            MainMenu=GameObject.Find("Main Camera").GetComponent<MainMenu>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CarScript>() != null) 
        {
            MainMenu.Meta();
        }
    }
}
