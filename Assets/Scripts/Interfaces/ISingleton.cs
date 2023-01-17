using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISingleton
{
    public bool IsDone { get; }
    public void Initialize();
}
