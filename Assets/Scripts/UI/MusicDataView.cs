using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BestScores
{
    public string Player { get; set; }
    public string Rank { get; set; }
    public int Score { get; set; }
}

public class MusicDataView : MonoBehaviour {
    // configure the following in Unity Editor
    public Image musicImage;
    public Text nameField;
    public Text musicianField;
    public GameObject bestScoresContent;

    protected LevelData[] levels;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    virtual public void SetName(string name) {
        nameField.text = name;
    }
    virtual public void SetMusician(string musician) {
        musicianField.text = musician;
    }

    virtual public void SetImageLink(string link) {
        Debug.LogError("Not implemented.");
    }
    virtual public void SetMusicLink(string link) { Debug.LogError("Not implemented."); }

    virtual public void SetLevels(LevelData[] leveldata) {
        int[] aa = new int[] { -1, -1, -1 };
        foreach(LevelData d in leveldata)
        {
            if (d.Difficulty <= 3)
            {
                if (aa[0] < 0)
                {
                    aa[0] = d.Difficulty;
                }
            }
            else if (d.Difficulty <= 7)
            {
                if (aa[1] < 0)
                {
                    aa[1] = d.Difficulty;
                }
            }
            else
            {
                if (aa[2] < 0)
                {
                    aa[2] = d.Difficulty;
                }
            }
        }
        int n = 0;
        foreach (int i in aa)
            if (i >= 0) n++;
        levels = new LevelData[n];

        int j = 0;
        foreach (LevelData d in leveldata)
        {
            if (d.Difficulty <= 3 && d.Difficulty == aa[0])
            {
                levels[j] = d;
                j++;
            }
            else if (d.Difficulty <= 7 && d.Difficulty == aa[1])
            {
                levels[j] = d;
                j++;
            }
            else if (d.Difficulty == aa[2])
            {
                levels[j] = d;
                j++;
            }
        }
        

        // download best scores data

        // if not null, display best scores

        // loop
        GameObject go = Instantiate(Resources.Load("Prefabs/UI/Best Score") as GameObject, bestScoresContent.transform);
        //go.transform.Find("Score").GetComponent<Text>().text = ;
        //go.transform.Find("Player").GetComponent<Text>().text = ;
        //go.transform.Find("Rank").GetComponent<Text>().text = ;
    }
}
