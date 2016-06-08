using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Taken from Ben Dangelo (gist @ github)
namespace slipperyeel
{
    public delegate void EventDelegate<T>(T e) where T : SEGameEvent;

    public class SEEventManager : Singleton<SEEventManager>
    {
        [SerializeField]
        private bool mLimitQueueProcTime = false;
        [SerializeField]
        private float mQueueProcTime = 0.0f;

        private Queue mEventQueue = new Queue();

        private delegate void EventDelegate(SEGameEvent sEGE);

        private Dictionary<System.Type, EventDelegate> delegates = new Dictionary<System.Type, EventDelegate>();
        private Dictionary<System.Delegate, EventDelegate> delegateLookup = new Dictionary<System.Delegate, EventDelegate>();
        private Dictionary<System.Delegate, System.Delegate> onceLookups = new Dictionary<System.Delegate, System.Delegate>();

        private EventDelegate AddDelegate<T>(EventDelegate<T> seDel) where T : SEGameEvent
        {
            if(delegateLookup.ContainsKey(seDel))
            {
                return null;
            }

            // Generate an internal delegate using the event 
            EventDelegate internalDelegate = (seGameEvent) => seDel((T)seGameEvent);
            delegateLookup[seDel] = internalDelegate;

            EventDelegate tempDel;
            if (delegates.TryGetValue(typeof(T), out tempDel))
            {
                delegates[typeof(T)] = tempDel += internalDelegate;
            }
            else
            {
                delegates[typeof(T)] = internalDelegate;
            }

            return internalDelegate;
        }

        public void AddListener<T>(EventDelegate<T> seDel) where T : SEGameEvent
        {
            AddDelegate<T>(seDel);
        }

        public void AddListenerOnce<T>(EventDelegate<T> seDel) where T : SEGameEvent
        {
            EventDelegate result = AddDelegate<T>(seDel);

            if (result != null)
            {
                // remember this is only called once
                onceLookups[result] = seDel;
            }
        }

        public void RemoveListener<T>(EventDelegate<T> seDel) where T : SEGameEvent
        {
            EventDelegate internalDelegate;
            if (delegateLookup.TryGetValue(seDel, out internalDelegate))
            {
                EventDelegate tempDel;
                if (delegates.TryGetValue(typeof(T), out tempDel))
                {
                    tempDel -= internalDelegate;
                    if (tempDel == null)
                    {
                        delegates.Remove(typeof(T));
                    }
                    else {
                        delegates[typeof(T)] = tempDel;
                    }
                }

                delegateLookup.Remove(seDel);
            }
        }

        public void RemoveAll()
        {
            delegates.Clear();
            delegateLookup.Clear();
            onceLookups.Clear();
        }

        public bool HasListener<T>(EventDelegate<T> seDel) where T : SEGameEvent
        {
            return delegateLookup.ContainsKey(seDel);
        }

        public void TriggerEvent(SEGameEvent seGameEvent)
        {
            EventDelegate del;
            if (delegates.TryGetValue(seGameEvent.GetType(), out del))
            {
                del.Invoke(seGameEvent);

                // remove listeners which should only be called once
                foreach (EventDelegate k in delegates[seGameEvent.GetType()].GetInvocationList())
                {
                    if (onceLookups.ContainsKey(k))
                    {
                        delegates[seGameEvent.GetType()] -= k;

                        if (delegates[seGameEvent.GetType()] == null)
                        {
                            delegates.Remove(seGameEvent.GetType());
                        }

                        delegateLookup.Remove(onceLookups[k]);
                        onceLookups.Remove(k);
                    }
                }
            }
            else {
                Debug.LogWarning("Event: " + seGameEvent.GetType() + " has no listeners");
            }
        }

        public bool QueueEvent(SEGameEvent evt)
        {
            if (!delegates.ContainsKey(evt.GetType()))
            {
                Debug.LogWarning("EventManager: QueueEvent failed due to no listeners for event: " + evt.GetType());
                return false;
            }

            mEventQueue.Enqueue(evt);
            return true;
        }

        void Update()
        {
            float timer = 0.0f;
            while (mEventQueue.Count > 0)
            {
                if (mLimitQueueProcTime)
                {
                    if (timer > mQueueProcTime)
                        return;
                }

                SEGameEvent evt = mEventQueue.Dequeue() as SEGameEvent;
                TriggerEvent(evt);

                if (mLimitQueueProcTime)
                    timer += Time.deltaTime;
            }
        }
    }
}