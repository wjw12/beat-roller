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
    
    public float flyTime = 1.3f;

    Vector2 screenCenter;
    bool isRotatable = false;
    float pixelsPerUnit;

    float currTime = 0f;
    int idx = 0;
    int deg_perqueue, deg_offset;
    List<Queue<BaseNote>> queues;
    List<NoteDescriptor> musicScore;

	// Use this for initialization
	void Start () {
        LoadMusicScore();
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        pixelsPerUnit = screenCenter.y / screenRad;
        deg_perqueue = 360 / n_queues;
        deg_offset = deg_perqueue / 2;

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
            HandleTouch(touch.position, touch.phase);
        }

        // for mouse input
        if (Input.GetMouseButtonDown(0))
        {
            HandleTouch(Input.mousePosition, TouchPhase.Began);
        }

        if (Input.GetMouseButtonUp(0))
        {
            HandleTouch(Input.mousePosition, TouchPhase.Ended);
        }
	}

    void HandleTouch(Vector2 pos, TouchPhase phase)
    {
        float r = Vector2.Distance(pos, screenCenter) / pixelsPerUnit;
        float x = (float)pos.x - screenCenter.x;
        float y = (float)pos.y - screenCenter.y;
        int deg = ((int)(Mathf.Atan2(y, x) * Mathf.Rad2Deg + deg_offset) + 360) % 360;
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
                GameObject go = Instantiate(singleNotePrefab, transform) as GameObject;
                SingleNote note = go.GetComponent<SingleNote>();
                note.noteType = NoteType.Normal;
                note.startTime = currTime;
                note.arriveTime = n.arriveTime;
                note.angle_deg = n.angle_deg;
                note.velocity = ringRad / flyTime;
                queues[(int)n.angle_deg / deg_perqueue].Enqueue(note);
                break;
            case NoteType.Long:
                go = Instantiate(longNotePrefab, transform) as GameObject;
                LongNote ln = go.GetComponent<LongNote>();
                ln.noteType = NoteType.Long;
                ln.startTime = currTime;
                ln.headArriveTime = n.arriveTime;
                ln.tailArriveTime = n.endTime;
                ln.angle_deg = n.angle_deg;
                ln.velocity = ringRad / flyTime;
                queues[(int)n.angle_deg / deg_perqueue].Enqueue(ln);
                break;
            default:
                break;
        }
        
    }

    void LoadMusicScore()
    {
        string fileName = "test2.txt";
        musicScore = new List<NoteDescriptor>();
        
        StreamReader file = new StreamReader(Application.dataPath + "/Resources/" + fileName);

        string line;
        NoteDescriptor n;
        while ((line = file.ReadLine()) != null)
        {
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

        file.Close();
        
    }
}
