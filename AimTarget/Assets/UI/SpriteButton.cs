using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SpriteButton : MonoBehaviour
{
    public enum BlendMode
    {
        EAdd,
        EDiff,
        EScrean,
    }
    public SpriteRenderer sprite;
    public TextMesh text;
    public Collider col;
    public MousePointee mousepointee;
    public BlendMode blendMode = BlendMode.EDiff;
    Color textColor;
    Color spriteColor;
    public Color onColor = Color.white;
    public Color clickColor = Color.white;
    public Color offColor = Color.white;
    

    void Awake() {
        if(text)
            textColor = text.color;
        if (sprite)
            spriteColor = sprite.color;
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

    public void SetActive(bool f)
    {
        if (sprite)
            sprite.gameObject.SetActive(f);
        if (text)
            text.gameObject.SetActive(f);
        if(col)
            col.enabled = f;
    }

    public void SetOn(){
        if (text)
            text.color = textColor * onColor;
        if (sprite)
            sprite.color = spriteColor * onColor;
    }
    public void SetClick()
    {
        if (text)
            text.color = textColor * clickColor;
        if (sprite)
            sprite.color = spriteColor * clickColor;
    }

    public void SetOff()
    {
        if (text)
            text.color = textColor * offColor;
        if (sprite)
            sprite.color = spriteColor * offColor;
    }

    public void SetOns()
    {
        if(text)
            text.color = Color.white - (Color.white - textColor) * (Color.white - onColor);
        if(sprite)
            sprite.color = Color.white - (Color.white - spriteColor) * (Color.white - onColor);

    }

    public void SetClicks()
    {
        if (text)
            text.color = Color.white - (Color.white - textColor) * (Color.white - clickColor);
        if (sprite)
            sprite.color = Color.white - (Color.white - spriteColor) * (Color.white - clickColor);

    }

    public void SetOffs()
    {
        if (text)
            text.color = Color.white - (Color.white - textColor) * (Color.white - offColor);
        if (sprite)
            sprite.color = Color.white - (Color.white - spriteColor) * (Color.white - offColor);

    }

    public void SetOna()
    {
        if (text)
            text.color = textColor + onColor;
        if (sprite)
            sprite.color = spriteColor + onColor;
    }
    public void SetClicka()
    {
        if (text)
            text.color = textColor + clickColor;
        if (sprite)
            sprite.color = spriteColor + clickColor;
    }

    public void SetOffa()
    {
        if (text)
            text.color = textColor + offColor;
        if (sprite)
            sprite.color = spriteColor + offColor;
    }
}
