using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace My{
  public class ScoreRate : MonoBehaviour {
    public TextMesh score;
    public TextMesh rate;
    public int s = 0;
    public float r = 0;


    public void SetScore(int i) {
      s = i;
      score.text = s.ToString();
    }

    public void SetRate(float i) {
      r = i;
      rate.text = ((int)r).ToString() + "%";
    }
  }
}