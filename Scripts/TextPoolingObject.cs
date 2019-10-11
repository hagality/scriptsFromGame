using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextPoolingObject : MonoBehaviour
{
    public TextMeshProUGUI txt;
    public bool active;
    float deActiveTime = 1.5f, currentTime = 0;
    bool setPos = false;
    public void FixedUpdate()
    {
        if (setPos == false)
        {
            transform.position = new Vector3(transform.position.x + 0.3f, transform.position.y + 0.03f, transform.position.z + 0.15f);
            setPos = true;
        }
        currentTime += Time.deltaTime;

        if (currentTime >= deActiveTime)
        {
            active = false;
            currentTime = 0;
            setPos = false;
            this.gameObject.SetActive(false);
        }

        transform.position = new Vector3(transform.position.x, transform.position.y + 0.03f, transform.position.z);
    }
}
