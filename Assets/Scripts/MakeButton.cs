using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MakeButton : MonoBehaviour
{
    public GameObject originObject;
    public Transform parentTransForm;
    public Object[] music;
    public Object[] list;
    public int n;

    public GameObject InstantiateList()
    {
        return GameObject.Instantiate(originObject, parentTransForm);
    }

    public void SetName(string name)
    {
        Text text = GameObject.Find("Name").GetComponent<Text>();
        text.GetComponent<Text>().text = name;
    }

    public void SetWriter(string name)
    {
        Text text = GameObject.Find("Writer").GetComponent<Text>();
        string[] strs = File.ReadAllLines("Assets/Resources/LocalMusic/" + name + ".txt");
        text.GetComponent<Text>().text = strs[0];
    }

    public void SetTime(string name)
    {
        Text text = GameObject.Find("Time").GetComponent<Text>();
        string[] strs = File.ReadAllLines("Assets/Resources/LocalMusic/" + name + ".txt");
        text.GetComponent<Text>().text = strs[1];
    }

    public void SetDegree(string name)
    {
        Text text = GameObject.Find("Degree").GetComponent<Text>();
        string[] strs = File.ReadAllLines("Assets/Resources/LocalMusic/" + name + ".txt");
        text.GetComponent<Text>().text = strs[2];
    }

    public void SetMusic(string name)
    {
        GameObject obj = GameObject.Find("Music");
        obj.GetComponent<AudioSource>().clip = (AudioClip)Resources.Load("LocalMusic/" + name, typeof(AudioClip));
        obj.GetComponent<AudioSource>().Play();
    }

    public void SetImage(string name)
    {
        GameObject obj = GameObject.Find("Image");
        obj.GetComponent<Image>().sprite = (Sprite)Resources.Load("LocalMusic/" + name, typeof(Sprite));
    }

    public void SetGameStarter(string name)
    {
        Debug.Log(name + " selected!");
        //prepare to switch scene
    }

    public void Make()
    {
        for (int i = 0; i < n&&music[i].name!="background"; i++)
        {
            Text text = GameObject.Find("Prefeb/Text").GetComponent<Text>();
            text.GetComponent<Text>().text = music[i].name;
            string name = music[i].name;
            GameObject obj=InstantiateList();
            obj.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                SetName(name);
                SetWriter(name);
                SetTime(name);
                SetDegree(name);
                SetMusic(name);
                SetImage(name);
                SetGameStarter(name);
            }
                );
        }
    }

    public void Start()
    {
        music = Resources.LoadAll("LocalMusic", typeof(AudioClip));
        list = Resources.LoadAll("LocalMusic", typeof(TextAsset));
        n = music.Length;
        Make();
    }
}