using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Слежение джунглей за игроком

public class Middle : MonoBehaviour {

    private float speed = 2f;

    public Transform target;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(0f, target.position.y-10f, 10f), speed * Time.deltaTime);
    }
}
