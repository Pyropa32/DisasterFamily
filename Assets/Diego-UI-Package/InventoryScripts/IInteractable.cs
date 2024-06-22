using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Diego
{
    public interface IInteractable : ISelectable
    {
        public Action<Item> OnInteract { get; set; }
    }
}