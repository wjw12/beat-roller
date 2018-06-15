using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

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

   

    public void AddMusic(MusicListItem musicListItem)
    {
        items.Add(musicListItem);
    }

    public void AddMusic(List<MusicListItem> musicList)
    {
        foreach (var i in musicList)
        {
            AddMusic(i);
        }
    }

    public void Clear()
    {
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            DestroyImmediate(parentTransform.GetChild(0).gameObject);
        }
        items = new List<MusicListItem>();
    }

    public void Refresh()
    {
        for (int i = 0; i < items.Count; i++)
        {
            //float x = parentTransform.GetComponent<RectTransform>().sizeDelta.x;
            //float y = parentTransform.GetComponent<RectTransform>().sizeDelta.y;
            //parentTransform.GetComponent<RectTransform>().sizeDelta = new Vector2(x, y + 40);

            GameObject button = Instantiate(buttonPrefab, parentTransform);
            Text text = button.transform.Find("Music Name").GetComponent<Text>();
            text.text = items[i].Name;

            text = button.transform.Find("Musician").GetComponent<Text>();
            text.text = items[i].Musician;

            string name = items[i].Name;
            string musician = items[i].Musician;
            string musicLink = items[i].MusicLink;
            string imgLink = items[i].ImgLink;
            LevelData[] levels = items[i].leveldata;
            button.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                dataView.SetName(name);
                dataView.SetMusician(musician);
                dataView.SetMusicLink(musicLink);
                dataView.SetImageLink(imgLink);
                dataView.SetLevels(levels);
            }
                );
        }
    }
}
