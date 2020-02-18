using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuScrollingObject : MonoBehaviour
{
    public float speed = 10f; // 이동 속도

    private void Update()
    {
        // 1초에 10만큼 이동
            transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}
