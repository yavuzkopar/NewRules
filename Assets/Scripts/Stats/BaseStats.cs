using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStats : MonoBehaviour
{
    public ScriptibleStats Stats;

    public enum TagManager{
        Player,
        Civil,
        Gangster,
        Barmen
    }
    public List<TagManager> tags = new List<TagManager>();
}
