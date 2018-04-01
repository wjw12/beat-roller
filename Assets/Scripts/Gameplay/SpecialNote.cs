using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialNote : BaseNote {
    public float startTime, arriveTime;

    float t3 = 0.25f;

    float currTime, sinx, cosx;
    Animator anim;
    bool hasStopped = false;


	// Use this for initialization
	void Start () {
        currTime = startTime;
        //distanceFromOrigin = 0f;
        anim = GetComponent<Animator>();
        angle_rad = angle_deg * Mathf.Deg2Rad;
        sinx = Mathf.Sin(angle_rad);
        cosx = Mathf.Cos(angle_rad);
    }
	
	// Update is called once per frame
	void Update () {
		if (!hasStopped)
        {
            transform.Translate(new Vector2(velocity * Time.deltaTime * cosx,
                velocity * Time.deltaTime * sinx));
            //distanceFromOrigin += velocity * Time.deltaTime;

            currTime += Time.deltaTime;
            if (currTime > arriveTime + t3)
            {
                Miss();
            }
        }
	}


    public override void TouchBegin(float t)
    {
        // this function is called when the special note is touched by SnapPoint
        if (!hasStopped)
        {
            hasStopped = true;
            Perfect();
        }
    }

    void Miss()
    {
        if (!hasStopped)
        {
            hasStopped = true;
            Fail();
        }
    }

    void Perfect()
    {
        Success();
    }

    void Success()
    {

        // animation
        anim.SetBool("Expand", true);

        StartCoroutine(DieAfter(t3));
    }

    void Fail()
    {
        hasStopped = true;

        // combo restart

        // fade animation
        anim.SetBool("Fade", true);

        StartCoroutine(DieAfter(t3));
    }

    IEnumerator DieAfter(float t)
    {
        NotifyDeath();
        yield return new WaitForSeconds(t);
        DestroyImmediate(gameObject);
    }

    void NotifyDeath()
    {
        FindObjectOfType<TouchRing>().NoteDie(this);
    }
}
