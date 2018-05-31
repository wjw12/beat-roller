using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChainLightning : MonoBehaviour
{
    public LineRenderer[] lineRenderer;
    public LineRenderer lightRenderer;
    
    public static float segmentLength = 0.3f;

    LightningBolt lb;
    Vector2 start, end;
    bool fade = false;
    float alpha = 1f;
    float fade_duration = 0.2f;
    float fade_timer = 0f;

    void Start()
    {
        lb = new LightningBolt(segmentLength, this);
    }

    public void Fade()
    {
        fade = true;
    }

    public void SetLineStart(float x, float y)
    {
        start = new Vector2(x, y);
    }

    public void SetLineEnd(float x, float y)
    {
        end = new Vector2(x, y);
    }

    void Update()
    {
        if (fade)
        {
            fade_timer += Time.deltaTime;
            alpha = 1 - fade_timer / fade_duration;
            if (alpha < 0f) return;
        }
        lb.DrawLightning(start, end, alpha);
    }

    private class LightningBolt
    {
        public float SegmentLength { get; set; }
        public bool IsActive { get; private set; }
        ChainLightning cl;

        public LightningBolt(float segmentLength, ChainLightning lightning)
        {
            SegmentLength = segmentLength;
            cl = lightning;
        }

        public void DrawLightning(Vector2 source, Vector2 target, float alphaMultiplier)
        {
            //Calculated amount of Segments
            float distance = Vector2.Distance(source, target);
            int segments = 5;
            if (distance > SegmentLength)
            {
                segments = Mathf.FloorToInt(distance / SegmentLength) + 2;
            }
            else
            {
                segments = 4;
            }

            for (int i = 0; i < cl.lineRenderer.Length; i++)
            {
                // Set the amount of points to the calculated value
                cl.lineRenderer[i].SetVertexCount(segments);
                cl.lineRenderer[i].SetPosition(0, source);
                Vector2 lastPosition = source;
                for (int j = 1; j < segments - 1; j++)
                {
                    //Go linear from source to target
                    Vector2 tmp = Vector2.Lerp(source, target, (float)j / (float)segments);
                    //Add randomness
                    lastPosition = new Vector2(tmp.x + Random.Range(-0.2f, 0.2f),
                        tmp.y + Random.Range(-0.2f, 0.2f));
                    //Set the calculated position
                    cl.lineRenderer[i].SetPosition(j, lastPosition);
                }
                cl.lineRenderer[i].SetPosition(segments - 1, target);
                Color c = cl.lineRenderer[i].startColor;
                c.a = alphaMultiplier;
                cl.lineRenderer[i].startColor = c;
                c = cl.lineRenderer[i].endColor;
                c.a = alphaMultiplier;
                cl.lineRenderer[i].endColor = c;
            }
            //Set the points for the light
            cl.lightRenderer.SetPosition(0, source);
            cl.lightRenderer.SetPosition(1, target);
            //Set the color of the light
            Color lightColor = new Color(0.995f, 0.95f, 0.7f, Random.Range(0.2f, 1f) * alphaMultiplier);
            cl.lightRenderer.SetColors(lightColor, lightColor);
        }
    }
}

