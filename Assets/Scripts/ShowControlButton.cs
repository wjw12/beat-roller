using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowControlButton : MonoBehaviour {
    public GameObject obj;
    public GameObject start;
    public GameObject select;
    Button btn;

	// Use this for initialization
	void Start () {
        start.SetActive(false);
        select.SetActive(false);
        btn = obj.GetComponent<Button>();
        btn.onClick.AddListener(delegate ()
        {
            start.SetActive(true);
            select.SetActive(true);
        });
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
