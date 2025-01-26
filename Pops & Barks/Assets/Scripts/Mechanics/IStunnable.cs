using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStunnable
{
    /// <summary>
    /// Stuns the object for a specified duration.
    /// </summary>
    /// <param name="duration">The duration of the stun in seconds.</param>
    void Stun(float duration);
}

