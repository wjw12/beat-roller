using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;

public class DownloadButton : MonoBehaviour {
    public GameObject downloadingIndicator;
    OnlineDataView view;

	// Use this for initialization
	void Start () {
        downloadingIndicator.SetActive(false);
        view = FindObjectOfType<OnlineDataView>();
        GetComponent<Button>().onClick.AddListener(view.DownloadAll);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
