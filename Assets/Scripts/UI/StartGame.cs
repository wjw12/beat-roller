using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {

    bool isLoading = false;
    public GameObject loadingIndicator;

	// Use this for initialization
	void Start () {
        loadingIndicator.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (isLoading && !loadingIndicator.activeSelf)
            loadingIndicator.SetActive(true);
	}

    public void LoadGame()
    {
        isLoading = true;
        StartCoroutine(LoadGameScene());
    }

    public IEnumerator LoadGameScene()
    {
        AsyncOperation async = Application.LoadLevelAsync("Main");
        yield return async;
    }
}
