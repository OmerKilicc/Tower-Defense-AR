using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{

    #region Singleton

    public static ParticleManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    [SerializeField]
    private GameObject[] _particles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnParticleAtLocation(Vector3 Location,Particles particle) 
    {
        GameObject particleObject = ReturnParticleFromEnums(particle);

        Instantiate(particleObject, Location, Quaternion.identity);
    }
    

    public enum Particles 
    {
        Explosion = 0,
        Spawn = 1    
    }

    GameObject ReturnParticleFromEnums(Particles particle) 
    {
        return _particles[(int)particle];
    }
}
