using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;
namespace My {
  public class TimeTextUpdater : MonoBehaviour {
    public Timer timer;
    public Text text;
    public bool isUpdating = true;
    void Awake() {
      timer.onDigitalTimeChanged.Where(_ => isUpdating).Subscribe(time => TextUpdate(time));
    }

    void Start() {
      TextUpdate(timer.digitalTime);
    }

    public void TextUpdate(int i) {
      text.text = i.ToString();
    }
  }
}
