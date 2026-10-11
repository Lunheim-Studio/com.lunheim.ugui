using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine.EventSystems
{
    [DisallowMultipleComponent]
    [AddComponentMenu("UI (Canvas)/Navigation/Navigation Group")]
    public class NavigationGroup : MonoBehaviour
    {
        [SerializeField]
        private int m_Layer;
        public int layer => m_Layer;

        public virtual bool IsNavigable(NavigationGroup other)
        {
            return other
                && other.isActiveAndEnabled
                && other.layer == layer;
        }

        [SerializeField]
        private Selectable m_EntryOverride;
        public Selectable entryOverride
        {
            get => m_EntryOverride;
            set => m_EntryOverride = value;
        }

        public virtual Selectable ResolveEntry(Selectable candidate)
        {
            if (m_EntryOverride
                && m_EntryOverride.IsActive()
                && m_EntryOverride.IsInteractable()
                && m_EntryOverride.navigation.mode != Navigation.Mode.None
                && m_EntryOverride.navigationGroup == this)
            {
                return m_EntryOverride;
            }

            return candidate;
        }
    }
}