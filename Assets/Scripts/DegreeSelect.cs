using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DegreeSelect : MonoBehaviour {
    public GameObject obj;
    public GameObject txt;
    public static string mode;
    Button btn;

	// Use this for initialization
	void Start () {
        btn = obj.GetComponent<Button>();
        btn.onClick.AddListener(delegate ()
        {
            mode = txt.GetComponent<Text>().text;
            Debug.Log(mode +" mode selected!");
        });
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
