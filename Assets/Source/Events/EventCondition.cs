using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventCondition : Event
{
    public abstract void Set();
    public abstract bool Check();
}
