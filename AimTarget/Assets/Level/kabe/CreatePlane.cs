using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlane : MonoBehaviour
{
    public GameObject prefab;
    public float num;
    public float dx, dz;
    void Awake(){
        Vector3 pos = new Vector3(-num * dx/2f,0f,-num * dz/2f);

        for (int i = 0; i < num; i++) {
            for (int j = 0; j < num; j++) {
                GameObject obj = Instantiate(prefab, pos + Vector3.forward * dz * i + Vector3.right * dx * j, Quaternion.identity,transform);
            }
        }
    }
}
