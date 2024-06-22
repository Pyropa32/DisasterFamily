using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Diego
{
    public interface Interactable : Selectable
    {
        public Action<Item> getAction();
    }
}