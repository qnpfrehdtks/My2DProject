using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolingObjectBase : MonoBehaviour, IPoolingObject
{
    public int StartCount { get; set; } = 20;
    public abstract void Initialize(GameObject factory);

    public abstract void OnPushToQueue();

    public abstract void OnPopFromQueue();
}
