using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebController : MonoBehaviour {
    MusicListView view;
    MyWebRequest req;

	// Use this for initialization
	void Start () {
        view = FindObjectOfType<MusicListView>();
        req = new MyWebRequest();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Search(string keyword)
    {
        view.Clear();
        string res = req.Search(keyword);

        if (res != null)
        {
            List<MusicListItem> musicItems = JsonUtils.ParseSearchResult(res);

            if (musicItems != null && musicItems.Count > 0)
            {
                view.AddMusic(musicItems);
            }
        }

        view.Refresh();
    }

}

