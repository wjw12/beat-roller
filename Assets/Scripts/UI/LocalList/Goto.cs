using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Goto : MonoBehaviour
{
    public void SwitchScene()
    {
        Debug.Log(GameObject.Find("Canvas/Name").GetComponent<Text>().text+" started!");
        //switch scene
    }
}