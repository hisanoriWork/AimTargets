using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace My {
  public class MousePointee : MonoBehaviour {
    public UnityEvent onEvent;
    public UnityEvent downEvent;
    public UnityEvent upEvent;
    public UnityEvent clickEvent;
    public UnityEvent offEvent;
  }
}
