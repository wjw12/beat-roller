using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicInfo : MonoBehaviour {
    
    // we shall use like file = new StreamReader(xxxPath);
    public string mapPath;
    public string imagePath;
    public string musicPath;
    public string musicName; // this is music name, not filename
    public int difficulty;

	// Use this for initialization
	void Start () {
        //DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
