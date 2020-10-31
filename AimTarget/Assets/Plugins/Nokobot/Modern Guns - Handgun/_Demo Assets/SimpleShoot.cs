using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot")]
public class SimpleShoot : MonoBehaviour {
  [Header("Prefab Refrences")]
  public GameObject bulletPrefab;
  public GameObject casingPrefab;
  public GameObject muzzleFlashPrefab;
  [Header("Location Refrences")]
  [SerializeField] private Animator gunAnimator;
  [SerializeField] private Transform barrelLocation;
  [SerializeField] private Transform casingExitLocation;
  [Header("Settings")]
  [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 2f;
  [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 500f;
  [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 150f;
  void Start() {
    if (barrelLocation == null)
      barrelLocation = transform;
    if (gunAnimator == null)
      gunAnimator = GetComponentInChildren<Animator>();
  }
  public void ToShootAnimation(){
    gunAnimator.SetTrigger("Fire");
  }
  void Shoot() {
    if (muzzleFlashPrefab) {
      GameObject tempFlash;//Create the muzzle flash
      tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);
      Destroy(tempFlash, destroyTimer);
    }
    //if (!bulletPrefab) { return; }
    //Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation).GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);
  }
  //This function creates a casing at the ejection slot
  void CasingRelease() {
    //Cancels function if ejection slot hasn't been set or there's no casing
    if (!casingExitLocation || !casingPrefab) { return; }
    GameObject tempCasing;
    tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
    tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
    tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);
    Destroy(tempCasing, destroyTimer);
  }
}
