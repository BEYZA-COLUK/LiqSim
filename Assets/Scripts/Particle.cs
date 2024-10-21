using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using vector2 = UnityEngine.Vector2;
using list = System.Collections.Generic.List<Particle>;

using static Config;

public class Particle : MonoBehaviour
{
    // Config.cs dosyas�ndaki sim�lasyon de�i�kenlerini i�e aktar
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

    // Fiziksel de�i�kenler
    public vector2 pos;  // par�ac���n pozisyonu
    public vector2 previous_pos;  // �nceki pozisyon
    public vector2 visual_pos;  // g�rsel pozisyon
    public float rho = 0.0f;  // yo�unluk
    public float rho_near = 0.0f;  // yak�n yo�unluk
    public float press = 0.0f;  // bas�n�
    public float press_near = 0.0f;  // yak�n bas�n�
    public list neighbours = new list();  // kom�u par�ac�klar listesi
    public vector2 vel = vector2.zero;  // h�z vekt�r�
    public vector2 force = new vector2(0f, -G);  // kuvvet vekt�r�
    public float velocity = 0.0f;  // h�z b�y�kl���

    // Kafes b�l�mlendirmesi i�in �zgaradaki pozisyon
    public int grid_x;
    public int grid_y;

    void Start()
    {
        // Ba�lang�� pozisyonunu ayarla
        pos = transform.position;
        previous_pos = pos;
        visual_pos = pos;
    }

    // Her karede bir kez g�ncellenir
    public void UpdateState()
    {
        // �nceki pozisyonu s�f�rla
        previous_pos = pos;

        // Kuvvet uygula, Newton'un ikinci yasas� ve Euler entegrasyonu ile k�tle = 1
        vel += force * Time.deltaTime * DT;

        // Par�ac��� h�z�na g�re hareket ettir, Euler entegrasyonu kullanarak
        pos += vel * Time.deltaTime * DT;

        // G�rsel pozisyonu g�ncelle
        visual_pos = pos;
        transform.position = visual_pos;

        // Kuvveti s�f�rla
        force = new vector2(0, -G);

        // Euler entegrasyonu kullanarak h�z� tan�mla
        vel = (pos - previous_pos) / Time.deltaTime / DT;

        // H�z b�y�kl���n� hesapla
        velocity = vel.magnitude;

        // H�z MAX_VEL'den b�y�kse, MAX_VEL'e ayarla
        if (velocity > MAX_VEL)
        {
            vel = vel.normalized * MAX_VEL;
        }

        // Yo�unlu�u s�f�rla
        rho = 0.0f;
        rho_near = 0.0f;

        // Kom�ular� s�f�rla
        neighbours = new list();

        // Pozisyon BOTTOM'un alt�ndaysa, par�ac��� sil
        if (pos.y < BOTTOM)
        {
            // E�er ad� Base_Particle de�ilse, par�ac��� sil
            if (name != "Base_Particle")
            {
                Destroy(gameObject);
            }
        }
    }

    public void CalculatePressure()
    {
        // Bas�n� hesapla
        press = K * (rho - REST_DENSITY);
        press_near = K_NEAR * rho_near;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // �arp��man�n normal vekt�r�n� hesapla
        vector2 normal = collision.contacts[0].normal;

        // Par�ac���n normal y�ndeki h�z�n� hesapla
        float vel_normal = Vector2.Dot(vel, normal);

        // H�z pozitifse, par�ac�k duvardan uzakla��yor demektir
        if (vel_normal > 0)
        {
            return;
        }

        // Par�ac���n te�et y�ndeki h�z�n� hesapla
        vector2 vel_tangent = vel - normal * vel_normal;

        // Par�ac���n yeni h�z�n� hesapla
        vel = vel_tangent - normal * vel_normal * WALL_DAMP;

        // Par�ac��� duvar�n d���na ta��
        pos = collision.contacts[0].point + normal * WALL_POS;
    }
}
