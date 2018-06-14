using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebController : MonoBehaviour {
    MusicListView view;

	// Use this for initialization
	void Start () {
        view = FindObjectOfType<MusicListView>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Search(string keyword)
    {
        // send http request

        // get response

        // parse json

        // call view.AddMusic()
    }

}

