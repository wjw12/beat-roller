using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showpath : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Text t = GetComponent<Text>();
        t.text = Application.persistentDataPath;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
