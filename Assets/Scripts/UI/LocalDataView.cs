using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalDataView : MusicDataView {
    public Button difficultyButton;
    int difficulty;
    MusicInfo musicInfo;

    // Use this for initialization
    void Start () {
        musicInfo = FindObjectOfType<MusicInfo>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void ChangeDifficulty()
    {

    }

    //override public void SetName(string name) { }
    //override public void SetMusician(string musician) { }

    override public void SetImageLink(string link) {
        // read image file and set musicImage

        // also set musicInfo
    }

    override public void SetMusicLink(string link) {

    }

    override public void SetLevels(LevelData[] data)
    {
        base.SetLevels(data);

        // current difficulty
        difficulty = levels[0].Difficulty;

        // set musicInfo

    }
}
