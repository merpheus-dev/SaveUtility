using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Subtegral.SaveUtility
{
    public class BinaryFormatterSaveService<T> : SaveService<T> where T : new()
    {
        private string _filePath;

        private void OnEnable()
        {
            _filePath = Path.Combine(Application.persistentDataPath, $"{nameof(T)}.pak");
        }

        public override T LoadData()
        {
            if (!File.Exists(_filePath)) return new T();
            var binaryFormatter = new BinaryFormatter();
            var file = File.Open(_filePath, FileMode.Open);
            var deserializedData = (T) binaryFormatter.Deserialize(file);
            file.Close();
            return deserializedData;
        }

        public override void SaveData(T data)
        {
            var binaryFormatter = new BinaryFormatter();
            var fileStream = File.Create(_filePath);
            binaryFormatter.Serialize(fileStream, data);
            fileStream.Close();
        }
    }
}