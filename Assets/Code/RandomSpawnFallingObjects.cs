using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnFallingObjects : MonoBehaviour
{

    [SerializeField] public GameObject[] table;
    [SerializeField] public GameObject player;
    [SerializeField] public float spawnHeight;
    [SerializeField] public float secondSpawn;
    [SerializeField] public float minPos;
    [SerializeField] public float maxPos;
    [SerializeField] public float deleteTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            var position = new Vector3(player.transform.position.x + Random.Range(minPos, maxPos), player.transform.position.y + spawnHeight);
            GameObject gameObject = Instantiate(table[Random.Range(0, table.Length)], position, Quaternion.identity);
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-10f, 10f), Random.Range(-5f, 5f)),ForceMode2D.Impulse);
            yield return new WaitForSeconds(secondSpawn);
        }
    }
}
