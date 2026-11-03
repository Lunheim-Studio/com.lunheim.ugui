using System.Collections.Generic;
using UnityEngine.UI;

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

        private bool m_Focused = false;
        public bool focused
        {
            get { return m_Focused; }
        }

        internal void OnFocusedInternal()
        {
            if (m_Focused) return;
            m_Focused = true;
            OnFocused();
        }

        internal void OnUnfocusedInternal()
        {
            if (!m_Focused) return;
            m_Focused = false;
            OnUnfocused();
        }

        protected virtual void OnFocused() { }
        protected virtual void OnUnfocused() { }

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
        private List<SelectionRule> m_SelectionRules = new();
        public List<SelectionRule> selectionRules
        {
            get { return m_SelectionRules; }
        }

        private GameObject m_LastSelected;
        public GameObject lastSelected
        {
            get { return m_LastSelected; }
            set { m_LastSelected = value; }
        }

        protected virtual GameObject GetDefaultSelection()
        {
            return null;
        }

        public virtual GameObject GetSelection()
        {
            if (!IsInteractable())
                return null;

            if (m_RememberSelection && IsValidSelection(m_LastSelected))
                return m_LastSelected;

            var selection = GetDefaultSelection();
            if (IsValidSelection(selection))
                return selection;

            for (int i = 0; i < m_SelectionRules.Count; i++)
            {
                selection = ResolveRule(m_SelectionRules[i]);
                if (selection)
                    return selection;
            }

            return null;
        }

        protected virtual bool IsValidSelection(GameObject target)
        {
            if (!target || !target.activeInHierarchy)
                return false;

            if (!target.TryGetComponent<Selectable>(out var selectable))
                return false;

            return selectable.IsActive()
                && selectable.IsInteractable();
        }

        private static readonly List<Selectable> s_SelectableCache = new();

        private GameObject ResolveRule(SelectionRule rule)
        {
            var target = rule.target;
            if (!target)
                return null;

            switch (rule.type)
            {
                case SelectionRule.Type.Select:
                    return IsValidSelection(target) ? target : null;
                case SelectionRule.Type.FirstIn:
                    return FindSelectionIn(target, false);
                case SelectionRule.Type.LastIn:
                    return FindSelectionIn(target, true);
                default:
                    return null;
            }
        }

        private GameObject FindSelectionIn(GameObject root, bool last)
        {
            if (!root.activeInHierarchy)
                return null;

            root.GetComponentsInChildren(false, s_SelectableCache);

            GameObject selection = null;
            int count = s_SelectableCache.Count;
            for (int i = 0; i < count; i++)
            {
                var candidate = s_SelectableCache[last ? count - 1 - i : i].gameObject;
                if (IsValidSelection(candidate))
                {
                    selection = candidate;
                    break;
                }
            }

            s_SelectableCache.Clear();
            return selection;
        }
    }
}