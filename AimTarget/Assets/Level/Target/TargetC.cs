using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class TargetC : MonoBehaviour
{
    public int HP = 100;
    public int maxHP = 100;
    public bool instant = true;
    public IObservable<Unit> onBreak
    { get { return mBreakSubject; } }
    protected Subject<Unit> mBreakSubject = new Subject<Unit>();
    public IObservable<Unit> onDamage{ get { return mDamageSubject; } }
    protected Subject<Unit> mDamageSubject = new Subject<Unit>();
    public void Damage(int i)
    {
        mDamageSubject.OnNext(Unit.Default);
        if (instant)
        {
            Break();
            return;
        }
        HP = Mathf.Clamp(HP - i, 0, maxHP);
        if (HP == 0)
        {
            Break();
            return;
        }
    }
    public void Break()
    {
        mBreakSubject.OnNext(Unit.Default);
        Destroy(gameObject);
    }

    public void Reset()
    {
        HP = maxHP;
    }
}
