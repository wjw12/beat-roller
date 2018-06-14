using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager : Singleton<AccountManager> {
    public bool isLoggedin = false;
    string username;
    string encryptedPasswd;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SignOut()
    {
        isLoggedin = false;
    }

    public void SignUp(string name, string passwd, string repearPasswd)
    {

        // use md5 encryption
    }

    public void Login(string name, string passwd)
    {

    }
}
