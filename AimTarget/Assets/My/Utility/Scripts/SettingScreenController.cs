using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace My {
  public class SettingScreenController : MonoBehaviour {
    public TextMesh SEText;
    public TextMesh BGMText;
    public TextMesh DPIText;
    public FPSPlayer fpsPlayer;
    void Start() {
      SEUpdate();
      BGMUpdate();
      DPIUpdate();
    }

    public void SEUpdate() {
      SEText.text = SEManager.instance.volume.ToString();
    }

    public void BGMUpdate() {
      BGMText.text = BGMManager.instance.volume.ToString();
    }
    public void DPIUpdate() {
      DPIText.text = fpsPlayer.dpi.ToString();
    }

    public void AddDPI(int value) {
      fpsPlayer.dpi += value;
      DPIUpdate();
    }
  }
}
