namespace MRFramework
{
    public interface IUnRegister
    {
        void UnRegister();
    }

    public static class UnRegisterExtension
    {
        public static IUnRegister AutoUnRegister(this IUnRegister unRegister, UnityEngine.GameObject gameObject)
        {
            var trigger = gameObject.GetComponent<UnRegisterOnDestroyTrigger>();

            if (!trigger)
            {
                trigger = gameObject.AddComponent<UnRegisterOnDestroyTrigger>();
            }

            trigger.AddUnRegister(unRegister);

            return unRegister;
        }

        public static IUnRegister AutoUnRegister<T>(this IUnRegister self, T component) where T : UnityEngine.Component
        {
            return self.AutoUnRegister(component.gameObject);
        }
    }
}