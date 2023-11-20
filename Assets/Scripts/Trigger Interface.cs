using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Enum for different types of effects
public enum Fx
{
    Sound,
    Narration,
    Light,
    Dynamic,
    Other
}

// ITrigger interface declaration
public interface ITrigger
{
    float Duration { get; }
    bool WaitForCompletion { get; }
    Fx Type { get; }
    void Trigger();
}





