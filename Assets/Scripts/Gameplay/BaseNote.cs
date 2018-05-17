using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNote : MonoBehaviour {
    public NoteType noteType;

    public float angle_deg, angle_rad, velocity, time;

    public virtual void TouchBegin(float t) { }
    public virtual void TouchEnd(float t) { }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
