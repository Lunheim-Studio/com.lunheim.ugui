using System;

namespace UnityEngine.EventSystems
{
    [Serializable]
    public struct SelectionRule
    {
        public enum Type
        {
            Select,
            FirstIn,
            LastIn,
        }

        [SerializeField]
        private Type m_Type;
        public Type type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }

        [SerializeField]
        private GameObject m_Target;
        public GameObject target
        {
            get { return m_Target; }
            set { m_Target = value; }
        }

        public SelectionRule(Type type, GameObject target)
        {
            m_Type = type;
            m_Target = target;
        }
    }
}
