namespace MRFramework
{
    public partial class EventManager : Singleton<EventManager>
    {
        private EventManager() { }

        public bool IsOpenEventLog;

        public void RemoveAllEventListener()
        {
            RemoveAllStringEventListener();
            RemoveAllTypeEventListener();
        }
    }
}