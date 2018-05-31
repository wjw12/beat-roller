using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNote : BaseNote {
    public float startTime, headArriveTime, tailArriveTime;
    public GameObject head, tail, link;//, petal;
    public ParticleSystem ps;

    float t1 = 0.08f;
    float t2 = 0.12f;
    float t3 = 0.18f;

    float combo_dt = 0.2f;
    float combo_timer = 0f;

    float sinx, cosx; // store sin(angle) to reduce calculation

    float currTime;
    bool isHeadComing = true;
    bool isPressing = false;
    bool isTailShown = false;
    bool isHeadOver = false;
    bool isTailOver = false;
    bool isReadyToFinish = false;

    ChainLightning lightning;

    ScoreRecorder scoreRecorder;

    //Animator petalAnim;
    
    public override void TouchBegin(float t)
    {
        if (headArriveTime - t > 2 * t3) return;
        if (isTailOver || isReadyToFinish) return;

        // touch animation begins
        //OpenPetal();
        ps.gameObject.SetActive(true);
        ps.Play();
        ps.transform.up = Vector3.zero - ps.transform.position;

        if (!isTailOver)
        { 
            isPressing = true;

            //pressing animation

            float delta_t = Mathf.Abs(t - headArriveTime);
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
                MissHead();
        }
    }

    public override void TouchEnd(float t)
    {
        if (isPressing)
        {
            isPressing = false;
            float delta_t = Mathf.Abs(t - tailArriveTime);
            if (delta_t < t3)
            {
                TailSuccess();
            }
            else
            {
                MissTail();
            }

            if (!isTailShown)
            {
                StopCoroutine("CreateTailAfter");
            }

            // animation stops
            //FadePetal();
            ps.Stop();
        }
    }

    /*
    void OpenPetal()
    {
        petal.SetActive(true);
        float radii = FindObjectOfType<TouchRing>().ringRad;
        petal.transform.position = new Vector2(radii * cosx, radii * sinx);
        //Animator anim = petal.GetComponent<Animator>();
        //anim.SetFloat("SpeedMultiplier", (tailArriveTime - headArriveTime) / anim.GetFloat("OriginalDuration"));
        //anim.Play("Petal Open", -1, (currTime - headArriveTime) / (tailArriveTime - headArriveTime));
    }

    void FadePetal()
    {
        petalAnim.speed = 1;
        petalAnim.SetBool("Fade", true);
    }
    */

    // Use this for initialization
    void Start () {
        currTime = startTime;
        angle_rad = angle_deg * Mathf.Deg2Rad;
        sinx = Mathf.Sin(angle_rad);
        cosx = Mathf.Cos(angle_rad);
        tail.SetActive(false);
        lightning = link.GetComponent<ChainLightning>();
        lightning.SetLineStart(0f, 0f);
        lightning.SetLineEnd(0f, 0f);
        head.transform.position = Vector2.zero;
        tail.transform.position = Vector2.zero;
        scoreRecorder = FindObjectOfType<ScoreRecorder>();
        //petalAnim = petal.GetComponent<Animator>();
        
        ps.gameObject.SetActive(false);

        StartCoroutine(CreateTailAfter(tailArriveTime - headArriveTime));
    }
	
	// Update is called once per frame
	void Update () {
        if (head.gameObject != null && ps.gameObject != null)
            ps.transform.position = head.transform.position;

        if (isHeadOver && isTailOver && !isReadyToFinish)
        {
            isReadyToFinish = true;
            StartCoroutine(FinishAfter(t2));
        }

		if (isHeadComing)
        {
            if (currTime <= headArriveTime)
            {
                head.transform.Translate(new Vector2(velocity * Time.deltaTime * cosx,
                    velocity * Time.deltaTime * sinx));
            }
            lightning.SetLineStart(head.transform.position.x, head.transform.position.y);
            if (currTime - headArriveTime > t3)
            {
                MissHead();
            }
        }

        if (isTailShown)
        {
            if (currTime <= tailArriveTime)
            {
                tail.transform.Translate(new Vector2(velocity * Time.deltaTime * cosx,
                    velocity * Time.deltaTime * sinx));
            }
            lightning.SetLineEnd(tail.transform.position.x, tail.transform.position.y);
            if (!isTailOver)
            {
                if (!isPressing && currTime > tailArriveTime - t3)
                {
                    MissTail();
                }
                if (isPressing && currTime > tailArriveTime + t3)
                {
                    isTailOver = true;
                    //Fair();
                }
            }
        }

        if (!isReadyToFinish)
        {
            combo_timer += Time.deltaTime;
            if (combo_timer >= combo_dt)
            {
                combo_timer = 0f;
                if (isPressing) scoreRecorder.IncrementCombo();
                else scoreRecorder.InterruptCombo();
            }
        }
        /*
        if (isPressing)
        {
            // shining effect
            if (petal.activeSelf && !petalAnim.GetBool("Fade"))
            {
                petalAnim.Play("Petal Open", -1, (currTime - headArriveTime) / (tailArriveTime - headArriveTime));
                petalAnim.speed = 0;
            }
        }
        */

        currTime += Time.deltaTime;
	}

    IEnumerator CreateTailAfter(float t)
    {
        yield return new WaitForSeconds(t);

        tail.SetActive(true);
        isTailShown = true;
    }

    void MissHead()
    {
        if (!isHeadOver)
        {
            isHeadOver = true;
            isHeadComing = false;
            head.GetComponent<Animator>().SetBool("Fade", true);
            StartCoroutine(DestroyAfter(head, t3));
            FindObjectOfType<TouchRing>().MissHit(angle_rad);
            scoreRecorder.Miss();
        }
    }

    void MissTail()
    {
        if (isTailShown && !isTailOver)
        {
            isTailShown = false;
            isTailOver = true;
            tail.GetComponent<Animator>().SetBool("Fade", true);
            //FindObjectOfType<TouchRing>().MissHit(angle_rad);
            //FindObjectOfType<ScoreRecorder>().InterruptCombo();
        }
    }

    void TailSuccess()
    {
        //FindObjectOfType<ScoreRecorder>().IncrementCombo();
    }

    void Perfect()
    {
        Success();
        FindObjectOfType<TouchRing>().PerfectHit(angle_rad);
    }

    void Good()
    {
        Success();
        FindObjectOfType<TouchRing>().GoodHit(angle_rad);
    }

    void Fair()
    {
        Success();
        FindObjectOfType<TouchRing>().FairHit(angle_rad);
    }

    void Success()
    {
        if (isHeadOver && !isTailOver)
        {
            isTailOver = true;
            isTailShown = false;
            tail.GetComponent<Animator>().SetBool("Expand", true);
        }

        if (!isHeadOver && head != null)
        {
            isHeadComing = false;
            isHeadOver = true;
            head.GetComponent<Animator>().SetBool("Expand", true);
            StartCoroutine(DestroyAfter(head, t3));
        }
        
    }

    IEnumerator FinishAfter(float t)
    {
        //if (tail != null)
        //    tail.GetComponent<Animator>().SetBool("Fade", true);
        //if (petal.activeSelf)
        //    FadePetal();
        if (ps.gameObject.activeSelf)
            ps.Stop();

        lightning.Fade();

        NotifyDeath();
        yield return new WaitForSeconds(t);
        StopAllCoroutines();
        Destroy(link);
        Destroy(tail);
        //Destroy(petal);
        Destroy(ps.gameObject);
        Destroy(gameObject);
    }

    IEnumerator DestroyAfter(GameObject go, float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(go);
    }

    void NotifyDeath()
    {
        FindObjectOfType<TouchRing>().NoteDie(this);
    }
}
