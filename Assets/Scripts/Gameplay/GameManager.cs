using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public GameObject skipButton;
    public SpriteRenderer backgroundImage;

    TouchRing touchRing;
    float skipTime;
    float currTime = 0f;
    bool hasSkipped = false;
    bool hasStarted = false;
    int musicID;

	// Use this for initialization
	void Start () {
        Application.targetFrameRate = 100;
        touchRing = FindObjectOfType<TouchRing>();
        MusicInfo minfo = FindObjectOfType<MusicInfo>();
        touchRing.LoadMusicScore(minfo.musicPath);
        LoadBackgroundPic(minfo.imagePath);
#if UNITY_EDITOR
        LoadMusic(minfo.musicPath);
#else
        LoadMusicAsync(minfo.musicFileName);
#endif
        skipTime = touchRing.GetFirstNoteTime() * 0.8f;
        Destroy(minfo.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        if (!hasStarted) return;
        currTime += Time.deltaTime;
        if (!hasSkipped && currTime >= skipTime)
        {
            skipButton.SetActive(false);
            hasSkipped = true;
        }
	}

    public void Skip()
    {
        currTime = skipTime;
        ANAMusic.seekTo(musicID, Mathf.RoundToInt(skipTime * 1000f));
        touchRing.SeekTo(currTime);
        hasSkipped = true;
        skipButton.SetActive(false);
    }

    // pause or resume game
    public void Pause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0f;
            ANAMusic.pause(musicID);
        }
        else
        {
            // resume
            Time.timeScale = 1f;
            ANAMusic.play(musicID);
        }
    }


    // stop game and return to Local List scene
    public void Exit()
    {
        ANAMusic.pause(musicID);
        ANAMusic.release(musicID);

        // load scene
    }

    // normal finish
    public void FinishGame()
    {
        ANAMusic.pause(musicID);
        ANAMusic.release(musicID);

        // show statistics

        // upload score
    }

    void LoadMusic(string path)
    {
        musicID = ANAMusic.load(path, true);
        ANAMusic.play(musicID);
        touchRing.SetStart();
    }

    void LoadMusicAsync(string path)
    {
        Action<int> cb;
        cb = delegate (int i) { hasStarted = true; touchRing.SetStart(); ANAMusic.play(musicID); };
        musicID = ANAMusic.load(path, usePersistentDataPath : true, loadAsync: true, loadedCallback : cb);
    }

    void LoadBackgroundPic(string path)
    {
        Sprite sp = IMG2Sprite.instance.LoadNewSprite(Application.persistentDataPath + "/" + path);
        backgroundImage.sprite = sp;

        float cameraHeight = Camera.main.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        Vector2 spriteSize = backgroundImage.sprite.bounds.size;

        Vector2 scale = backgroundImage.transform.localScale;
        if (cameraSize.x >= cameraSize.y)
        { // Landscape (or equal)
            scale *= cameraSize.x / spriteSize.x;
        }
        else
        { // Portrait
            scale *= cameraSize.y / spriteSize.y;
        }

        backgroundImage.transform.position = -cameraSize / 2;
        backgroundImage.transform.localScale = scale;
    }
}
