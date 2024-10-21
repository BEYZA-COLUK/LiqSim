using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    // Sim�lasyon parametreleri
    public static int N = 20; // Par�ac�k say�s�
    public static float SIM_W = 0.5f;  // Sim�lasyon alan�n�n geni�li�i
    public static float BOTTOM = -2f;  // Sim�lasyon alan�n�n taban�
    public static float DAM = -0.3f;  // Baraj�n pozisyonu, sim�lasyon alan� -0.5 ile 0.5 aras�ndad�r
    public static int DAM_BREAK = 200; // Baraj�n k�r�laca�� kare say�s�
    public static float DT = 10f;  // Zaman ad�m�
    public static float WALL_POS = 0.08f; // Par�ac�klar�n duvarlara �ok yak�n olmas�n� �nlemek i�in pozisyon ayar�

    // Fizik parametreleri
    public static float G = 0.02f * 0.25f;  // Yer�ekimi ivmesi
    public static float SPACING = 0.09f;  // Par�ac�klar aras�ndaki mesafe, bas�n� hesaplamalar�nda kullan�l�r
    public static float K = SPACING / 1000.0f;  // Bas�n� fakt�r�
    public static float K_NEAR = K * 10f;  // Yak�n bas�n� fakt�r�, par�ac�klar birbirine �ok yak�n oldu�unda hesaplan�r
    // Varsay�lan yo�unluk, yerel yo�unluk ile kar��la�t�r�l�r ve bas�n� hesaplan�r
    public static float REST_DENSITY = 3.0f;
    // Kom�u yar��ap�, iki par�ac�k aras�ndaki mesafe R'den azsa, kom�u kabul edilirler
    public static float R = SPACING * 1.5f;
    public static float SIGMA = 0.2f;  // Viskozite fakt�r�
    public static float MAX_VEL = 0.1f;  // Par�ac�klar�n maksimum h�z�, karars�zl��� �nlemek i�in kullan�l�r
    // Duvar k�s�tlar� fakt�r�, par�ac���n sim�lasyon duvarlar�ndan ne kadar itilece�ini belirler
    public static float WALL_DAMP = 1f;// buras� �nemli oyuna g�re artt�r
    public static float VEL_DAMP = 0.5f;  // Par�ac�klar MAX_VEL'den daha h�zl� hareket ederse h�z�n azalt�lma fakt�r�
}
