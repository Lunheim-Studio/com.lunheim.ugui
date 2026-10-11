namespace UnityEngine.EventSystems
{
    public abstract class EventBehaviour : MonoBehaviour
    {
        private EventSystem m_EventSystem;

        protected virtual void OnEnable()
        {
            m_EventSystem = EventSystem.current;
            if (m_EventSystem)
                m_EventSystem.composition.Register(this);
        }

        protected virtual void OnDisable()
        {
            if (m_EventSystem)
                m_EventSystem.composition.Unregister(this);

            m_EventSystem = null;
        }

        public abstract bool IsInteractable();

        [SerializeField]
        private int m_SortOrder = 0;
        public int sortOrder
        {
            get { return m_SortOrder; }
        }

        [SerializeField]
        private bool m_RememberSelection = true;
        public bool rememberSelection
        {
            get { return m_RememberSelection; }
            set { m_RememberSelection = value; }
        }

        [SerializeField]
        private GameObject m_FallbackSelection;
        public GameObject fallbackSelection
        {
            get { return m_FallbackSelection; }
            set { m_FallbackSelection = value; }
        }

        private GameObject m_LastSelected;
        public GameObject lastSelected
        {
            get { return m_LastSelected; }
            set { m_LastSelected = value; }
        }

        protected virtual GameObject GetDefaultSelection()
        {
            return m_FallbackSelection;
        }

        public abstract GameObject GetSelection();
    }
}