using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    private Vector3 velocity;
    public float maxX;
    public float maxZ;
    public float speedIncrease;
    private AudioSource sound;
    private PlayerMotion playerScript;

    private bool changedDirection = false;

    public PlayerMotion PlayerScript { get => playerScript; set => playerScript = value; }


    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3(0, 0, maxZ);
        sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        changedDirection = false;
        transform.position += velocity * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "Player":
                float maxDist = other.transform.localScale.x * 1 * 0.5f + transform.localScale.x * 1 * 0.5f;
                float dist = transform.position.x - other.transform.position.x;
                float nDist = dist / maxDist;
                velocity = new Vector3(nDist * maxX, velocity.y, -velocity.z);
                sound.Play();
                break;

            case "Boundary":
                velocity = new Vector3(-velocity.x, velocity.y, velocity.z);
                break;

            case "Block":
                int response = other.GetComponent<BlockScript>().Damage();
                float speedToAdd = 0;
                if (response > 0)
                {
                    Destroy(other.gameObject);
                    playerScript.AddPoints(response);
                    speedToAdd = speedIncrease;
                }
                sound.Play();
                if (!changedDirection)
                {
                    velocity = new Vector3(velocity.x, velocity.y, -(Math.Sign(velocity.z) * (Math.Abs(velocity.z) + speedToAdd)));
                }
                changedDirection = true;
                break;

            case "Roof":
                velocity = new Vector3(velocity.x, velocity.y, -velocity.z);
                break;

            case "DeathZone":
                playerScript.LoseBall();
                Destroy(gameObject);
                break;
        }
    }
}
