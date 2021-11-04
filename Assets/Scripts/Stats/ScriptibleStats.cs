using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Character Sheet", menuName = "Character Sheets", order = 0)]
public class ScriptibleStats : ScriptableObject {
   
   [Header("Attributes")]
     [Range(1,10)] public int art;
   [Range(1,10)] public int combat;
   [Range(1,10)] public int charizma;
   [Range(1,10)] public int perception;

   [Header("SomeThing")]
 [Range(1,10)] public int abovv;
   [Range(1,10)] public int canoyy;

}

