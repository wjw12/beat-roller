using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNote : BaseNote {
    public float startTime, headArriveTime, tailArriveTime;
    public GameObject head, tail, link;

    float t1 = 0.08f;
    float t2 = 0.17f;
    float t3 = 0.25f;

    float currTime;
    bool isHeadComing = true;
    bool isPressing = false;
    bool isTailShown = false;
    bool isHeadOver = false;
    bool isTailOver = false;
    bool isReadyToFinish = false;

    LineRenderer lineRd;

    
    public override void TouchBegin(float t)
    {
        if (headArriveTime - t > 2 * t3) return;
        if (isTailOver || isReadyToFinish) return;
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
            if (delta_t < t1 * 1.3f)
            {
                Perfect();
            }
            else if (delta_t < t2 * 1.3f)
            {
                Good();
            }
            else if (delta_t < t3 * 1.3f)
            {
                Fair();
            }
            else
            {
                MissTail();
            }

            if (!isTailShown)
            {
                StopCoroutine("CreateTailAfter");
            }
        }
    }

    // Use this for initialization
    void Start () {
        currTime = startTime;
        angle_rad = angle_deg * Mathf.Deg2Rad;
        tail.SetActive(false);
        lineRd = link.GetComponent<LineRenderer>();
        lineRd.positionCount = 2;
        lineRd.SetPosition(0, Vector3.zero);
        lineRd.SetPosition(1, Vector3.zero);
        head.transform.position = Vector2.zero;
        tail.transform.position = Vector2.zero;

        StartCoroutine(CreateTailAfter(tailArriveTime - headArriveTime));
    }
	
	// Update is called once per frame
	void Update () {
        if (isHeadOver && isTailOver && !isReadyToFinish)
        {
            isReadyToFinish = true;
            StartCoroutine(FinishAfter(t2));
        }

		if (isHeadComing)
        {
            head.transform.Translate(new Vector2(velocity * Time.deltaTime * Mathf.Cos(angle_rad),
                velocity * Time.deltaTime * Mathf.Sin(angle_rad)));
            lineRd.SetPosition(0, head.transform.position);
            if (currTime - headArriveTime > t3)
            {
                MissHead();
            }
        }

        if (isTailShown && !isTailOver)
        {
            tail.transform.Translate(new Vector2(velocity * Time.deltaTime * Mathf.Cos(angle_rad),
                velocity * Time.deltaTime * Mathf.Sin(angle_rad)));
            lineRd.SetPosition(1, tail.transform.position);
            if (!isPressing && currTime > tailArriveTime - t3)
            {
                MissTail();
            }
            if (isPressing && currTime > tailArriveTime + t3)
            {
                isTailOver = true;
                Fair();
            }
        }
        if (isPressing)
        {
            // shining effect
        }

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
            Debug.Log("Head missed");
            isHeadOver = true;
            isHeadComing = false;
            head.GetComponent<Animator>().SetBool("Fade", true);
            StartCoroutine(DestroyAfter(head, t3));
        }
    }

    void MissTail()
    {
        if (isTailShown && !isTailOver)
        {
            Debug.Log("Tail missed");
            isTailShown = false;
            isTailOver = true;
        }
    }

    void Perfect()
    {
        Success();
    }

    void Good()
    {
        Success();
    }

    void Fair()
    {
        Success();
    }

    void Success()
    {
        // combo +1
        Debug.Log("long note success");
        
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
        if (tail != null)
            tail.GetComponent<Animator>().SetBool("Fade", true);

        // link fade animation

        NotifyDeath();
        yield return new WaitForSeconds(t);
        StopAllCoroutines();
        Destroy(link);
        Destroy(tail);
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
