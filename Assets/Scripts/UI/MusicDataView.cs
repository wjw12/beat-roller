using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    virtual public void SetName(string name) { }
    virtual public void SetMusician(string musician) { }
    virtual public void SetImageLink(string link) { }
    virtual public void SetMusicLink(string link) { }

    virtual public void SetLevels(LevelData[] levels) {
        // download best scores data

        // if not null, display best scores

        // loop
        GameObject go = Instantiate(Resources.Load("Prefabs/UI/Best Score") as GameObject, bestScoresContent.transform);
        //go.transform.Find("Score").GetComponent<Text>().text = ;
        //go.transform.Find("Player").GetComponent<Text>().text = ;
        //go.transform.Find("Rank").GetComponent<Text>().text = ;
    }
}
