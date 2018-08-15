using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Слежение камеры за игроком

public class MoveCamera : MonoBehaviour {

    public float mitTransformX;
    public float maxTransformX;

    public float mitTransformY;
    public float maxTransformY;

    public float correctionY;

    public GameObject player;
	

	void FixedUpdate () {

        float xPosition = player.transform.position.x;
        float yPosition = player.transform.position.y + correctionY;

        if(xPosition < mitTransformX || xPosition > maxTransformX)
        {
            xPosition = transform.position.x;
        }

        if(yPosition <= mitTransformY || yPosition >= maxTransformY)
        {
            yPosition = Mathf.Round(transform.position.y);
        }

        transform.position = new Vector3(xPosition, yPosition, -10.0f);

	}
}
