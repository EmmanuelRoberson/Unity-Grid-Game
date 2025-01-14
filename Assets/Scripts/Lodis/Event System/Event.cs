﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lodis
{
    [CreateAssetMenu(menuName = "Event")]
    public class Event : ScriptableObject
    {
        //All listeners for the event
        private List<IListener> listeners = new List<IListener>();
        //Adds a listener to the event
        public void AddListener(IListener newListener)
        {
            listeners.Add(newListener);
        }
        //Raises the event with the gameobject information
        public void Raise(GameObject sender)
        {
            foreach(IListener listener in listeners)
            {
                listener.Invoke(sender);
            }
        }
        //Raises the game event with no information about who sent it
        public void Raise()
        {
            foreach (IListener listener in listeners)
            {
                listener.Invoke(null);
            }
        }
    }
}
