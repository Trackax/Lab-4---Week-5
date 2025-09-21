using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public Transform player;
    public Transform thisObject;
    public float orbitDistance = 5f;
    public float orbitSpeed = 30f;
    public float currentAngle = 0f;

    private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * 2f);

        currentAngle += orbitSpeed * Time.deltaTime;
        currentAngle %= 360f;
        float angleRadius = currentAngle * Mathf.Deg2Rad;
        float orbitX = player.position.x + orbitDistance * Mathf.Cos(currentAngle);
        float orbitY = player.position.y + orbitDistance * Mathf.Sin(currentAngle);
        float orbitZ = player.position.z;
        transform.position = new Vector3(orbitX, orbitY, orbitZ);

        Vector3 direction = player.position - thisObject.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = targetRotation;

        if (transform.position.y < -11f)
        {
            Destroy(this.gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D whatIHit)
    {
        if (whatIHit.tag == "Player")
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().gameOver = true;
            TriggerNoise();
            Destroy(whatIHit.gameObject);
            Destroy(this.gameObject);
        } else if (whatIHit.tag == "Laser")
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().meteorCount++;
            TriggerNoise();
            Destroy(whatIHit.gameObject);
            Destroy(this.gameObject);
        }
    }

    public void TriggerNoise()
    {
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }
    }
}
