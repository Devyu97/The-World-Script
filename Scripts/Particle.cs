using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Particle : MonoBehaviour
{
    public static Particle instance;

    [HideInInspector]
    public List<UnityEngine.GameObject> hitParticles;
    public List<UnityEngine.GameObject> damagedParticles;

    [Header("Set")]
    public UnityEngine.GameObject hitParticlePrefab;
    public UnityEngine.GameObject damagedParticlePrefab;

    private void Awake()
    {
        instance = this;        
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += Init;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= Init;
    }

    private void Init(Scene scene, LoadSceneMode mode)
    {
        hitParticles = new List<UnityEngine.GameObject>();
        hitParticles.Clear();
        damagedParticles = new List<UnityEngine.GameObject>();
        damagedParticles.Clear();

        for (int i = 0; i < 5; i++)
        {
            UnityEngine.GameObject hit = Instantiate(hitParticlePrefab);
            hit.SetActive(false);
            hitParticles.Add(hit);
        }

        for (int i = 0; i < 5; i++)
        {
            UnityEngine.GameObject damaged = Instantiate(damagedParticlePrefab);
            damaged.SetActive(false);
            damagedParticles.Add(damaged);
        }
    }

    public void ActiveParticle(Vector3 pos, List<UnityEngine.GameObject> particles)
    {
        pos.z = 0;

        foreach (UnityEngine.GameObject particle in particles)
        {
            if (!particle.activeSelf)
            {
                particle.transform.position = pos;
                particle.SetActive(true);
                return;
            }
        }
    }





}
