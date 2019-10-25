using System;
using UnityEngine;

namespace Subtegral.SaveUtility
{
    [Serializable]
    public class PlayerPrefsSaveService<T> : SaveService<T> where T:new()
    {
        private const string SaveKey = "UserSave";

        public override T LoadData()
        {
            var loadedData = PlayerPrefs.HasKey(SaveKey) ? PlayerPrefs.GetString(SaveKey) : string.Empty;
            var data = string.IsNullOrEmpty(loadedData) ? new T() : JsonUtility.FromJson<T>(loadedData);
            return data;
        }

        public override void SaveData(T data)
        {
            var jsonString = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(SaveKey,jsonString);
            PlayerPrefs.Save();
        }
    }
}