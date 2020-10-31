using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using My;
using System;
using UniRx;
public class TrackingTargetC : MonoBehaviour {
  public TargetC targetC;
  public Renderer meshRenderer;
  public float minTime;
  public float maxTime;

  public float minVel;
  public float maxVel;
  public Color offColor;
  public Color onColor;
  public Timer timer;
  public Transform areaT;
  public Vector3 area;
  Vector3 vec;
  float vel;
  Material mat;
  void Awake() {
    areaT = transform.parent;
    mat = meshRenderer.material;
    timer.SetTime(UnityEngine.Random.Range(minTime, maxTime));
    timer.Reset();
    timer.whenTimeIsUp.Subscribe(_ => {
      int i = 0;
      float time = 0;
      while (i < 100) {
        float angle = UnityEngine.Random.Range(0f, 360f);
        vec = Quaternion.AngleAxis(angle, transform.up)* Quaternion.AngleAxis(angle, transform.right)*Quaternion.AngleAxis(angle, transform.forward) * transform.right;
        vel = UnityEngine.Random.Range(minVel, maxVel);
        time = UnityEngine.Random.Range(minTime, maxTime);
        Vector3 d = transform.position + vec * vel * time;
        Vector3 a = areaT.position - area;
        Vector3 b = areaT.position + area;
        if (d.x > a.x && d.y > a.y && d.z > a.z && d.x < b.x && d.y < b.y && d.z < b.z)
          break;
        i++;
      }
      timer.SetTime(time);
    });
  }
  void FixedUpdate() {
    transform.position += vec * vel * Time.fixedDeltaTime;
  }

  public void OnTarget(){
    mat.color = onColor;
    if (mat.HasProperty("_EmissionColor")) {
      mat.SetColor("_EmissionColor", onColor);
    }
  }

  public void OffTarget() {
    mat.color = offColor;
    if (mat.HasProperty("_EmissionColor")) {
      mat.SetColor("_EmissionColor", offColor);
    }
  }
}
