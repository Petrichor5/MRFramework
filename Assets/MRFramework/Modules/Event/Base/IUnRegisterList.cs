using System.Collections.Generic;

namespace MRFramework
{
    public interface IUnRegisterList
    {
        List<IUnRegister> UnRegisterList { get; }
    }

    public static class IUnRegisterListExtension
    {
        public static void AddToUnregisterList(this IUnRegister self, IUnRegisterList unRegisterList)
        {
            unRegisterList.UnRegisterList.Add(self);
        }

        public static void UnRegisterAll(this IUnRegisterList self)
        {
            foreach (var unRegister in self.UnRegisterList)
            {
                unRegister.UnRegister();
            }

            self.UnRegisterList.Clear();
        }
    }
}