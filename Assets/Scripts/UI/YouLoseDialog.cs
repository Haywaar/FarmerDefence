using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YouLoseDialog : MonoBehaviour, IWindow
{
   public void OnTryAgainClicked()
   {
      //TODO - many levels
      LevelManager.LoadMain();
   }
   
   public void OnExitClicked()
   {
      Application.Quit();
   }
}
