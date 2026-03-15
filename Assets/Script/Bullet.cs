using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 40;
    public float lifetime = 5;
    public float damage = 20;
    AudioSource myAudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myAudio = GetComponent<AudioSource>();
        
        if (myAudio != null) 
        myAudio.Play();
        Debug.Log(myAudio);

        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {       
        //GetComponent<Rigidbody>().Move(Time.deltaTime * speed);
        transform.position = transform.position + transform.forward * Time.deltaTime * speed;
    }
}
