using UnityEngine;

public class ResetButtonPopUp : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private float destLocation;
    [SerializeField] private float bottomLocation;

    private RectTransform RT;
    private float speed;
    private float currentSecond;

    void Start()
    {
        RT = GetComponent<RectTransform>();
        speed = (destLocation - bottomLocation) / time;
        currentSecond = 0;
        RT.position = new Vector3(0, bottomLocation, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSecond < time)
        {
            RT.position += new Vector3(0, (speed * Time.deltaTime), 0);
            currentSecond += Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
            enabled = false;
        }
    }
}
