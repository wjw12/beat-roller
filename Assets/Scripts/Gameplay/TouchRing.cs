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

    public float ringRad = 4.8f;
    public float screenRad = 5.5f;
    public float rmin = 4.7f;
    public float rmax = 4.9f;
    public int n_queues = 18;
    public GameObject singleNotePrefab;
    public GameObject longNotePrefab;
    public GameObject snapPointPrefab;
    public GameObject specialNotePrefab;
    
    public float flyTime = 1.3f;
    
    bool isRotatable = false;

    float currTime = 0f;
    int idx = 0;
    int deg_perqueue, deg_offset;
    List<Queue<BaseNote>> queues;
    List<NoteDescriptor> musicScore;
    List<SnapPoint> snapPoints;

    Vector2 lastMousePosition = new Vector2(0, 0);

	// Use this for initialization
	void Start () {
        LoadMusicScore();
        deg_perqueue = 360 / n_queues;
        deg_offset = deg_perqueue / 2;

        snapPoints = new List<SnapPoint>();
        queues = new List<Queue<BaseNote>>();
        for (int i = 0; i < n_queues; i++)
            queues.Add(new Queue<BaseNote>());
	}
	
	// Update is called once per frame
	void Update () {
        currTime += Time.deltaTime;

        // generate notes
        if (idx < musicScore.Count && currTime > musicScore[idx].arriveTime - flyTime)
        {
            // instantiate
            CreateNote(musicScore[idx]);
            ++idx;
            if (idx < musicScore.Count && currTime > musicScore[idx].arriveTime - flyTime)
            {
                CreateNote(musicScore[idx]);
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

        if (isRotatable)
        {
            foreach (SnapPoint sp in snapPoints)
            {
                int center_deg = (Mathf.FloorToInt(Mathf.Atan2(sp.transform.position.y, sp.transform.position.x) * Mathf.Rad2Deg) + 360) % 360;
                int q1 = ((center_deg + deg_offset + 360) % 360) / deg_perqueue;
                int q2 = (center_deg + deg_offset) % deg_perqueue > deg_perqueue / 2 ? (q1 + 1 + n_queues) % n_queues : (q1 - 1 + n_queues) % n_queues;

                BaseNote bn;
                if (queues[q1].Count > 0)
                {
                    bn = queues[q1].Peek();
                    if (bn != null && Vector2.Distance(bn.transform.position, sp.transform.position) < sp.radius * 1.5f)
                        bn.TouchBegin(currTime);
                }

                if (queues[q2].Count > 0)
                {
                    bn = queues[q2].Peek();
                    if (bn != null && Vector2.Distance(bn.transform.position, sp.transform.position) < sp.radius * 1.5f)
                        bn.TouchBegin(currTime);
                }
            }
        }
        

    }

    void HandleTouch(Touch touch)
    {
        TouchPhase phase = touch.phase;
        Vector2 wpos = Camera.main.ScreenToWorldPoint(touch.position);
        Vector2 dwpos = Camera.main.ScreenToWorldPoint(touch.position) - Camera.main.ScreenToWorldPoint(touch.position - touch.deltaPosition);
        float r = Vector2.Distance(wpos, Vector2.zero);
        int deg = ((int)(Mathf.Atan2(wpos.y, wpos.x) * Mathf.Rad2Deg + deg_offset) + 360) % 360;
        BaseNote bn;

        if (!isRotatable)
        {
            if (phase == TouchPhase.Began)
            {
                
                if (r > rmin && r < rmax)
                {
                    if (queues[deg / deg_perqueue].Count > 0) {
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
        }
        else
        {
            // the ring can be rotated if touching 1 or 2 snap points
            
            if (snapPoints.Count == 1)
            {
                if (phase == TouchPhase.Began && !snapPoints[0].isSnapped)
                {
                    if (Vector2.Distance(snapPoints[0].transform.position, wpos) < snapPoints[0].radius)
                    {
                        snapPoints[0].SetSnap(true);
                    }
                }
                else if (phase == TouchPhase.Moved && snapPoints[0].isSnapped)
                {
                    if (Vector2.Distance(snapPoints[0].transform.position, wpos) > snapPoints[0].detachRadius)
                    {
                        snapPoints[0].SetSnap(false);
                    }

                    // rotate the ring
                    if (snapPoints[0].isSnapped)
                    {
                        transform.rotation *= Quaternion.FromToRotation(wpos - dwpos, wpos);
                    }

                }
                else if (phase == TouchPhase.Ended && snapPoints[0].isSnapped)
                {
                    if (Vector2.Distance(snapPoints[0].transform.position, wpos) < snapPoints[0].detachRadius)
                    {
                        snapPoints[0].SetSnap(false);
                    }
                }
            }
            else if(snapPoints.Count == 2)
            {
                SnapPoint sp = null;
                if (Vector2.Distance(snapPoints[0].transform.position, wpos) < snapPoints[0].detachRadius)
                    sp = snapPoints[0];
                if (Vector2.Distance(snapPoints[1].transform.position, wpos) < snapPoints[1].detachRadius)
                    sp = snapPoints[1];

                if (sp != null)
                {
                    if (phase == TouchPhase.Began && !sp.isSnapped)
                    {
                        if (Vector2.Distance(sp.transform.position, wpos) < sp.radius)
                        {
                            sp.SetSnap(true);
                        }
                    }
                    else if (phase == TouchPhase.Moved && sp.isSnapped)
                    {
                        if (Vector2.Distance(sp.transform.position, wpos) > sp.detachRadius)
                        {
                            sp.SetSnap(false);
                        }

                        // rotate the ring
                        if (sp.isSnapped)
                        {
                            transform.rotation *= Quaternion.FromToRotation(wpos - dwpos / 2, wpos);
                        }

                    }
                    else if (phase == TouchPhase.Ended && snapPoints[0].isSnapped)
                    {
                        if (Vector2.Distance(sp.transform.position, wpos) < sp.detachRadius)
                        {
                            sp.SetSnap(false);
                        }
                    }
                }
            }
            

        }
    }

    public void NoteDie(BaseNote note)
    {
        queues[(int)note.angle_deg / deg_perqueue].Dequeue();
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
                queues[(int)n.angle_deg / deg_perqueue].Enqueue(ln);
                break;
            case NoteType.Rotate:
                go = Instantiate(snapPointPrefab) as GameObject;
                go.transform.position = new Vector2(ringRad * Mathf.Cos(n.angle_deg * Mathf.Deg2Rad), ringRad * Mathf.Sin(n.angle_deg * Mathf.Deg2Rad));
                go.transform.parent = this.transform;
                // save in a list
                snapPoints.Add(go.GetComponent<SnapPoint>());
                isRotatable = true;
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
                queues[(int)n.angle_deg / deg_perqueue].Enqueue(sn);
                break;
            case NoteType.EndRotate:
                isRotatable = false;
                StartCoroutine(snapPoints[0].DieAfter(flyTime));
                snapPoints.Remove(snapPoints[0]);
                break;
            default:
                break;
        }
        
    }

   

    void LoadMusicScore()
    {
        //string fileName = "test.txt";
        musicScore = new List<NoteDescriptor>();
        
        //StreamReader file = new StreamReader(Application.dataPath + "/Resources/" + fileName);
        
        TextAsset t = Resources.Load("test") as TextAsset;
        var arrayString = t.text.Split('\n');
        NoteDescriptor n;
        foreach (var line in arrayString)
        {
            if (line[0] == '#')
                continue;
            string[] parts = line.Split(' ');
            switch (parts[0])
            {
                case "N":
                    n = new NoteDescriptor(NoteType.Normal, float.Parse(parts[1]), float.Parse(parts[2]));
                    musicScore.Add(n);
                    break;
                case "R":
                    n = new NoteDescriptor(NoteType.Rotate, float.Parse(parts[1]), float.Parse(parts[2]));
                    musicScore.Add(n);
                    break;
                case "L":
                    n = new NoteDescriptor(NoteType.Long, float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]));
                    musicScore.Add(n);
                    break;
                case "S":
                    n = new NoteDescriptor(NoteType.Special, float.Parse(parts[1]), float.Parse(parts[2]));
                    musicScore.Add(n);
                    break;
                case "X":
                    n = new NoteDescriptor(NoteType.EndRotate, float.Parse(parts[1]));
                    musicScore.Add(n);
                    break;
                default:
                    Debug.LogError("Unrecognized note description.");
                    break;
            }

        }

        //file.Close();
        
    }
}
