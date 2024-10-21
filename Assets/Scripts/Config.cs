using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    // Simülasyon parametreleri
    public static int N = 20; // Parçacýk sayýsý
    public static float SIM_W = 0.5f;  // Simülasyon alanýnýn geniþliði
    public static float BOTTOM = -2f;  // Simülasyon alanýnýn tabaný
    public static float DAM = -0.3f;  // Barajýn pozisyonu, simülasyon alaný -0.5 ile 0.5 arasýndadýr
    public static int DAM_BREAK = 200; // Barajýn kýrýlacaðý kare sayýsý
    public static float DT = 10f;  // Zaman adýmý
    public static float WALL_POS = 0.08f; // Parçacýklarýn duvarlara çok yakýn olmasýný önlemek için pozisyon ayarý

    // Fizik parametreleri
    public static float G = 0.02f * 0.25f;  // Yerçekimi ivmesi
    public static float SPACING = 0.09f;  // Parçacýklar arasýndaki mesafe, basýnç hesaplamalarýnda kullanýlýr
    public static float K = SPACING / 1000.0f;  // Basýnç faktörü
    public static float K_NEAR = K * 10f;  // Yakýn basýnç faktörü, parçacýklar birbirine çok yakýn olduðunda hesaplanýr
    // Varsayýlan yoðunluk, yerel yoðunluk ile karþýlaþtýrýlýr ve basýnç hesaplanýr
    public static float REST_DENSITY = 3.0f;
    // Komþu yarýçapý, iki parçacýk arasýndaki mesafe R'den azsa, komþu kabul edilirler
    public static float R = SPACING * 1.5f;
    public static float SIGMA = 0.2f;  // Viskozite faktörü
    public static float MAX_VEL = 0.1f;  // Parçacýklarýn maksimum hýzý, kararsýzlýðý önlemek için kullanýlýr
    // Duvar kýsýtlarý faktörü, parçacýðýn simülasyon duvarlarýndan ne kadar itileceðini belirler
    public static float WALL_DAMP = 1f;// burasý önemli oyuna göre arttýr
    public static float VEL_DAMP = 0.5f;  // Parçacýklar MAX_VEL'den daha hýzlý hareket ederse hýzýn azaltýlma faktörü
}
