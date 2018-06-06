using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleNote : BaseNote {

    public float startTime, arriveTime;

    float t1 = 0.08f;
    float t2 = 0.15f;
    float t3 = 0.25f;

    float currTime;
    bool hasStopped = false;
    Animator anim;
    float sinx, cosx; // store sin(angle) to reduce calculation

    public override void TouchBegin(float t)
    {
        if (arriveTime - t > 2 * t3) return;
        hasStopped = true;
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
        if (!hasStopped)
        {
            transform.Translate(new Vector2(velocity * Time.deltaTime * cosx,
                velocity * Time.deltaTime * sinx));

            currTime += Time.deltaTime;
            if (currTime > arriveTime + t3)
            {
                Miss();
            }
        }
	}

    void Perfect() {
        Success();
    }

    void Good() {
        Success();
    }

    void Fair() {
        Success();
    }

    void Miss()
    {

        Fail();
    }

    void Success()
    {
        hasStopped = true;

        Debug.Log("Success!");
        // combo +1

        // animation
        anim.SetBool("Expand", true);

        StartCoroutine(DieAfter(t3));
    }

    void Fail()
    {

        hasStopped = true;

        // combo restart

        Debug.Log("Fail!");

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
