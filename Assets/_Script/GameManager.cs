
// 전역, 지역, 멤버 => 전역 > 멤버 > 지역
// static : 정적인 <-> dynamic (new Vector)
// 전역 클래스
using System.Drawing;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;

public static class GameManager
{
    // 전역 변수
    public static bool IsPlaying = false;
    public static bool IsGameOver = false;
    
    // 이동 거리
    public static double mileage;

    // 획득 코인 (int -21억 ~ 21억 : 4byte, uint 0 ~ 42억 : 4byte)
    public static uint coins;

    public static int life = 3;

    // 전역 함수 (Method)
    // public static void Function1()
    // {
    // }

}