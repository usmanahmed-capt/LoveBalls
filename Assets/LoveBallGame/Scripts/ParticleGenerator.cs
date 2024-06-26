using UnityEngine;
using System.Collections;


public class ParticleGenerator : MonoBehaviour {
    
    public float WaterSpawnTimeInterval = 0.01f;        
    public float TapClosingTime = 2.5f;                 
    float lastSpawnTime = float.MinValue;              
    private Vector3 particleForce;                     
	private Transform particlesParent;                  

	void Start() {
        Invoke("TapClose", TapClosingTime);
        GetComponent<AudioSource>().Play();
        particlesParent = this.transform;
        if (transform.eulerAngles == Vector3.zero)
        {
            particleForce = Vector3.zero;
        }
        if (transform.eulerAngles.z == 90)
        {
            particleForce = new Vector3(90,0,0);
        }
        if (transform.eulerAngles.z == 270)
        {
            particleForce = new Vector3(-90, 0, 0);
        }
    }

    void FixedUpdate()
    {
        if (lastSpawnTime + WaterSpawnTimeInterval < Time.time)                                                 // Is it time already for spawning a new particle?
        { 
            GameObject newLiquidParticle = (GameObject)Instantiate(Resources.Load("DynamicParticle"), new Vector3(Random.Range(transform.position.x - .001f,
                transform.position.x + .001f), Random.Range(transform.position.y - .001f, transform.position.y
                + .001f), 0), Quaternion.identity);                                                                                     
            newLiquidParticle.GetComponent<Rigidbody2D>().AddForce(particleForce);                            
            DynamicParticle particleScript = newLiquidParticle.GetComponent<DynamicParticle>();                 
                                                                                
            newLiquidParticle.transform.parent = particlesParent;                                              
            lastSpawnTime = Time.time;                                                                      
        }
    }

    void TapClose()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<ParticleGenerator>().enabled = false;
    }
}
