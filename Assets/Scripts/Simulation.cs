using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using list = System.Collections.Generic.List<Particle>;
using vector2 = UnityEngine.Vector2;

using static Config;

public class Simulation : MonoBehaviour
{
    public list particles = new list();

    // Config.cs dosyas�ndan sim�lasyon de�i�kenlerini al
    public static int N = Config.N;
    public static float SIM_W = Config.SIM_W;
    public static float BOTTOM = Config.BOTTOM;
    public static float DAM = Config.DAM;
    public static int DAM_BREAK = Config.DAM_BREAK;
    public static float G = Config.G;
    public static float SPACING = Config.SPACING;
    public static float K = Config.K;
    public static float K_NEAR = Config.K_NEAR;
    public static float REST_DENSITY = Config.REST_DENSITY;
    public static float R = Config.R;
    public static float SIGMA = Config.SIGMA;
    public static float MAX_VEL = Config.MAX_VEL;
    public static float WALL_DAMP = Config.WALL_DAMP;
    public static float VEL_DAMP = Config.VEL_DAMP;
    public static float DT = Config.DT;
    public static float WALL_POS = Config.WALL_POS;

    // Temel Par�ac�k Nesnesi
    public GameObject Base_Particle;

    // Mekansal B�l�mlendirme Izgaras� De�i�kenleri
    public int grid_size_x = 60;
    public int grid_size_y = 30;
    public list[,] grid;
    public float x_min = 1.8f;
    public float x_max = 6.4f;
    public float y_min = -1.4f;
    public float y_max = 0.61f;

    void Start()
    {
        Base_Particle = GameObject.Find("Base_Particle");

        // Mekansal b�l�mlendirme �zgaras�n� ba�lat
        grid = new list[grid_size_x, grid_size_y];
        for (int i = 0; i < grid_size_x; i++)
        {
            for (int j = 0; j < grid_size_y; j++)
            {
                grid[i, j] = new list();
            }
        }
    }

    // Yard�mc� de�i�kenler
    private float density;
    private float density_near;
    private float dist;
    private float distance;
    private float normal_distance;
    private float relative_distance;
    private float total_pressure;
    private float velocity_difference;
    private vector2 pressure_force;
    private vector2 particule_to_neighbor;
    private vector2 pressure_vector;
    private vector2 normal_p_to_n;
    private vector2 viscosity_force;
    private float time;

    public void calculate_density(list particles)
    {
        /*
            Par�ac�klar�n yo�unlu�unu hesaplar
            Yo�unluk, kom�u par�ac�klar�n g�receli mesafesinin toplanmas�yla hesaplan�r.
            �arp��malar� �nlemek ve karars�zl��� engellemek i�in normal ve yak�n yo�unluk hesaplan�r.

        Parametreler:
            particles (list[Particle]): par�ac�klar�n listesi
        */

        // Her par�ac�k i�in
        foreach (Particle p in particles)
        {
            density = 0.0f;
            density_near = 0.0f;

            // Mekansal b�l�mlendirme �zgaras�nda 9 kom�u h�credeki par�ac�klar� inceler
            for (int i = p.grid_x - 1; i <= p.grid_x + 1; i++)
            {
                for (int j = p.grid_y - 1; j <= p.grid_y + 1; j++)
                {
                    // E�er h�cre �zgarada ise
                    if (i >= 0 && i < grid_size_x && j >= 0 && j < grid_size_y)
                    {
                        // H�credeki her par�ac�k i�in
                        foreach (Particle n in grid[i, j])
                        {
                            // Par�ac�klar aras�ndaki mesafeyi hesapla
                            dist = Vector2.Distance(p.pos, n.pos);

                            if (dist < R)
                            {
                                normal_distance = 1 - dist / R;
                                p.rho += normal_distance * normal_distance;
                                p.rho_near += normal_distance * normal_distance * normal_distance;
                                n.rho += normal_distance * normal_distance;
                                n.rho_near += normal_distance * normal_distance * normal_distance;

                                // Daha sonra kullanmak �zere n'yi p'nin kom�ular�na ekle
                                p.neighbours.Add(n);
                            }
                        }
                    }
                }
            }
            p.rho += density;
            p.rho_near += density_near;
        }
    }

    public void create_pressure(list particles)
    {
        /*
            Par�ac�klar�n bas�n� kuvvetini hesaplar
            Kom�ular listesi ve bas�n�, calculate_density taraf�ndan zaten hesaplanm��t�r
            Bas�n� kuvveti, her kom�unun bas�n� kuvvetinin toplanmas� ve kom�uya do�ru uygulanmas�yla hesaplan�r.

        Parametreler:
            particles (list[Particle]): par�ac�klar�n listesi
        */

        foreach (Particle p in particles)
        {
            pressure_force = vector2.zero;

            foreach (Particle n in p.neighbours)
            {
                particule_to_neighbor = n.pos - p.pos;
                distance = Vector2.Distance(p.pos, n.pos);

                normal_distance = 1 - distance / R;
                total_pressure = (p.press + n.press) * normal_distance * normal_distance + (p.press_near + n.press_near) * normal_distance * normal_distance * normal_distance;
                pressure_vector = total_pressure * particule_to_neighbor.normalized;
                n.force += pressure_vector;
                pressure_force += pressure_vector;
            }
            p.force -= pressure_force;
        }
    }

    public void calculate_viscosity(list particles)
    {
        /*
        Par�ac�klar�n viskozite kuvvetini hesaplar
        Kuvvet = (par�ac�klar�n g�receli mesafesi)*(viskozite katsay�s�)*(par�ac�klar�n h�z fark�)
        H�z fark�, par�ac�klar aras�ndaki vekt�rde hesaplan�r.

        Parametreler:
            particles (list[Particle]): par�ac�klar�n listesi
        */
        foreach (Particle p in particles)
        {
            foreach (Particle n in p.neighbours)
            {
                particule_to_neighbor = n.pos - p.pos;
                distance = Vector2.Distance(p.pos, n.pos);
                normal_p_to_n = particule_to_neighbor.normalized;
                relative_distance = distance / R;
                velocity_difference = Vector2.Dot(p.vel - n.vel, normal_p_to_n);
                if (velocity_difference > 0)
                {
                    viscosity_force = (1 - relative_distance) * velocity_difference * SIGMA * normal_p_to_n;
                    p.vel -= viscosity_force * 0.5f;
                    n.vel += viscosity_force * 0.5f;
                }
            }
        }
    }

    // Her karede bir kez �al��t�r�l�r
    void Update()
    {

        // Alt nesneleri par�ac�klar listesine ekle
        time = Time.realtimeSinceStartup;
        particles.Clear();
        foreach (Transform child in transform)
        {
            particles.Add(child.GetComponent<Particle>());
        }

        // Par�ac�klar� mekansal b�l�mlendirme �zgaras�na yerle�tir
        for (int i = 0; i < grid_size_x; i++)
        {
            for (int j = 0; j < grid_size_y; j++)
            {
                grid[i, j].Clear();
            }
        }
        foreach (Particle p in particles)
        {
            // grid_x ve grid_y'yi x_min, y_min, x_max ve y_max kullanarak ata
            p.grid_x = (int)((p.pos.x - x_min) / (x_max - x_min) * grid_size_x);
            p.grid_y = (int)((p.pos.y - y_min) / (y_max - y_min) * grid_size_y);

            // Par�ac��� s�n�rlar i�inde ise �zgaraya ekle
            if (p.grid_x >= 0 && p.grid_x < grid_size_x && p.grid_y >= 0 && p.grid_y < grid_size_y)
            {
                grid[p.grid_x, p.grid_y].Add(p);
            }
        }
        time = Time.realtimeSinceStartup - time;
        //Debug.Log("Par�ac�klar� �zgaraya yerle�tirme s�resi: " + time);

        time = Time.realtimeSinceStartup;
        foreach (Particle p in particles)
        {
            p.UpdateState();
        }

        time = Time.realtimeSinceStartup - time;
        //Debug.Log("Par�ac�klar� g�ncelleme s�resi: " + time);

        time = Time.realtimeSinceStartup;
        calculate_density(particles);
        time = Time.realtimeSinceStartup - time;
        //Debug.Log("Yo�unluk hesaplama s�resi: " + time);

        time = Time.realtimeSinceStartup;
        foreach (Particle p in particles)
        {
            p.CalculatePressure();
        }
        time = Time.realtimeSinceStartup - time;
        //Debug.Log("Bas�n� hesaplama s�resi: " + time);

        time = Time.realtimeSinceStartup;
        create_pressure(particles);
        time = Time.realtimeSinceStartup - time;
        //Debug.Log("Bas�n� olu�turma s�resi: " + time);

        time = Time.realtimeSinceStartup;
        calculate_viscosity(particles);
        time = Time.realtimeSinceStartup - time;
        //Debug.Log("Viskozite hesaplama s�resi: " + time);
    }
}
