using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SpriteButton : MonoBehaviour
{
    public SpriteRenderer sprite;
    public TextMesh text;
    public Collider col;
    public Color onColor;
    public Color downColor;
    public Color offColor;
    public void ChangeSpriteColor()
    {
        sprite.color = onColor;
    }

    public void ChangeTextColor()
    {
        text.color = onColor;
    }

    public void SetActive(bool f)
    {
        sprite.gameObject.SetActive(f);
        text.gameObject.SetActive(f);
        col.enabled = f;
    }

    public void Change(Color color)
    {
        sprite.color = color;
    }

}
