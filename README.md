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

# Installation
You can either clone the project and put it inside the "Packages" folder of your project or add it to your *package.json* file as follows:
```json
{
  "dependencies": {
       "com.m3rt32.saveutility": "https://github.com/m3rt32/SaveUtility.git"
  }
}
```
# TO-DO:
- Default components
