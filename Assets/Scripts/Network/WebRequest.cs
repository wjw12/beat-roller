using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;
//using Newtonsoft.Json;

public class MyWebRequest{
    string url;

    public MyWebRequest()
    {
        url = "http://162.105.86.75:39080";
    }

    public MyWebRequest(string serverUrl)
    {
        url = serverUrl;
    }

    public string GetBestScores(string musicName, int difficulty)
    {
        return null;
    }

    public string Search(string text)
    {
        string result = string.Empty;
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        req.Method = "POST";
        req.TimeOut = "1000";
        req.ContentType = "application/x-www-form-urlencoded";
        byte[] PostString = Encoding.UTF8.GetBytes(QuerySong);
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
        queryString = "username=" + username + "passwd=" + passwd + "musicName=" + musicName + "difficulty=" + difficulty.ToString() + "rank=" + rank + "score=" + score.ToString();
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        req.Method = "POST";
        req.TimeOut = "600";
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

    public Byte[] DownloadFile(string url)
    {
        return null;
    }

}


public class JsonUtils
{
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
        musicinfo = JsonConvert.DeserializeObject<List<MusicListItem>>(json);
        return musicinfo;
    }
}

// -------------- the following does not work --------------

/*
public string TestSearch(string keyword)
{
    HttpWebRequest req = WebRequest.Create(urlStr) as HttpWebRequest;
    var data = Encoding.Unicode.GetBytes(keyword);
    req.Method = "POST";
    req.ContentType = "application/x-www-form-urlencoded";
    req.ContentLength = data.Length;

    using (var stream = req.GetRequestStream()) stream.Write(data, 0, data.Length);

    var response = req.GetResponse();

    var responseStr = new StreamReader(response.GetResponseStream()).ReadToEnd();

    Debug.Log(responseStr);
    return responseStr;
}

public string TestSignup(string username, string passwd)
{
    //url relativeurl = new url("signup", urlKind.Relative);
    //url fullurl = new url(serverurl, relativeurl);
    HttpWebRequest req = WebRequest.Create("http://162.105.86.75:39080/signup") as HttpWebRequest;
    //var data = Encoding.Unicode.GetBytes("username=" + username + "&password=" + passwd);
    var data = Encoding.Default.GetBytes("username=yzs1000&password=yzsyzsyzs1");
    req.Method = "post";
    req.ContentType = "application/x-www-form-urlencoded";
    req.Headers["cache-control"] = "no-cache";
    req.ContentLength = data.Length;
    req.KeepAlive = false;

    using (var stream = req.GetRequestStream())
    {
        stream.Write(data, 0, data.Length);
        stream.Close();
    }

    //using (var response = (HttpWebResponse) await req.GetRes)
    try
    {
        var response = req.GetResponse() as HttpWebResponse;
        var responseStr = new StreamReader(response.GetResponseStream()).ReadToEnd();
        Debug.Log(responseStr);
        return responseStr;
    }
    catch (WebException e)
    {
        Debug.Log(e.Status.ToString());
        return null;
    }
}
*/