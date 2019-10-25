namespace Subtegral.SaveUtility.CodeGen
{
    public struct ScriptData
    {
        public string ServiceName;
        public string DataClassName;
        public string NameSpaceName;
        public bool DoesNameSpaceExists => !string.IsNullOrEmpty(NameSpaceName);
    }
}