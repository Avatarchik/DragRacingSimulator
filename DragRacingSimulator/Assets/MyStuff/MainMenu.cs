using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MySql.Data.MySqlClient;
using System.Data.Odbc;
using System.Data;
using UnityEngine.Networking;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    public bool PauseMenu = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            PauseMenu = !PauseMenu;
        }
    }

    string g_email = "email@email.email";
    string g_password = "password123";
    string g_password_conf = "password123";
    string g_nick = "nick222";
    public bool LoggedIn = false;
    bool Register = false;
    Texture2D LogInBackground;
    string Alert = "";
    private void OnGUI()
    {
        if (PauseMenu)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), LogInBackground);
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 15, 200, 30), "Wyjdź"))
            {
                Application.Quit();
            }
        }
        else
        {
            if (!LoggedIn)
            {
                if (LogInBackground == null)
                {
                    LogInBackground = new Texture2D(1, 1);
                    LogInBackground.SetPixel(0, 0, new Color(0, 0, 0, 0.7f));
                    LogInBackground.Apply();
                }
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), LogInBackground);

                if (!Register)
                {

                    GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 160, 200, 30), "Logowanie");
                    GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 130, 200, 30), "Email:");
                    g_email = GUI.TextField(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 30), g_email);
                    GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 70, 200, 30), "Hasło:");
                    g_password = GUI.PasswordField(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 40, 200, 30), g_password, '*');

                    if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 10, 200, 30), "Zaloguj"))
                    {
                        StartCoroutine(GetUserData());
                    }

                    if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 20, 200, 30), "Zarejestruj"))
                    {
                        Register = true;
                    }
                }
                else
                {
                    GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 160, 200, 30), "Rejestracja");
                    GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 130, 200, 30), "Email:");
                    g_email = GUI.TextField(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 30), g_email);
                    GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 70, 200, 30), "Hasło:");
                    g_password = GUI.PasswordField(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 40, 200, 30), g_password, '*');
                    GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 10, 200, 30), "Powtórz hasło:");
                    g_password_conf = GUI.PasswordField(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 20, 200, 30), g_password_conf, '*');
                    GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 50, 200, 30), "Nick:");
                    g_nick = GUI.TextField(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 80, 200, 30), g_nick);

                    if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 110, 200, 30), "Zarejestruj"))
                    {
                        if (g_password == g_password_conf)
                        {
                            StartCoroutine(RegisterNewUser());
                        }
                        else
                        {
                            Alert = "Hasła nie są identyczne!";
                        }
                    }

                    if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 140, 200, 30), "Wróć"))
                    {
                        Register = false;
                    }
                }
            }

            GUI.Label(new Rect(Screen.width / 2 - 100, 10, Screen.width, 30), Alert);
        }
    }

    public IEnumerator GetUserData()
    {
        //http://dragracingsimulator.4pgs.ugu.pl/AddTrackRecord.php?USER_ID=0&GEAR_CHANGES=test&TIME=32.22
        string login_url = "http://dragracingsimulator.4pgs.ugu.pl/Login.php?Login=" + g_email + "& Password=" + g_password;
        var download = UnityWebRequest.Get(login_url);
        yield return download.SendWebRequest();

        if (download.isNetworkError || download.isHttpError)
        {
            print("Error: " + download.error);
        }
        else
        {
            Debug.Log("Downloaded sth");
            foreach (string split1 in download.downloadHandler.text.Split('/'))
            {
                if (split1 == "ID=NONE")
                {
                    Alert = "Nie znaleziono!";
                    break;
                }

                string[] split2 = split1.Split('=');
                if (split2[0] == "NICK")
                {
                    Alert = "";
                    Debug.Log("Hello " + split2[1]); 
                    LoggedIn = true;
                    Register = false;
                }

            }
        }
    }

    public IEnumerator RegisterNewUser()
    {
        //http://dragracingsimulator.4pgs.ugu.pl/Register.php?EMAIL=email@email2.email&PASSWORD=password123&NICK=NICK123&USER_TYPE=0
        string url = "http://dragracingsimulator.4pgs.ugu.pl/Register.php?EMAIL=" + g_email + "& PASSWORD=" + g_password + "& NICK=" + g_nick + "& USER_TYPE=0";
        var download = UnityWebRequest.Get(url);
        yield return download.SendWebRequest();

        if (download.isNetworkError || download.isHttpError)
        {
            print("Error: " + download.error);
        }
        else
        {
            Debug.Log("Downloaded sth");
            foreach (string split1 in download.downloadHandler.text.Split('/'))
            {
                if (split1 == "EMAIL_TAKEN")
                {
                    Alert = "Email jest zajęty!";
                    break;
                }
                else if (split1 == "NICK_TAKEN")
                {
                    Alert = "Nick jest zajęty!";
                    break;
                }
                else
                {
                    string[] split2 = split1.Split('=');
                    if (split2[0] == "ID")
                    {
                        StartCoroutine(GetUserData());
                    }
                }
            }
        }
    }

    public void Meta() {
        Debug.Log("Meta!!!");
    }
}
