using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineDataView : MusicDataView {
    string musicLink;
    string imgLink;
    int difficulty; // current selected

    MyWebRequest webReq;

    // Use this for initialization
    void Start () {
        webReq = new MyWebRequest();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //override public void SetName(string name) { }
    //override public void SetMusician(string musician) { }

    override public void SetImageLink(string link)
    {
        // download and display image
    }

    override public void SetMusicLink(string link)
    {
        musicLink = link;
    }

    public void DownloadAll()
    {
        // download current selected music file, image file and map files
        // this must be async
    }

    public void GetBestScores()
    {
        // download and display best scores of current selected difficulty 
    }

    public void ChangeDifficulty()
    {

    }
}
