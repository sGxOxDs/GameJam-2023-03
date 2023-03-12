using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTriggerPass : MonoBehaviour
{
    public GameObject AirWall;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            AirWall.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var isLeft = transform.position.x < collision.transform.position.x;
            MapManager.Instance.HitTrigger(transform.parent.gameObject, isLeft);
        }
    }
}
