using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TargetManager : MonoBehaviour
{
    public Transform targetPosTransform;
    public float dx,dy;
    public int maxX, maxY;
    public GameObject target;
    [System.NonSerialized] public Vector3 anker;
    [System.NonSerialized] public Dictionary<TargetC, Tuple<int, int>> targetList;
    [System.NonSerialized] public int point = 0;
    [System.NonSerialized] public int num = 0;
    void Awake() {
        anker = targetPosTransform.position - new Vector3(maxX * dx / 2f, maxY * dy / 2f, 0f);
        targetList = new Dictionary<TargetC, Tuple<int, int>>();
    }

    void Start()
    {
        CreateTarget();
        CreateTarget();
        CreateTarget();
    }
    void FixedUpdate()
    {
        
    }
    public void CreateTarget() {

        while (true) {
            int numX = UnityEngine.Random.Range(0, maxX);
            int numY = UnityEngine.Random.Range(0, maxY);
            if (!SearchTargetPosList(numX, numY)) {
                GameObject obj = Instantiate(target, anker + Vector3.right * dx * numX + Vector3.up * dy * numY, Quaternion.identity, targetPosTransform);
                //GameObject obj = Instantiate(target, anker, Quaternion.identity, targetPosTransform);
                TargetC t = obj.transform.GetComponent<TargetC>();
                t.onBreak.Subscribe(_ => TargetBreak(t));
                targetList[t] = new Tuple<int, int>(numX, numY);
                num++;
                break;
            }
        }
    }
    TargetC SearchTargetPosList(int numX, int numY) {
        foreach (var i in targetList.Keys)
        {
            Tuple<int, int> pos = targetList[i];
            if (pos.Item1 == numX && pos.Item2 == numY)
            {
                return i;
            }
        }
        return null;
    }

    void TargetBreak(TargetC target) {
        if (targetList.ContainsKey(target))
        {
            targetList.Remove(target);
            point += 100;
            num--;
            if (num < 3) CreateTarget();
        }
    }
    
}
