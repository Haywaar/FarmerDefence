using System;
using System.Collections.Generic;
using UnityEngine;

public interface IWindow
{
}

public class DialogManager
{
    // При создании новых окон добавлять их сюда
    private static readonly Dictionary<Type, string> PrefabsDictionary = new Dictionary<Type, string>()
    {
        {typeof(SettingsDialog), "SettingsDialog"},
        {typeof(MenuDialog),"MenuDialog"},
        {typeof(YouLoseDialog),"YouLoseDialog"},
    };
    
    public static void ShowWindow<T>() where T : IWindow
    {
        // GetPrefabFromFactory
       var go = GetPrefabByType<T>();
       if (go == null)
       {
           Debug.LogError("Show window - object not found");
           return;
       }

       GameObject.Instantiate(go, GuiHolder);
    }

    private static GameObject GetPrefabByType<T>() where T : IWindow
    {
        var prefabName =  PrefabsDictionary[typeof(T)];
        if (string.IsNullOrEmpty(prefabName))
        {
            Debug.LogError("cant find prefab type of " + typeof(T) + "Do you added it in PrefabsDictionary?");
        }

        var path = "Prefabs/Dialogs/" + PrefabsDictionary[typeof(T)];
        var windowGO = Resources.Load<GameObject>(path);
        if (windowGO == null)
        {
            Debug.LogError("Cant find prefab at path " + path);
        }

        return windowGO;
    }
    
    //TODO - прокинуть в сервис локатор?
    public static Transform GuiHolder
    {
        get { return GameObject.Find("MainCanvas").transform; }
    }
}
