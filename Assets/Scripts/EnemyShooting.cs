using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject Bullet;
    public Transform BulletPos;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 2)
        {
            timer = 0;
            shoot();
        }
    }

    public void shoot() 
    {
        Instantiate(Bullet, BulletPos.position, Quaternion.identity);
    }
}
