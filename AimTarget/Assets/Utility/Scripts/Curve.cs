using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My {

  [System.Serializable]
  public class Curve {
    [SerializeField] public AnimationCurve curve;
    private float t = 0.0f;
    private float maxt = 1.0f;

    public void SetMax(float T) { maxt = T; }
    public void AddTime(float dt) {
      t += dt;
      t = Mathf.Clamp(t, 0.0f, maxt);
    }
    public void ReSet() { t = 0.0f; }
    public float GetValue() {
      return curve.Evaluate(t / maxt);
    }
  }
}