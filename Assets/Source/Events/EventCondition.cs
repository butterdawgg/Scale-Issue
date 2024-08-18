using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventCondition : MonoBehaviour
{
    public abstract void Set();
    public abstract bool Check();
}
