using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    private Dictionary<string, UnityEvent> _events;

    private static EventManager _eventManager;

    public static EventManager Instance
    {
        get
        {
            if (_eventManager) return _eventManager;
            _eventManager = FindObjectOfType<EventManager>();
            if (!_eventManager)
            {
                Debug.LogError("No active EventManager script");
                return null;
            }
            _eventManager.Init();
            return _eventManager;
        }
    }

    private void Init()
    {
        if (_events == null)
        {
            _events = new Dictionary<string, UnityEvent>();
        }
    }

    private static void Register(string name, UnityAction listener)
    {
        UnityEvent @event;
        if (Instance._events.TryGetValue(name, out @event))
        {
            @event.AddListener(listener);
        }
        else
        {
            @event = new UnityEvent();
            @event.AddListener(listener);
            Instance._events.Add(name, @event);
        }
    }

    public static void Unregister(string name, UnityAction listener)
    {
        UnityEvent @event;
        if (Instance._events.TryGetValue(name, out @event))
        {
            @event.RemoveListener(listener);
        }
    }

    public static void Trigger(string name)
    {
        UnityEvent @event;
        if (Instance._events.TryGetValue(name, out @event))
        {
            @event.Invoke();
        }
    }
}