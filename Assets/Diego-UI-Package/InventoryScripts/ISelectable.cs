using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Diego
{
    public interface ISelectable
    {
        public bool CanBeCollected { get; }
        public bool CanBeInteractedWith { get; }
        public int ID { get; }
        public Sprite Sprite { get; }
    }
}