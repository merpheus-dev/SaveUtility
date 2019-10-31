using System;
using UnityEngine;

namespace Subtegral.SaveUtility
{
    [Serializable]
    public class PlayerPrefsSaveService<T> : SaveService<T> where T:new()
    {

        public override T LoadData()
        {
            var loadedData = PlayerPrefs.HasKey(nameof(T)) ? PlayerPrefs.GetString(nameof(T)) : string.Empty;
            var data = string.IsNullOrEmpty(loadedData) ? new T() : JsonUtility.FromJson<T>(loadedData);
            return data;
        }

        public override void SaveData(T data)
        {
            var jsonString = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(nameof(T),jsonString);
            PlayerPrefs.Save();
        }
    }
}