using System;
using System.Collections;
using System.Collections.Generic;
using Towers;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  private GameManager()
  {
  }

  public static GameManager Instance;

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
    else
    {
      Debug.LogError("Duplicate detected");
      Destroy(this);
    }
  }
}
