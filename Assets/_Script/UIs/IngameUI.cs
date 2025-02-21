using UnityEngine;
using TMPro;
using DG.Tweening;
using CustomInspector;


public class IngameUI : MonoBehaviour
{
    [HorizontalLine]
    [SerializeField] TextMeshProUGUI tmInfomation;

    [HorizontalLine]
    [SerializeField] TextMeshProUGUI tmMileage;
    [SerializeField] TextMeshProUGUI tmCoin;
    [SerializeField] TextMeshProUGUI tmHealth;

    void Awake()
    {
        tmInfomation.text = "";

    }

    void Update()
    {
        UpdateCoins();
        UpdateMileage();
    }

    Sequence _seqInfo;
    public void ShowInfo(string info, float duration = 1f)
    {
        tmInfomation.transform.localScale = Vector3.zero;

        if(_seqInfo != null)
            _seqInfo.Kill(true);

        // 숫자 시작할 때 크기 120% -> 100% -> 0%
        // 모든 연출은 duration 동안 완료되도록
        // duration 전체 길이 => 연출이 duration 안에 종료되도록
        _seqInfo = DOTween.Sequence();
        _seqInfo.AppendCallback(() => tmInfomation.text = info);
        _seqInfo.Append(tmInfomation.transform.DOScale(1.2f, duration * 0.1f));
        _seqInfo.Append(tmInfomation.transform.DOScale(1f, duration * 0.4f));
        _seqInfo.AppendInterval(duration*0.2f);
        _seqInfo.Append(tmInfomation.transform.DOScale(0f, duration * 0.1f));

    }

    void UpdateMileage()
    {

        // 작은 수 표현
        if(GameManager.mileage <= 1000f)
        {
            long intpart = (long)GameManager.mileage;
            double decpart = (int)((GameManager.mileage - intpart)*10);
            tmMileage.text = $"{intpart}<size=80%>.{decpart}</size><size=60%>m</size>";
        }

        // 큰 수 표현
        else
        {
            // 정수, 소수점
            ((long)GameManager.mileage).ToStringKilo(out string intpart, out string decpart, out string  unitpart);
            
            tmMileage.text = $"{intpart}<size=80%>{decpart}{unitpart}</size><size=60%>m</size>";
        }

    }
    // 코인 획득 시, 
    // UI 숫자 120% -> 100% 애니메이션
    // Tween -> Update, OneTime
    // Event 호출
    // Player -> 코인획득 -> GM -> UI 호출
    // Event Driven (이벤트 주도 방식)

    // Send, Receive
    // Player -> 사건 -> Manager 방송 (송출, 수신)

    private uint _lastcoins;
    private Tween _tweeencoin;
    void UpdateCoins()
    {
        if(_lastcoins == GameManager.coins)
            return;

        if(_tweeencoin != null)
            _tweeencoin.Kill(true);

        // "N0" 역할 12345 => 12,345
        tmCoin.text = GameManager.coins.ToString("N0");
        _lastcoins = GameManager.coins;

        tmCoin.rectTransform.localScale = Vector3.one;
        _tweeencoin = tmCoin.rectTransform.DOPunchScale(Vector3.one*0.5f, 0.2f, 10, 1)
                        .OnComplete(() => tmCoin.rectTransform.localScale = Vector3.one);
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
