using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpOrbController : MonoBehaviour
{
    private int xp = 10;

    public void SetXp(int xp)
    {
        this.xp = xp;
    }
    public int GetXp()
    {
        return xp;
    }
}
