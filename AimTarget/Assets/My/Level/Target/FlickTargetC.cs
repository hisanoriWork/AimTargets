using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using My;
public class FlickTargetC : MonoBehaviour
{
  public float minScale;
  public float maxScale;
  public float time = 1f;
  public TargetC targetc;

  void Awake() {
    float s = UnityEngine.Random.Range(minScale, maxScale);
    transform.localScale = Vector3.one * s;
  }
  void FixedUpdate() {
    
    time -= Time.fixedDeltaTime;
    if (time < 0)
      targetc.DestroySelf();
  }
}
