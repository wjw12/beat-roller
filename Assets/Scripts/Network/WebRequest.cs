using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System;
using System.IO;
using Newtonsoft.Json;

public class MyWebRequest{
    public string url;
    public string fileUrl;

    public MyWebRequest()
    {
        url = "http://162.105.86.196:39080";
        fileUrl = "http://yzs-qingstor.pek3b.qingstor.com/beatroller/";
    }

    public MyWebRequest(string serverUrl)
    {
        url = serverUrl;
        fileUrl = "http://yzs-qingstor.pek3b.qingstor.com/beatroller/";
    }

    public string GetBestScores(string musicName, int difficulty)
    {
        // 'musicname=Yuzuki%20-%20you(Vocal)&difficulty=3'

        string result = string.Empty;
        string queryString = string.Empty;
        queryString = "musicname=" + musicName + "&difficulty=" + difficulty.ToString();
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url + "/rank");
        req.Method = "POST";
        req.Timeout = 1000;
        req.ContentType = "application/x-www-form-urlencoded";
        //req.ContentType = "text/plain";
        //req.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
        byte[] PostString = Encoding.UTF8.GetBytes(queryString);
        req.ContentLength = PostString.Length;

        using (Stream reqStream = req.GetRequestStream())
        {
            reqStream.Write(PostString, 0, PostString.Length);
            reqStream.Close();
        }

        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

        if ((int)resp.StatusCode != 200)
        {
            return null;
        }
        Stream stream = resp.GetResponseStream();

        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
        {
            result = reader.ReadToEnd();
            reader.Close();//?
        }


        return result;
    }

    public string Search(string text)
    {
        string result = string.Empty;
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url + "/search");
        req.Method = "POST";
        req.Timeout = 1000;
        //req.ContentType = "application/x-www-form-urlencoded";
        req.ContentType = "text/plain";
        byte[] PostString = Encoding.UTF8.GetBytes(text);
        req.ContentLength = PostString.Length;
        
        using (Stream reqStream = req.GetRequestStream())
        {
            reqStream.Write(PostString, 0, PostString.Length);
            reqStream.Close();
        }

        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
        if ((int)resp.StatusCode != 200)
        {
            return null;
        }

        Stream stream = resp.GetResponseStream();

        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
        {
            result = reader.ReadToEnd();
            reader.Close();
        }

        return result;
    }

    public bool SignUp(string username, string passwd)
    {
        return false;
    }

    public bool AddRecord(string username, string passwd, string musicName, int difficulty, string rank, int score)
    {
        /*
         * 'username=yzs6000&password=yzsyzsyzs&musicname=Yuzuki%20-%20you(Vocal)&difficulty=3&rank=SSS&score=6000'
         */
        string result = string.Empty;
        string queryString = string.Empty;
        queryString = "username=" + username + "&password=" + passwd + "&musicname=" + musicName + "&difficulty=" + difficulty.ToString() + "&rank=" + rank + "&score=" + score.ToString();
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url + "/upload");
        req.Method = "POST";
        req.Timeout = 1000;
        req.ContentType = "application/x-www-form-urlencoded";
        byte[] PostString = Encoding.UTF8.GetBytes(queryString);
        req.ContentLength = PostString.Length;

        using (Stream reqStream = req.GetRequestStream())
        {
            reqStream.Write(PostString, 0, PostString.Length);
            reqStream.Close();
        }

        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

        if ((int)resp.StatusCode != 200)
        {
            return false;
        }
        Stream stream = resp.GetResponseStream();

        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
        {
            result = reader.ReadToEnd();
            reader.Close();//?
        }
        

        return true;
    }

    public void DownloadFile(string relativeUrl, string destination, DownloadDataCompletedEventHandler completedEventHandler)
    {
        using (WebClient wc = new WebClient())
        {
            wc.DownloadDataCompleted += completedEventHandler;
            wc.DownloadFileAsync(new Uri(fileUrl + relativeUrl), destination);
        }
    }

}


public class JsonUtils
{
    public class SearchResult
    {
        public List<MusicListItem> results;
    }

    public static List<BestScores> ParseBestScores(string json)
    {
        List<BestScores> scoreinfo;
        //scoreinfo = JsonConvert.DeserializeObject<List<BestScores>>(JasonScore);
        //if (scoreinfo[0].Player == '0') return 0;
        //return scoreinfo;
        return null;
    }

    public static List<MusicListItem> ParseSearchResult(string json)
    {
        List<MusicListItem> musicinfo;
        SearchResult res = JsonConvert.DeserializeObject<SearchResult>(json);
        //musicinfo = JsonConvert.DeserializeObject<List<MusicListItem>>(json);
        musicinfo = res.results;
        return musicinfo;
    }
}
