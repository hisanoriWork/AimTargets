using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using My;
using UnityEngine.UI;
public class SelectCrosshair : MonoBehaviour
{
  public TextMesh text;
  public Image image;
  public FPSPlayer player;
  public List<Sprite> list;
  int index = 0;
  void Awake() {
    index = 0;
    image.sprite = list[index];
    TextUpdate();
  }

  public void UseLaser(bool f) {
    player.useLaser = f;
  }

  public void NextCrosshair() {
    index = Mathf.Clamp(index + 1, 0, list.Count()-1);
    image.sprite = list[index];
    TextUpdate();
  }

  public void BackCrosshair() {
    index = Mathf.Clamp(index - 1, 0, list.Count() - 1);
    image.sprite = list[index];
    TextUpdate();
  }
  public void TextUpdate() {
    text.text = (index+1).ToString();
  }
}
