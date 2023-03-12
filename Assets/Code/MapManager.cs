using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }
    void Awake()
    { 
        Instance = this; 
    }

    [SerializeField] GameObject[] zones;
    [SerializeField] float zoneDist = 28.3f;
    [SerializeField] GameObject CamArea;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitTrigger(GameObject hitZone, bool isLeft = true)
    {
        CamArea.transform.position = hitZone.transform.position;
        foreach (var zone in zones)
        {
            if(hitZone != zone)
            {
                zone.transform.position = hitZone.transform.position +  zoneDist * 2f * -(isLeft ? Vector3.left : Vector3.right);
                zone.GetComponentInChildren<ZoneTriggerPass>().AirWall.SetActive(false);
            }
        }
    }
}
