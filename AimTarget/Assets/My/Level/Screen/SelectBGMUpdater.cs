using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using My;
using UnityEngine.Events;
public class SelectBGMUpdater : MonoBehaviour
{
  public TextMesh text;
  public MousePointee pointee;
 
  void Awake() {
    pointee.onEvent.AddListener(TextUpdate);
  }
  void Start() {
    TextUpdate();
  }

  public void TextUpdate() {
    text.text = BGMManager.instance.audioName;
  }
}
