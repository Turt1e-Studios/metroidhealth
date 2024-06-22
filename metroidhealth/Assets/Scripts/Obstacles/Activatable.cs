using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activatable : MonoBehaviour
{
    // Parent class for any class that can activate e.g. with pressure plates.
    
    public virtual void Activate() {}
    public virtual void Deactivate() {}
}
