using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using vector2 = UnityEngine.Vector2;

public class Shower : MonoBehaviour
{
    // Sim�lasyon nesnesini al
    public GameObject Simulation;
    // Sahnedeki Base_Particle nesnesini al
    public GameObject Base_Particle;
    public Vector2 init_speed = new Vector2(1.0f, 0.0f);  // Ba�lang�� h�z�
    public float spawn_rate = 1f;  // Par�ac�k olu�turma h�z�
    private float time;

    // Ba�lang��ta bir kez �al��t�r�l�r
    void Start()
    {
        // "Simulation" adl� nesneyi bul
        Simulation = GameObject.Find("Simulation");
        // "Base_Particle" adl� nesneyi bul
        Base_Particle = GameObject.Find("Base_Particle");
    }

    // Her karede bir kez �al��t�r�l�r
    void Update()
    {
        // Par�ac�k say�s�n� s�n�rla
        if (Simulation.transform.childCount < 1000)
        {
            // Par�ac�klar� sabit bir h�zda olu�tur
            time += Time.deltaTime;
            if (time < 1.0f / spawn_rate)
            {
                return;
            }

            // Nesnenin mevcut pozisyonunda yeni bir par�ac�k olu�tur
            GameObject new_particle = Instantiate(Base_Particle, transform.position, Quaternion.identity);

            // Par�ac���n pozisyonunu g�ncelle
            new_particle.GetComponent<Particle>().pos = transform.position;
            new_particle.GetComponent<Particle>().previous_pos = transform.position;
            new_particle.GetComponent<Particle>().visual_pos = transform.position;
            new_particle.GetComponent<Particle>().vel = init_speed;

            // Par�ac��� Sim�lasyon nesnesinin alt nesnesi olarak ayarla
            new_particle.transform.parent = Simulation.transform;

            // Zaman� s�f�rla
            time = 0.0f;
        }
    }
}
