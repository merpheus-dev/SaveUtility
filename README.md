# SaveUtility
Modular Save System for Unity

# Example
- Create a wrapper around save service
```C#
[CreateAssetMenu]
public class BinaryFormatterServiceWrapper : BinaryFormatterSaveService<UserData>{}
```
- Create a scriptable object file from the wrapper
- Use it in your class:
```C#
public class MyAwesomeSaveManager
{
      [SerializeField] private BinaryFormatterServiceWrapper serviceWrapper;
      
      //Load data, returns a new instance with default values of your serialized class
      _data = serviceWrapper.LoadData();
      
      //Saves data
      serviceWrapper.SaveData(_data);

}
```


# TO-DO:
- Code generators for serialized types
- Default components
