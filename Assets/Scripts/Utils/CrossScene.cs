using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossScene : MonoBehaviour {
    public static bool isClone;
    public GameObject obj;
    private GameObject cloneObj;

    private void Awake()
    {
        if (!isClone)
        {
            cloneObj = Instantiate(obj) as GameObject;
            isClone = true;
        }
        DontDestroyOnLoad(cloneObj);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
