using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YouLoseDialog : MonoBehaviour, IWindow
{
   public void OnTryAgainClicked()
   {
      //TODO - add option to switch level
      LevelManager.LoadMain();
   }
   
   public void OnExitClicked()
   {
      Application.Quit();
   }
}
