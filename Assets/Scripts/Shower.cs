using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using vector2 = UnityEngine.Vector2;

public class Shower : MonoBehaviour
{
    // Simülasyon nesnesini al
    public GameObject Simulation;
    // Sahnedeki Base_Particle nesnesini al
    public GameObject Base_Particle;
    public Vector2 init_speed = new Vector2(1.0f, 0.0f);  // Baþlangýç hýzý
    public float spawn_rate = 1f;  // Parçacýk oluþturma hýzý
    private float time;

    // Baþlangýçta bir kez çalýþtýrýlýr
    void Start()
    {
        // "Simulation" adlý nesneyi bul
        Simulation = GameObject.Find("Simulation");
        // "Base_Particle" adlý nesneyi bul
        Base_Particle = GameObject.Find("Base_Particle");
    }

    // Her karede bir kez çalýþtýrýlýr
    void Update()
    {
        // Parçacýk sayýsýný sýnýrla
        if (Simulation.transform.childCount < 1000)
        {
            // Parçacýklarý sabit bir hýzda oluþtur
            time += Time.deltaTime;
            if (time < 1.0f / spawn_rate)
            {
                return;
            }

            // Nesnenin mevcut pozisyonunda yeni bir parçacýk oluþtur
            GameObject new_particle = Instantiate(Base_Particle, transform.position, Quaternion.identity);

            // Parçacýðýn pozisyonunu güncelle
            new_particle.GetComponent<Particle>().pos = transform.position;
            new_particle.GetComponent<Particle>().previous_pos = transform.position;
            new_particle.GetComponent<Particle>().visual_pos = transform.position;
            new_particle.GetComponent<Particle>().vel = init_speed;

            // Parçacýðý Simülasyon nesnesinin alt nesnesi olarak ayarla
            new_particle.transform.parent = Simulation.transform;

            // Zamaný sýfýrla
            time = 0.0f;
        }
    }
}
