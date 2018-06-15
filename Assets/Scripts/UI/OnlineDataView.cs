using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.IO;

public class OnlineDataView : MusicDataView {
    public GameObject downloading;
    string musicLink;
    string imgLink;
    int difficulty; // current selected

    MyWebRequest webReq;

    // Use this for initialization
    void Start () {
        webReq = new MyWebRequest();
        downloading.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //override public void SetName(string name) { }
    //override public void SetMusician(string musician) { }

    override public void SetImageLink(string link)
    {
        // download and display image
        imgLink = link;
        string url = webReq.fileUrl + link;
        StartCoroutine(PreviewImage(url));

        //DownloadDataCompletedEventHandler callback = new DownloadDataCompletedEventHandler(ImgDownloadCompleted);
        //webReq.DownloadFile(link, Application.persistentDataPath + "/temp.jpg", callback);
    }

    IEnumerator PreviewImage(string url)
    {
        WWW www = new WWW(url);
        yield return www;
        // System.IO.File.WriteAllBytes(@"C:\SomeFolder\SomeAudioClip.mp3", www.bytes);
        //musicImage.sprite = Sprite.Create(www.texture, musicImage.rectTransform.rect, musicImage.rectTransform.pivot);
        File.WriteAllBytes(Application.persistentDataPath + "/temp.jpg", www.bytes);
        musicImage.sprite = IMG2Sprite.instance.LoadNewSprite(Application.persistentDataPath + "/temp.jpg");
    }
    //private void ImgDownloadCompleted(object sender, DownloadDataCompletedEventArgs e)
    //{
    //    if (e.Cancelled) return;
    //    string path = Application.persistentDataPath + "/temp.jpg";
    //    musicImage.sprite = IMG2Sprite.instance.LoadNewSprite(path);
    //}

    override public void SetMusicLink(string link)
    {
        musicLink = link;
    }

    public void DownloadAll()
    {
        if (musicLink == null || imgLink == null) return;
        // download current selected music file, image file and map files

        StartCoroutine(DownloadEverything());
    }

    IEnumerator DownloadEverything()
    {
        downloading.SetActive(true);
        // image
        string url = webReq.fileUrl + imgLink;
        WWW www = new WWW(url);
        yield return www;
        string baseName = nameField.text + "_" + musicianField.text;
        File.WriteAllBytes(Application.persistentDataPath + "/" + baseName + ".jpg", www.bytes);

        // music
        url = webReq.fileUrl + musicLink;
        www = new WWW(url);
        yield return www;
        File.WriteAllBytes(Application.persistentDataPath + "/" + baseName + ".mp3", www.bytes);

        // level maps
        foreach (LevelData level in levels)
        {
            string levelName = baseName + "_" + level.Difficulty.ToString() + ".csv";
            url = webReq.fileUrl + level.MapLink;
            www = new WWW(url);
            yield return www;
            File.WriteAllBytes(Application.persistentDataPath + "/" + levelName, www.bytes);
        }
        downloading.SetActive(false);
    }

    public void GetBestScores()
    {
        // download and display best scores of current selected difficulty 
    }

    public void ChangeDifficulty()
    {

    }
}
