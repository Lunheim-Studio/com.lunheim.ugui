using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.EventSystems
{
    public sealed class EventSystemComposition
    {
        private readonly List<EventBehaviour> _elements = new();
        private readonly Dictionary<EventBehaviour, int> _order = new();

        private int _orderCounter;
        private bool _isRefreshing;

        public void Register(EventBehaviour element)
        {
            if (element == null) return;
            if (_order.ContainsKey(element)) return;

            _elements.Add(element);
            _order[element] = ++_orderCounter;

            Refresh();
        }

        public void Unregister(EventBehaviour element)
        {
            if (element == null) return;
            if (!_order.Remove(element)) return;

            _elements.Remove(element);

            Refresh();
        }

        private EventBehaviour SortTopmost()
        {
            EventBehaviour best = null;
            int bestPriority = int.MinValue;
            int bestOrder = int.MinValue;

            for (int i = 0; i < _elements.Count; i++)
            {
                if (_elements[i] is not EventBehaviour element) continue;
                if (element == null) continue;

                int priority = element.sortOrder;
                int order = _order.TryGetValue(element, out var value)
                    ? value
                    : 0;

                if (best == null ||
                    priority > bestPriority ||
                    (priority == bestPriority && order > bestOrder))
                {
                    best = element;
                    bestPriority = priority;
                    bestOrder = order;
                }
            }

            return best;
        }

        private void Refresh()
        {
            // Focus callbacks may register or unregister views synchronously.
            if (_isRefreshing)
                return;

            _isRefreshing = true;

            var notifiedView = m_Focused;

            try
            {
                while (true)
                {
                    var nextView = SortTopmost();

                    if (!ReferenceEquals(m_Focused, nextView))
                    {
                        var previousView = m_Focused;

                        // Both properties describe the same focus state in callbacks.
                        m_Focused = nextView;

                        /*if (previousView != null)
                            previousView.OnUnfocusedInternal();*/

                        // OnUnfocused may have removed or replaced the incoming view.
                        m_Focused = SortTopmost();

                        /*if (m_Focused != null)
                            m_Focused.OnFocusedInternal();*/

                        continue;
                    }

                    if (!ReferenceEquals(notifiedView, m_Focused))
                    {
                        notifiedView = m_Focused;
                        focusedChanged?.Invoke(m_Focused);
                        continue;
                    }

                    break;
                }
            }
            finally
            {
                _isRefreshing = false;
            }
        }

        private EventBehaviour m_Focused;
        public EventBehaviour focused
        {
            get { return m_Focused; }
        }

        public event Action<EventBehaviour> focusedChanged;
    }
}