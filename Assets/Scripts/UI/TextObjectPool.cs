using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextObjectPool : MonoBehaviour {

    public GameObject perfectText, goodText, fairText, missText;
    public int pooledCount = 10;

    List<GameObject> perfectList, goodList, fairList, missList;

    // Use this for initialization
    void Start () {
        int i;
        perfectList = new List<GameObject>();
        for (i = 0; i < pooledCount; i++)
        {
            perfectList.Add(Instantiate(perfectText));
            perfectList[i].SetActive(false);
        }

        goodList = new List<GameObject>();
        for (i = 0; i < pooledCount; i++)
        {
            goodList.Add(Instantiate(goodText));
            goodList[i].SetActive(false);
        }

        fairList = new List<GameObject>();
        for (i = 0; i < pooledCount; i++)
        {
            fairList.Add(Instantiate(fairText));
            fairList[i].SetActive(false);
        }

        missList = new List<GameObject>();
        for (i = 0; i < pooledCount; i++)
        {
            missList.Add(Instantiate(missText));
            missList[i].SetActive(false);
        }
    }

    public GameObject GetText(string type)
    {
        int i;
        switch(type)
        {
            case "perfect":
                for (i = 0; i < perfectList.Count; i++)
                    if (!perfectList[i].activeSelf)
                    {
                        perfectList[i].SetActive(true);
                        return perfectList[i];
                    }
                break;
            case "good":
                for (i = 0; i < goodList.Count; i++)
                    if (!goodList[i].activeSelf)
                    {
                        goodList[i].SetActive(true);
                        return goodList[i];
                    }
                break;
            case "fair":
                for (i = 0; i < fairList.Count; i++)
                    if (!fairList[i].activeSelf)
                    {
                        fairList[i].SetActive(true);
                        return fairList[i];
                    }
                break;
            case "miss":
                for (i = 0; i < missList.Count; i++)
                    if (!missList[i].activeSelf)
                    {
                        missList[i].SetActive(true);
                        return missList[i];
                    }
                break;
            default:
                return null;
        }
        return null;
    }

    public void DestroyTextObject(GameObject go)
    {
        go.transform.position = Vector2.zero;
        go.transform.rotation = Quaternion.identity;
        go.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
