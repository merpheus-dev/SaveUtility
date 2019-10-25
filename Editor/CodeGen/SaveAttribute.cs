using System;

namespace Subtegral.SaveUtility.CodeGen
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SaveAttribute : Attribute
    {
        public SaveServiceType saveService;
        public SaveAttribute(SaveServiceType saveService)
        {
            this.saveService = saveService;
        }
    }
}