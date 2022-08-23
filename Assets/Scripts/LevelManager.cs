using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
   public static void LoadMain()
   {
      SceneManager.LoadScene("Main", LoadSceneMode.Single);
   }

   public static void LoadMenu()
   {
      SceneManager.LoadScene("TitleScene", LoadSceneMode.Single);
   }
}
