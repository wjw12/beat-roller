using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public enum NoteType { Normal, Long, Rotate, Special, EndRotate };

public struct NoteDescriptor
{
    public NoteDescriptor(NoteType t, float t1) { noteType = t; arriveTime = t1; angle_deg = -10; endTime = -1f; }
    public NoteDescriptor(NoteType t, float t1, float deg) { noteType = t;  arriveTime = t1; angle_deg = deg; endTime = -1f; }
    public NoteDescriptor(NoteType t, float t1, float deg, float t2) { noteType = t; arriveTime = t1; angle_deg = deg; endTime = t2; }
    public NoteType noteType;
    public float arriveTime;
    public float angle_deg;
    public float endTime;
}

public class TouchRing : MonoBehaviour {

    public float ringRad;
    public float screenRad;
    public float textDisplayRad;
    public float rmin;
    public float rmax;
    public int n_queues;
    public GameObject singleNotePrefab;
    public GameObject longNotePrefab;
    public GameObject specialNotePrefab;
    
    public float flyTime;

    float currTime = -0.5f; // offset
    int idx = 0;
    int deg_perqueue, deg_offset;
    List<Queue<BaseNote>> queues;
    List<NoteDescriptor> musicMap;

    Vector2 lastMousePosition = new Vector2(0, 0);

    RingEmitParticles particleEmitter;
    Animator ringAnim;
    TextObjectPool pool;
    
    ScoreRecorder scoreRecorder;

    bool hasStarted = false;

    // Use this for initialization
    void Start () {
        deg_perqueue = 360 / n_queues;
        deg_offset = deg_perqueue / 2;
        
        queues = new List<Queue<BaseNote>>();
        for (int i = 0; i < n_queues; i++)
            queues.Add(new Queue<BaseNote>());

        particleEmitter = GetComponent<RingEmitParticles>();
        ringAnim = GetComponent<Animator>();
        pool = FindObjectOfType<TextObjectPool>();
        scoreRecorder = FindObjectOfType<ScoreRecorder>();
        //LoadMusicScore("light 70s.csv");
        //LoadBackgroundPic("Snowflake3_1280x720.jpg");
        //LoadMusic("raja_ffm_-_the_light.mp3");
    }

    public void SetStart()
    {
        hasStarted = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (!hasStarted) return;
        currTime += Time.deltaTime;

        // generate notes
        if (idx < musicMap.Count && currTime > musicMap[idx].arriveTime - flyTime)
        {
            // instantiate
            CreateNote(musicMap[idx]);
            ++idx;
            if (idx < musicMap.Count && currTime > musicMap[idx].arriveTime - flyTime)
            {
                CreateNote(musicMap[idx]);
                ++idx;
            }
        } 
        
		
        foreach (var touch in Input.touches)
        {
            HandleTouch(touch);
        }

#if UNITY_EDITOR
        // for mouse input, fake touch
        Touch fakeTouch = new Touch();
        if (Input.GetMouseButtonDown(0))
        {
            fakeTouch.position = Input.mousePosition;
            fakeTouch.phase = TouchPhase.Began;
            HandleTouch(fakeTouch);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            fakeTouch.position = Input.mousePosition;
            fakeTouch.phase = TouchPhase.Ended;
            HandleTouch(fakeTouch);
        }
        else if (Input.GetMouseButton(0))
        {
            fakeTouch.position = Input.mousePosition;
            fakeTouch.phase = TouchPhase.Moved;
            fakeTouch.deltaPosition = fakeTouch.position - lastMousePosition;
            HandleTouch(fakeTouch);
        }
        
        lastMousePosition = Input.mousePosition;

#endif
        

    }

    void HandleTouch(Touch touch)
    {
        TouchPhase phase = touch.phase;
        Vector2 wpos = Camera.main.ScreenToWorldPoint(touch.position);
        Vector2 dwpos = Camera.main.ScreenToWorldPoint(touch.position) - Camera.main.ScreenToWorldPoint(touch.position - touch.deltaPosition);
        float r = Vector2.Distance(wpos, Vector2.zero);
        int deg = ((int)(Mathf.Atan2(wpos.y, wpos.x) * Mathf.Rad2Deg + deg_offset) + 360) % 360;
        BaseNote bn;

        
        if (phase == TouchPhase.Began)
        {

            if (r > rmin && r < rmax)
            {
                particleEmitter.DoEmit();
                ringAnim.Play("ring touched", -1, 0f);

                if (queues[deg / deg_perqueue].Count > 0)
                {
                    bn = queues[deg / deg_perqueue].Peek();
                    if (bn != null && (bn.noteType == NoteType.Normal || bn.noteType == NoteType.Long))
                    {
                        bn.TouchBegin(currTime);
                    }
                }
            }
        }
        else if (phase == TouchPhase.Ended)
        {
            if (queues[deg / deg_perqueue].Count > 0)
            {
                bn = queues[deg / deg_perqueue].Peek();
                if (bn != null && bn.noteType == NoteType.Long)
                {
                    bn.TouchEnd(currTime);
                }
            }
        }
        else if (phase == TouchPhase.Stationary || phase == TouchPhase.Moved)
        {
            if (queues[deg / deg_perqueue].Count > 0)
            {
                bn = queues[deg / deg_perqueue].Peek();
                if (bn != null && bn.noteType == NoteType.Special)
                {
                    if (currTime >= bn.time)
                        bn.TouchBegin(currTime);
                }
            }
        }
        
    }

    public void NoteDie(BaseNote note)
    {
        queues[(int)note.angle_deg / deg_perqueue].Dequeue();
    }

    public void PerfectHit(float angle_rad)
    {
        GameObject go = pool.GetText("perfect");
        PlaceText(go, angle_rad);
        scoreRecorder.Perfect();
    }

    public void GoodHit(float angle_rad)
    {
        GameObject go = pool.GetText("good");
        PlaceText(go, angle_rad);
        scoreRecorder.Good();
    }

    public void FairHit(float angle_rad)
    {
        GameObject go = pool.GetText("fair");
        PlaceText(go, angle_rad);
        scoreRecorder.Fair();
    }

    public void MissHit(float angle_rad)
    {
        GameObject go = pool.GetText("miss");
        PlaceText(go, angle_rad);

        scoreRecorder.InterruptCombo();
    }


    void PlaceText(GameObject go, float angle_rad)
    {
        go.transform.position = new Vector2(textDisplayRad * Mathf.Cos(angle_rad),
            textDisplayRad * Mathf.Sin(angle_rad));
        go.transform.Rotate(Vector3.forward * (Mathf.Rad2Deg * angle_rad - 90));
        go.GetComponent<Animator>().Play("ExpandAndFade", -1, 0);
        StartCoroutine(DistroyTextAfter(go, 0.2f));
    }

    IEnumerator DistroyTextAfter(GameObject go, float t)
    {
        yield return new WaitForSeconds(t);
        pool.DestroyTextObject(go);
    }

    void CreateNote(NoteDescriptor n)
    {
        switch (n.noteType)
        {
            case NoteType.Normal:
                // create a normal note at the center
                GameObject go = Instantiate(singleNotePrefab) as GameObject;
                SingleNote note = go.GetComponent<SingleNote>();
                note.noteType = NoteType.Normal;
                note.startTime = currTime;
                note.arriveTime = n.arriveTime;
                note.angle_deg = n.angle_deg;
                note.velocity = ringRad / flyTime;
                note.time = n.arriveTime;
                queues[(int)n.angle_deg / deg_perqueue].Enqueue(note);
                break;
            case NoteType.Long:
                go = Instantiate(longNotePrefab) as GameObject;
                LongNote ln = go.GetComponent<LongNote>();
                ln.noteType = NoteType.Long;
                ln.startTime = currTime;
                ln.headArriveTime = n.arriveTime;
                ln.tailArriveTime = n.endTime;
                ln.angle_deg = n.angle_deg;
                ln.velocity = ringRad / flyTime;
                ln.time = n.arriveTime;
                queues[(int)n.angle_deg / deg_perqueue].Enqueue(ln);
                break;
            case NoteType.Special:
                go = Instantiate(specialNotePrefab) as GameObject;
                go.transform.parent = null;
                SpecialNote sn = go.GetComponent<SpecialNote>();
                sn.noteType = NoteType.Special;
                sn.startTime = currTime;
                sn.arriveTime = n.arriveTime;
                sn.angle_deg = n.angle_deg;
                sn.velocity = ringRad / flyTime;
                sn.time = n.arriveTime;
                queues[(int)n.angle_deg / deg_perqueue].Enqueue(sn);
                break;
            default:
                break;
        }
        
    }

    public void SeekTo(float t)
    {
        currTime = t;
    }
    
    public void LoadMusicMap(string path)
    {
        musicMap = new List<NoteDescriptor>();

        StreamReader file = new StreamReader(path);

        //TextAsset t = Resources.Load(path) as TextAsset;
        //var arrayString = t.text.Split('\n');

        NoteDescriptor n;
        //foreach (var line in arrayString)
        string line = file.ReadLine();
        while (line != null)
        {
            if (line[0] == '#')
                continue;
            string[] parts = line.Split(',');
            switch (parts[0])
            {
                case "N":
                    n = new NoteDescriptor(NoteType.Normal, float.Parse(parts[1]), float.Parse(parts[2]));
                    musicMap.Add(n);
                    break;
                case "R":
                    n = new NoteDescriptor(NoteType.Rotate, float.Parse(parts[1]), float.Parse(parts[2]));
                    musicMap.Add(n);
                    break;
                case "L":
                    n = new NoteDescriptor(NoteType.Long, float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]));
                    musicMap.Add(n);
                    break;
                case "S":
                    n = new NoteDescriptor(NoteType.Special, float.Parse(parts[1]), float.Parse(parts[2]));
                    musicMap.Add(n);
                    break;
                case "X":
                    n = new NoteDescriptor(NoteType.EndRotate, float.Parse(parts[1]));
                    musicMap.Add(n);
                    break;
                default:
                    Debug.LogError("Unrecognized note description.");
                    break;
            }

            line = file.ReadLine();
        }

        file.Close();
        
    }

    public float GetFirstNoteTime()
    {
        return musicMap[0].arriveTime;
    }
}

