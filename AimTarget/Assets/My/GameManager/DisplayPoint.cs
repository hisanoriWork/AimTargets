using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPoint : MonoBehaviour
{
  TextMesh text;
  public float t;
  public float vel;
  void Awake() {
    text = GetComponent<TextMesh>();
  }

  void FixedUpdate() {
    float dt = Time.fixedDeltaTime;
    t -= dt;
    if (t > 0)
      transform.position += transform.up * vel * dt;
    else Destroy(gameObject);
  }

  public void TextUpdate(int value) {
    text.text = "+" +value.ToString();
  }
}
