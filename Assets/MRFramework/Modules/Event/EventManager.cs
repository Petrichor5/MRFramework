namespace MRFramework
{
    [MonoSingletonPath("MRFramework/EventManager")]
    public partial class EventManager : MonoSingleton<EventManager>
    {
        private EventManager() { }

        public bool IsOpenEventLog;

        public override void OnDispose()
        {
            base.OnDispose();
            RemoveAllEventListener();
        }

        public void RemoveAllEventListener()
        {
            RemoveAllStringEventListener();
            RemoveAllTypeEventListener();
        }
    }
}