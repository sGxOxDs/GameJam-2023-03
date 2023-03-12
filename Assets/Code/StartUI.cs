using UnityEngine;

public class StartUI : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private float distance;

    private RectTransform RT;
    private float speed;
    private float currentSecond;
    public Vector3 originPos;

    void Start()
    {
        RT = GetComponent<RectTransform>();
        speed = distance / time;
        currentSecond = 0;
        originPos = RT.position;
    }

    void Update()
    {
        if (currentSecond < time)
        {
            RT.position += new Vector3(0, -(speed * Time.deltaTime), 0);
            currentSecond += Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
            enabled = false;
        }
    }
}
