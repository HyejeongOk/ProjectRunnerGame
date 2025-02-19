
// 전역, 지역, 멤버 => 전역 > 멤버 > 지역
// static : 정적인 <-> dynamic (new Vector)
// 전역 클래스
using System.Drawing;
using Unity.VisualScripting.Dependencies.NCalc;

public static class GameManager
{
    // 전역 변수
    public static bool IsPlaying = false;
    
    //이동 거리
    public static double mileage;

    // 전역 함수 (Method)
    // public static void Function1()
    // {
    // }

}