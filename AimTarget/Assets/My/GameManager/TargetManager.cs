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
    public GameObject flickTarget;
    public GameObject trackingTarget;

    public int num = 0;
    /*****private field*****/
    Vector3 mAnker;
    List<TargetC> mTargetList;
    IDictionary<TargetC, Tuple<int, int>> mTargetList2;
    /*****event field*****/
    public IObservable<Unit> onTargetDamege { get { return mTargetDamegeSubject; } }
    protected Subject<Unit> mTargetDamegeSubject = new Subject<Unit>();
    public IObservable<Unit> onTargetBreak { get { return mTargetBreakSubject; } }
    protected Subject<Unit> mTargetBreakSubject = new Subject<Unit>();
    public IObservable<Unit> onTargetDestroy { get { return mTargetDestroySubject; } }
    protected Subject<Unit> mTargetDestroySubject = new Subject<Unit>();
    public IObservable<TargetC> onTargetAwake { get { return mTargetAwakeSubject; } }
    protected Subject<TargetC> mTargetAwakeSubject = new Subject<TargetC>();
    /*****monobehaviour method*****/
    void Awake() {
      mAnker = targetPosTransform.position - new Vector3(maxX * dx / 2f, maxY * dy / 2f, 0f);
      mTargetList = new List<TargetC>();
      mTargetList2 = new Dictionary<TargetC, Tuple<int, int>>();
    }
    /*****public method*****/
    public void Create3thTarget(){
      while (true) {
        int numX = UnityEngine.Random.Range(0, maxX);
        int numY = UnityEngine.Random.Range(0, maxY);
        if (!SearchTargetPosList(numX, numY)){
          TargetC t = CreateTarget(target,mAnker + Vector3.right * dx * numX + Vector3.up * dy * numY);
          mTargetList2[t] = new Tuple<int, int>(numX, numY);
          break;
        }
      }
    }
    public void CreateFlickTarget() {
      float ddx = UnityEngine.Random.Range(0, dx * maxX);
      float ddy = UnityEngine.Random.Range(0, dy * maxY);
      Vector3 pos = mAnker + Vector3.right * ddx + Vector3.up * ddy;
      TargetC t = CreateTarget(flickTarget,pos);
      mTargetList.Add(t);
    }
    public void CreateTrackingTarget() {
      float ddx = UnityEngine.Random.Range(0, dx * maxX);
      float ddy = UnityEngine.Random.Range(0, dy * maxY);
      Vector3 pos = mAnker + Vector3.right * ddx + Vector3.up * ddy;
      TargetC t = CreateTarget(trackingTarget, pos);
      mTargetList.Add(t);
    }
    public TargetC CreateTarget(GameObject prefab,Vector3 pos){
      GameObject obj = Instantiate(prefab,pos, Quaternion.identity, targetPosTransform);
      TargetC t = obj.transform.GetComponent<TargetC>();
      mTargetAwakeSubject.OnNext(t);
      t.onBreak.Subscribe(_ => TargetBreak());
      t.onDestroy.Subscribe(_ => TargetDestroy(t));
      t.onDamage.Subscribe(_ => TargetDamege());
      num++;
      return t;
    }
    public void RemoveTarget(TargetC target) {
      if (mTargetList2.ContainsKey(target)) {
        mTargetList2.Remove(target);
        num--;
      }
      int i = mTargetList.IndexOf(target);
      if (i >= 0){
        mTargetList.RemoveAt(i);
        num--;
      }
    }
    public void ClearTarget() {
      foreach (var i in mTargetList2.Keys) {
        Destroy(i.gameObject);
      }
      foreach (var i in mTargetList) {
        Destroy(i.gameObject);
      }
      mTargetList.Clear();
      mTargetList2.Clear();
      num = 0;
    }
    /*****private method*****/
    TargetC SearchTargetPosList(int numX, int numY) {
      foreach (var i in mTargetList2.Keys) {
        Tuple<int, int> pos = mTargetList2[i];
        if (pos.Item1 == numX && pos.Item2 == numY) {
          return i;
        }
      }
      return null;
    }
    void TargetBreak() {
      mTargetBreakSubject.OnNext(Unit.Default);
    }
    void TargetDestroy(TargetC target) {
      mTargetDestroySubject.OnNext(Unit.Default);
      RemoveTarget(target);
    }

    void TargetDamege() {
      mTargetDamegeSubject.OnNext(Unit.Default);
    }
  }
}