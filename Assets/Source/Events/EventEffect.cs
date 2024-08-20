using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventEffect : Event
{
    public bool performOnReload = true;
    public abstract void Perform();
}
