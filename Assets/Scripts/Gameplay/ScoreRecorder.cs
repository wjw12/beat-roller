using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreRecorder : MonoBehaviour {
    public int perfectScore = 300;
    public int goodScore = 150;
    public int fairScore = 50;
    public float comboBonus = 0.1f;
    public Text scoreText;
    public Text comboText;
    public CanvasGroup statistics;
    public Text statScore, statCombo, statRank;
    
    public int TotalScore { get; private set; }
    public int Combo { get; private set; }

    int maxScore = 0; // maximum possible score, to calculate ranking
    int maxCombo = 0;
    float SSS = 0.99f;
    float SS = 0.97f;
    float S = 0.95f;
    float A = 0.9f;
    float B = 0.8f;
    float C = 0.7f;

	// Use this for initialization
	void Start () {
        TotalScore = 0;
	}
	
	// Update is called once per frame
	void Update () {
        // update text areas
        scoreText.text = TotalScore.ToString();
        comboText.text = Combo.ToString();
	}

    public void ShowStatistics()
    {
        statistics.gameObject.SetActive(true);
        GroupFadeAlpha(statistics, 0f, 1f, 1f);
        statScore.text = TotalScore.ToString();
        statCombo.text = Combo.ToString() + " / " + maxCombo.ToString();
        statRank.text = GetRank();
    }

    IEnumerator GroupFadeAlpha(CanvasGroup group, float start, float end, float time)
    {
        float t = 0f;
        while (t < time) {
            group.alpha = start + (end - start) * t / time;
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    public void Perfect()
    {
        TotalScore += perfectScore;
        maxScore += perfectScore;
        IncrementCombo();
    }

    public void Good()
    {
        TotalScore += goodScore;
        maxScore += perfectScore;
        IncrementCombo();
    }

    public void Fair()
    {
        TotalScore += fairScore;
        maxScore += perfectScore;
        IncrementCombo();
    }

    public void Miss()
    {
        maxScore += perfectScore;
        InterruptCombo();
    }

    public void InterruptCombo()
    {
        maxCombo++;
        maxScore += Mathf.FloorToInt(maxCombo * comboBonus);
        Combo = 0;
    }

    public void IncrementCombo()
    {
        TotalScore += Mathf.FloorToInt(Combo * comboBonus);
        maxScore += Mathf.FloorToInt(maxCombo * comboBonus);
        maxCombo++;
        Combo++;
    }

    public string GetRank()
    {
        if (TotalScore >= Mathf.RoundToInt(maxScore * SSS))
            return "SSS";
        else if (TotalScore >= Mathf.RoundToInt(maxScore * SS))
            return "SS";
        else if (TotalScore >= Mathf.RoundToInt(maxScore * S))
            return "S";
        else if (TotalScore >= Mathf.RoundToInt(maxScore * A))
            return "A";
        else if (TotalScore >= Mathf.RoundToInt(maxScore * B))
            return "B";
        else if (TotalScore >= Mathf.RoundToInt(maxScore * C))
            return "C";
        else
            return "F";
    }
}
