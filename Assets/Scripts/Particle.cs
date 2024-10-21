using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using vector2 = UnityEngine.Vector2;
using list = System.Collections.Generic.List<Particle>;

using static Config;

public class Particle : MonoBehaviour
{
    // Config.cs dosyasýndaki simülasyon deðiþkenlerini içe aktar
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

    // Fiziksel deðiþkenler
    public vector2 pos;  // parçacýðýn pozisyonu
    public vector2 previous_pos;  // önceki pozisyon
    public vector2 visual_pos;  // görsel pozisyon
    public float rho = 0.0f;  // yoðunluk
    public float rho_near = 0.0f;  // yakýn yoðunluk
    public float press = 0.0f;  // basýnç
    public float press_near = 0.0f;  // yakýn basýnç
    public list neighbours = new list();  // komþu parçacýklar listesi
    public vector2 vel = vector2.zero;  // hýz vektörü
    public vector2 force = new vector2(0f, -G);  // kuvvet vektörü
    public float velocity = 0.0f;  // hýz büyüklüðü

    // Kafes bölümlendirmesi için ýzgaradaki pozisyon
    public int grid_x;
    public int grid_y;

    void Start()
    {
        // Baþlangýç pozisyonunu ayarla
        pos = transform.position;
        previous_pos = pos;
        visual_pos = pos;
    }

    // Her karede bir kez güncellenir
    public void UpdateState()
    {
        // Önceki pozisyonu sýfýrla
        previous_pos = pos;

        // Kuvvet uygula, Newton'un ikinci yasasý ve Euler entegrasyonu ile kütle = 1
        vel += force * Time.deltaTime * DT;

        // Parçacýðý hýzýna göre hareket ettir, Euler entegrasyonu kullanarak
        pos += vel * Time.deltaTime * DT;

        // Görsel pozisyonu güncelle
        visual_pos = pos;
        transform.position = visual_pos;

        // Kuvveti sýfýrla
        force = new vector2(0, -G);

        // Euler entegrasyonu kullanarak hýzý tanýmla
        vel = (pos - previous_pos) / Time.deltaTime / DT;

        // Hýz büyüklüðünü hesapla
        velocity = vel.magnitude;

        // Hýz MAX_VEL'den büyükse, MAX_VEL'e ayarla
        if (velocity > MAX_VEL)
        {
            vel = vel.normalized * MAX_VEL;
        }

        // Yoðunluðu sýfýrla
        rho = 0.0f;
        rho_near = 0.0f;

        // Komþularý sýfýrla
        neighbours = new list();

        // Pozisyon BOTTOM'un altýndaysa, parçacýðý sil
        if (pos.y < BOTTOM)
        {
            // Eðer adý Base_Particle deðilse, parçacýðý sil
            if (name != "Base_Particle")
            {
                Destroy(gameObject);
            }
        }
    }

    public void CalculatePressure()
    {
        // Basýnç hesapla
        press = K * (rho - REST_DENSITY);
        press_near = K_NEAR * rho_near;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // Çarpýþmanýn normal vektörünü hesapla
        vector2 normal = collision.contacts[0].normal;

        // Parçacýðýn normal yöndeki hýzýný hesapla
        float vel_normal = Vector2.Dot(vel, normal);

        // Hýz pozitifse, parçacýk duvardan uzaklaþýyor demektir
        if (vel_normal > 0)
        {
            return;
        }

        // Parçacýðýn teðet yöndeki hýzýný hesapla
        vector2 vel_tangent = vel - normal * vel_normal;

        // Parçacýðýn yeni hýzýný hesapla
        vel = vel_tangent - normal * vel_normal * WALL_DAMP;

        // Parçacýðý duvarýn dýþýna taþý
        pos = collision.contacts[0].point + normal * WALL_POS;
    }
}
