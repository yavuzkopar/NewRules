using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameWorld : MonoBehaviour
{
    private static GameWorld instance =null;

    public static GameWorld Instance{
        get{
            return instance;
        }
    }

    public static GameObject player;
    public GameObject[] chairs;

    public GameObject[] npcs;
    public List<GameObject> gangsters = new List<GameObject>();

   [Range(0,1)] public static float saat = .5f;
   public bool ilerliyor;
   Light isik;
   [Range(0,24)] float saaatt = 18;
   public TMPro.TextMeshProUGUI texttt;
   float ilerlemeHizi;

    private void Awake() {
        instance = this;
//       isik = GameObject.FindGameObjectWithTag("light").GetComponent<Light>();
//       ilerlemeHizi =200 * 0.00138f;
    }
    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");

        npcs = GameObject.FindGameObjectsWithTag("Npc");
        foreach (var item in npcs)
        {
          if(item.GetComponent<BaseStats>().tags.Contains(BaseStats.TagManager.Gangster))
          {
            gangsters.Add(item);
          }
        }
        Debug.Log(gangsters.Count);
        Debug.Log(npcs.Length);
    }
  /*  private void Update() {
        if(ilerliyor){
            saat += ilerlemeHizi*Time.deltaTime;
            saaatt = saat *12;
            if (saat>=1)
            {
                ilerliyor = false;
                
            }
        }
        else{
            saat -= ilerlemeHizi*Time.deltaTime;
            saaatt = 24 - (saat * 12);
             if (saat<=0)
            {
                ilerliyor = true;
            }
        }

        texttt.text = "Saat = "+ saaatt.ToString("F0");
        isik.intensity = saat;
    }*/
}
