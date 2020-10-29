using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;
namespace My {
  public class TimeTextUpdater : MonoBehaviour {
    public Timer timer;
    Text mText;
    public bool isUpdating = true;
    void Awake() {
      mText = GetComponent<Text>();
      timer.onDigitalTimeChanged.Where(_ => isUpdating).Subscribe(time => TextUpdate(time));
    }

    public void TextUpdate(int i) {
      mText.text = i.ToString();
    }
  }
}
