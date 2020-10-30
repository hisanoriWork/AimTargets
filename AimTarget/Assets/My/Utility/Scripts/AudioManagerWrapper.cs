using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace My {
  public class AudioManagerWrapper : MonoBehaviour {
    public void SetSEVolume(int value) {
      SEManager.instance.volume = value;
    }
    public void SetBGMVolume(int value) {
      BGMManager.instance.volume = value;
    }
    public void AddSEVolume(int value) {
      SEManager.instance.volume += value;
    }
    public void AddBGMVolume(int value) {
      BGMManager.instance.volume += value;
    }
    public void PlaySE(string clipname) {
      SEManager.instance.Play(clipname);
    }
    public void PlayBGM(string clipname) {
      BGMManager.instance.Play(clipname);
    }
    public void ReplayBGM(string clipname) {
      BGMManager.instance.Replay(clipname);
    }
  }
}
