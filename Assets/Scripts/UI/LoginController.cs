using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginController : MonoBehaviour {
    public GameObject LoggedInPage, NotLoggedInPage;
    public InputField usernameField, passwdField, repPasswdField;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SignOut()
    {
        AccountManager.Instance.SignOut();
    }

    public void Login()
    {
        // get input field texts
    }

    public void SignUp()
    {

    }
}
