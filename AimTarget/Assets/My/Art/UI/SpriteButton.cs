using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace My {
  public class SpriteButton : MonoBehaviour {
    public enum BlendMode {
      EAdd,
      EDiff,
      EScrean,
    }
    /*****public field*****/
    public SpriteRenderer sprite;
    public TextMesh text;
    public Collider col;
    public MousePointee mousepointee;
    public BlendMode blendMode = BlendMode.EDiff;
    public Color onColor = Color.white;
    public Color clickColor = Color.white;
    public Color offColor = Color.white;
    /*****private field*****/
    Color mTextColor;
    Color mSpriteColor;
    /*****monobehaviour method*****/
    void Awake() {
      if (text)
        mTextColor = text.color;
      if (sprite)
        mSpriteColor = sprite.color;
      switch (blendMode) {
        case BlendMode.EDiff:
          mousepointee.onEvent.AddListener(SetOn);
          mousepointee.clickEvent.AddListener(SetClick);
          mousepointee.offEvent.AddListener(SetOff);
          break;
        case BlendMode.EAdd:
          mousepointee.onEvent.AddListener(SetOna);
          mousepointee.clickEvent.AddListener(SetClicka);
          mousepointee.offEvent.AddListener(SetOffa);
          break;
        case BlendMode.EScrean:
          mousepointee.onEvent.AddListener(SetOns);
          mousepointee.clickEvent.AddListener(SetClicks);
          mousepointee.offEvent.AddListener(SetOffs);
          break;
      }
    }
    /*****public method*****/
    public void SetActive(bool f) {
      if (sprite)
        sprite.gameObject.SetActive(f);
      if (text)
        text.gameObject.SetActive(f);
      if (col)
        col.enabled = f;
    }
    public void SetOn() {
      if (text)
        text.color = mTextColor * onColor;
      if (sprite)
        sprite.color = mSpriteColor * onColor;
    }
    public void SetClick() {
      if (text)
        text.color = mTextColor * clickColor;
      if (sprite)
        sprite.color = mSpriteColor * clickColor;
    }
    public void SetOff() {
      if (text)
        text.color = mTextColor * offColor;
      if (sprite)
        sprite.color = mSpriteColor * offColor;
    }
    public void SetOns() {
      if (text)
        text.color = Color.white - (Color.white - mTextColor) * (Color.white - onColor);
      if (sprite)
        sprite.color = Color.white - (Color.white - mSpriteColor) * (Color.white - onColor);

    }
    public void SetClicks() {
      if (text)
        text.color = Color.white - (Color.white - mTextColor) * (Color.white - clickColor);
      if (sprite)
        sprite.color = Color.white - (Color.white - mSpriteColor) * (Color.white - clickColor);
    }
    public void SetOffs() {
      if (text)
        text.color = Color.white - (Color.white - mTextColor) * (Color.white - offColor);
      if (sprite)
        sprite.color = Color.white - (Color.white - mSpriteColor) * (Color.white - offColor);
    }
    public void SetOna() {
      if (text)
        text.color = mTextColor + onColor;
      if (sprite)
        sprite.color = mSpriteColor + onColor;
    }
    public void SetClicka() {
      if (text)
        text.color = mTextColor + clickColor;
      if (sprite)
        sprite.color = mSpriteColor + clickColor;
    }
    public void SetOffa() {
      if (text)
        text.color = mTextColor + offColor;
      if (sprite)
        sprite.color = mSpriteColor + offColor;
    }
  }
}
