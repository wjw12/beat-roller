using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleNote : BaseNote {

    public float startTime, arriveTime;

    float t1 = 0.08f;
    float t2 = 0.12f;
    float t3 = 0.18f;

    float currTime;
    bool hasStopped = false;
    //bool isFade = false;
    Animator anim;
    float sinx, cosx; // store sin(angle) to reduce calculation

    public override void TouchBegin(float t)
    {
        if (arriveTime - t > 1.5f * t3) return;
        float delta_t = Mathf.Abs(t - arriveTime);
        if (delta_t < t1)
        {
            Perfect();
        }
        else if (delta_t < t2)
        {
            Good();
        }
        else if (delta_t < t3)
        {
            Fair();
        }
        else
            Miss();
        hasStopped = true;
    }


    // Use this for initialization
    void Start () {
        currTime = startTime;
        anim = GetComponent<Animator>();
        angle_rad = angle_deg * Mathf.Deg2Rad;
        sinx = Mathf.Sin(angle_rad);
        cosx = Mathf.Cos(angle_rad);
    }
	
	// Update is called once per frame
	void Update () {
        // flying

        //if (currTime > arriveTime && !isFade)
        //{
        //    isFade = true;
        //    anim.SetBool("Fade", true);
        //}
        if (!hasStopped && currTime < arriveTime)
        {
            transform.Translate(new Vector2(velocity * Time.deltaTime * cosx,
                velocity * Time.deltaTime * sinx));
        }

        currTime += Time.deltaTime;

        if (currTime > arriveTime + t2)
        {
            Miss();
        }
	}

    void Perfect() {
        Success();
        anim.SetBool("Perfect Expand", true);
        FindObjectOfType<TouchRing>().PerfectHit(angle_rad);
    }

    void Good() {
        Success();
        anim.SetBool("Normal Expand", true);
        FindObjectOfType<TouchRing>().GoodHit(angle_rad);
    }

    void Fair() {
        Success();
        anim.SetBool("Normal Expand", true);
        FindObjectOfType<TouchRing>().FairHit(angle_rad);
    }

    void Miss()
    {
        if (!hasStopped)
        {
            Fail();
            FindObjectOfType<TouchRing>().MissHit(angle_rad);
        }
    }

    void Success()
    {
        hasStopped = true;
        

        StartCoroutine(DieAfter(t3));
    }

    void Fail()
    {

        hasStopped = true;
        anim.SetBool("Fade", true);

        // combo restart
        
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
