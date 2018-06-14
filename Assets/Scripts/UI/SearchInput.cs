using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchInput : MonoBehaviour {
    public InputField inputField;
    public Button button;

	// Use this for initialization
	void Start () {
        button.onClick.AddListener(Search);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Search()
    {
        FindObjectOfType<WebController>().Search(inputField.text);
    }
}
