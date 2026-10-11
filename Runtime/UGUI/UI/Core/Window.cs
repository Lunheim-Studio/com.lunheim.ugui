using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI (Canvas)/Window")]
    public class Window : EventBehaviour
    {
        private readonly List<CanvasGroup> m_CanvasGroupCache = new();

        private bool m_GroupsAllowInteraction = true;

        public override bool IsInteractable()
        {
            return m_GroupsAllowInteraction;
        }

        public override GameObject GetSelection()
        {
            if (!IsInteractable())
                return null;

            if (rememberSelection && IsValidSelection(lastSelected))
                return lastSelected;

            var selection = GetDefaultSelection();

            if (IsValidSelection(selection))
                return selection;

            return IsValidSelection(fallbackSelection)
                ? fallbackSelection
                : null;
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

        protected override void OnEnable()
        {
            m_GroupsAllowInteraction = ParentGroupAllowsInteraction();
            base.OnEnable();
        }

        private void OnCanvasGroupChanged()
        {
            m_GroupsAllowInteraction = ParentGroupAllowsInteraction();
        }

        private void OnTransformParentChanged()
        {
            m_GroupsAllowInteraction = ParentGroupAllowsInteraction();
        }

        private bool ParentGroupAllowsInteraction()
        {
            Transform t = transform;

            while (t != null)
            {
                t.GetComponents(m_CanvasGroupCache);

                foreach (var group in m_CanvasGroupCache)
                {
                    if (!group.enabled)
                        continue;

                    if (!group.interactable)
                        return false;

                    if (group.ignoreParentGroups)
                        return true;
                }

                t = t.parent;
            }

            return true;
        }
    }
}