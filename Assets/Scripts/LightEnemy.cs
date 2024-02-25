using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LightEnemy : BaseEnemy, ISliceable
{
    // Different Attack?
    // Different Range?
    public void Slice()
    {
        TakeDamage(9999);
    }
}
