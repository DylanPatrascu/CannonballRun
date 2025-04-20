using System.Collections;
using TMPro;
using UnityEngine;

public class chatScript : MonoBehaviour
{

    public GameObject head;

    public TMP_Text chat;
    public TMP_Text button1;
    public TMP_Text button2;

    private Vector3 pos1;
    private Vector3 pos2;
    private Vector3 add = new Vector3(0, 30, 0);
    private Vector3 sub = new Vector3(0, 30, 0);

    void Start()
    {
        pos1 = head.transform.position;
        pos2 = pos1 + add;
    }

    private IEnumerator animate()
    {

        for (int i = 0; i < 4; i++)
        {
            float t = 0;
            float timeElapsed = 0;

            while (t < 1)
            {
                t = timeElapsed * 4;

                head.transform.position = Vector3.Lerp(pos1, pos2, t);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            t = 0;
            timeElapsed = 0;
            while (t < 1)
            {
                t = timeElapsed * 4;

                head.transform.position = Vector3.Lerp(pos2, pos1, t);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
    }

    public void click()
    {
        StartCoroutine(animate());
    }
}
