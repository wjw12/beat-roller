using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalListController : MonoBehaviour {
    string builtinDataPath;
    string downloadDataPath;

    MusicListView view;

	// Use this for initialization
	void Start () {
        builtinDataPath = Application.dataPath + "/Resources/BuiltinMusic";
        downloadDataPath = Application.persistentDataPath;
        view = FindObjectOfType<MusicListView>();
        // read files in paths

        // prepare data
        // call view.AddMusic(...)

        // finally, 
        view.Refresh();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
