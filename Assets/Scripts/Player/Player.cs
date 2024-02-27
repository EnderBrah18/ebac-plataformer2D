using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D myrigibody;

    public Vector2 velocity;

    public float speed;

    public void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //myrigibody.MovePosition(myrigibody.position - velocity * Time.deltaTime);
            myrigibody.velocity = new Vector2(-speed, myrigibody.velocity.x);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
            {
            //myrigibody.MovePosition(myrigibody.position + velocity * Time.deltaTime);
            myrigibody.velocity = new Vector2(speed, myrigibody.velocity.x);
        }
    }
}
