using UnityEngine;
using TMPro;

public class IngameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmDistance;

    void Start()
    {
        
    }
    
    public long test;
    void Update()
    {
        if (GameManager.IsPlaying == false)
            return;

        // 작은 수 표현
        if(GameManager.mileage <= 1000f)
        {
            long intpart = (long)GameManager.mileage;
            double decpart = (int)((GameManager.mileage - intpart)*10);
            tmDistance.text = $"{intpart}<size=80%>.{decpart}</size><size=60%>m</size>";
        }

        // 큰 수 표현
        else
        {
            // 정수, 소수점
            ((long)GameManager.mileage).ToStringKilo(out string intpart, out string decpart, out string  unitpart);
            
            tmDistance.text = $"{intpart}<size=80%>{decpart}{unitpart}</size><size=60%>m</size>";
        }

    }

    // Utility : 정수부분 + 실수부분을 나눠서 string으로 전달
    // 예 123.456f => 123 + 456 => string 결합
    string FormattedFloat(float value)
    {
        // 123.456   
        // intpart 123  
        // floatpart 123.456 - 0.456 => 0.456 * 10 => 4.56 => (int)4.56 => 4
        int intpart = (int)value;
        float floatpart = value - intpart;

        return $"{intpart}<size=75%>.{(int)(floatpart * 10)}</size>";
    }
}
