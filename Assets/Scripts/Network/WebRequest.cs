using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;
//using Newtonsoft.Json;

public class MyWebRequest{
    string uri;

    public MyWebRequest()
    {
        uri = "http://162.105.86.75:39080";
    }

    public MyWebRequest(string serverUrl)
    {
        uri = serverUrl;
    }

    public string GetBestScores(string musicName, int difficulty)
    {
        return null;
    }

    public string Search(string text)
    {
        return null;
    }

    public bool SignUp(string username, string passwd)
    {
        return false;
    }

    public bool AddRecord(string username, string passwd, string musicName, int difficulty, string rank, int score)
    {
        return false;
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
        return null;
    }
}

// -------------- the following does not work --------------

/*
public string TestSearch(string keyword)
{
    HttpWebRequest req = WebRequest.Create(uriStr) as HttpWebRequest;
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
    //Uri relativeUri = new Uri("signup", UriKind.Relative);
    //Uri fullUri = new Uri(serverUri, relativeUri);
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