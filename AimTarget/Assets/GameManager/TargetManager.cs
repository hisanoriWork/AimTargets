using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
namespace My {
  public class TargetManager : MonoBehaviour {
    public Transform targetPosTransform;
    public float dx, dy;
    public int maxX, maxY;
    public GameObject target;

    public int num = 0;
    /*****private field*****/
    Vector3 mAnker;
    IDictionary<TargetC, Tuple<int, int>> mTargetList;
    /*****event field*****/
    public IObservable<Unit> onTargetBreak { get { return mTargetBreakSubject; } }
    protected Subject<Unit> mTargetBreakSubject = new Subject<Unit>();
    /*****monobehaviour method*****/
    void Awake() {
      mAnker = targetPosTransform.position - new Vector3(maxX * dx / 2f, maxY * dy / 2f, 0f);
      mTargetList = new Dictionary<TargetC, Tuple<int, int>>();
    }

    /*****public method*****/
    public void CreateTarget() {
      while (true) {
        int numX = UnityEngine.Random.Range(0, maxX);
        int numY = UnityEngine.Random.Range(0, maxY);
        if (!SearchTargetPosList(numX, numY)) {
          GameObject obj = Instantiate(target, mAnker + Vector3.right * dx * numX + Vector3.up * dy * numY, Quaternion.identity, targetPosTransform);
          TargetC t = obj.transform.GetComponent<TargetC>();
          t.onBreak.Subscribe(_ => TargetBreak(t));
          mTargetList[t] = new Tuple<int, int>(numX, numY);
          num++;
          break;
        }
      }
    }
    public void RemoveTarget(TargetC target) {
      mTargetList.Remove(target);
      num--;
    }
    public void ClearTarget() {
      foreach (var i in mTargetList.Keys) {
        Destroy(i.gameObject);
      }
      mTargetList.Clear();
      num = 0;
    }
    /*****private method*****/
    TargetC SearchTargetPosList(int numX, int numY) {
      foreach (var i in mTargetList.Keys) {
        Tuple<int, int> pos = mTargetList[i];
        if (pos.Item1 == numX && pos.Item2 == numY) {
          return i;
        }
      }
      return null;
    }
    void TargetBreak(TargetC target) {
      if (mTargetList.ContainsKey(target)) {
        mTargetBreakSubject.OnNext(Unit.Default);
        RemoveTarget(target);
        if (num < 3) CreateTarget();
      }
    }
  }
}