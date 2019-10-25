# SaveUtility
Modular Save System for Unity

# Example
- Go to Top Menu Subtegral>SaveUtility>Generate to manually generate wrapper scripts.
- Use Subtegral>SaveUtility>Auto Generate to enable auto wrapper code generation.
- Create a scriptable object from generated wrappers from Create Menu>Save Utility>Your Data Class' Wrapper
- Use it in your class:
```C#
public class MyAwesomeSaveManager
{
      [SerializeField] private MyPotatoCountWrapper serviceWrapper;
      
      //Load data, returns a new instance with default values of your serialized class
      _data = serviceWrapper.LoadData();
      
      //Saves data
      serviceWrapper.SaveData(_data);

}
```

# TO-DO:
- Default components
