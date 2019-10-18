using System;
using UnityEngine;

//TO-DO: Implement Codegen for serializable types
//Use "[SaveObject]" attribute to find classes that gonna receive a wrapper class codegen
namespace Subtegral.SaveUtility
{
    [Serializable]
    public abstract class SaveService<T> : ScriptableObject
    {
        public abstract T LoadData();
        public abstract void SaveData(T data);
    }
}