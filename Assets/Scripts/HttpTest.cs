using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Text;
using System.IO;
using System;

// to run the tests,
// add this script to any gameobject and run the game

public class TestHttp : MonoBehaviour {
	// Use this for initialization
	void Start () {
        MyWebRequest req = new MyWebRequest();

        string s;
        s = req.GetBestScores("Yuzuki%20-%20you(Vocal)", 3);
        Debug.Log(s);

        s = req.Search("you");
        Debug.Log(s);

        // write more tests here
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    
}
