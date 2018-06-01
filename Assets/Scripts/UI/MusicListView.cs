using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicListItem
{
    public string Name { get; set; }
    public string Musician { get; set; }
    public string ImgLink { get; set; } // can be either http link or path
    public string MusicLink { get; set; }
    public LevelData[] leveldata { get; set; }
}

public class LevelData
{
    public int Difficulty { get; set; }
    public string MapLink { get; set; } // http link or path
}//

public class MusicListView : MonoBehaviour {
    public GameObject buttonPrefab;
    public Transform parentTransform;

    MusicDataView dataView;
    List<MusicListItem> items;
     

	// Use this for initialization
	void Awake () {
        dataView = FindObjectOfType<MusicDataView>();
        items = new List<MusicListItem>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddMusic(string name, string musician, string imgPath, string musicPath, LevelData[] levels)
    {
        // to be implemented

        // items.Add(.....)
        
    }

    public void AddMusic(List<MusicListItem> musicList)
    {

    }

    public void Clear()
    {
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            DestroyImmediate(parentTransform.GetChild(0).gameObject);
        }
    }

    public void Refresh()
    {
        Clear();
        for (int i = 0; i < items.Count; i++)
        {
            GameObject button = Instantiate(buttonPrefab, parentTransform);
            Text text = button.transform.Find("Music Name").GetComponent<Text>();
            text.text = items[i].Name;

            text = button.transform.Find("Musician").GetComponent<Text>();
            text.text = items[i].Musician;
            
            button.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                dataView.SetName(items[i].Name);
                dataView.SetMusician(items[i].Musician);
                dataView.SetMusicLink(items[i].MusicLink);
                dataView.SetImageLink(items[i].ImgLink);
                dataView.SetLevels(items[i].leveldata);
            }
                );
        }
    }
}
