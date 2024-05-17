using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
  private Collider _collider;

  private void Awake()
  {
    _collider = GetComponent<Collider>();
  }

  public void OnPickUp()
  {
    _collider.enabled = false;
  }

  public void OnPlace()
  {
    _collider.enabled = true;
  }
}
