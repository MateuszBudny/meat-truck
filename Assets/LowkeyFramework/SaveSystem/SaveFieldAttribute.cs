using System;

namespace LowkeyFramework.AttributeSaveSystem
{
    // suggestion: renaming this to SaveMemeber, because it is now possible to save fields and properties also. but I think, that SaveField sounds better than SaveMember xd oh, and "member" might also mean "method", but we do not operate on methods.
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SaveFieldAttribute : Attribute { }
}