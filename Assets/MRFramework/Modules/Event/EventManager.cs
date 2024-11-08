namespace MRFramework
{
    [MonoSingletonPath("MRFramework/EventManager")]
    public partial class EventManager : MonoSingleton<EventManager>
    {
        private EventManager() { }

        public bool IsOpenEventLog;

        private void OnDestroy()
        {
            RemoveAllEventListener();
        }

        public void RemoveAllEventListener()
        {
            RemoveAllStringEventListener();
            RemoveAllTypeEventListener();
        }
    }
}