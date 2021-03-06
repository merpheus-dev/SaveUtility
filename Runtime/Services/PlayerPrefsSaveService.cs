﻿using System;
using UnityEngine;

namespace Subtegral.SaveUtility
{
    [Serializable]
    public class PlayerPrefsSaveService<T> : SaveService<T> where T:new()
    {

        public override T LoadData()
        {
            var loadedData = PlayerPrefs.HasKey(typeof(T).Name) ? PlayerPrefs.GetString(typeof(T).Name) : string.Empty;
            var data = string.IsNullOrEmpty(loadedData) ? new T() : JsonUtility.FromJson<T>(loadedData);
            return data;
        }

        public override void SaveData(T data)
        {
            var jsonString = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(typeof(T).Name,jsonString);
            PlayerPrefs.Save();
        }
    }
}