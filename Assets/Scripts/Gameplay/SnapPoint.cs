using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPoint : MonoBehaviour {
    public float radius, detachRadius;
    public bool isSnapped = false;

    Animator anim;

    public void SetSnap(bool s)
    {
        isSnapped = s;
        anim.SetBool("IsSnapped", s);
    }

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public IEnumerator DieAfter(float t)
    {
        anim.SetBool("Fade", true);
        yield return new WaitForSeconds(t);
        DestroyImmediate(this.gameObject);
    }
}
