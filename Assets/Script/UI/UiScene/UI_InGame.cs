using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_InGame : Ui_Scene
{


    enum myText
    {
        COUNTDOWN = 1,
    }
    public override void Initialize(GameObject factory)
    {
        base.Initialize(factory);
        BindUI<TMPro.TextMeshProUGUI>(typeof(myText));

    }


    /// <summary>
    /// UI를 켤때 실행되는 함수.
    /// </summary>
    public override void OnShowUp()
    {
        base.OnShowUp();
        m_currentCountDown = 3;

        StartCoroutine(StartCountDonw_C(3));
    }

    int m_currentCountDown;

    IEnumerator StartCountDonw_C(int countDown)
    {
        TMPro.TextMeshProUGUI text = GetText((int)myText.COUNTDOWN);

        while (m_currentCountDown > 0)
        {
            text.text = m_currentCountDown.ToString();
            m_currentCountDown -= 1;
            yield return new WaitForSeconds(1.0f);
        }
        text.text = "GO!";

        GameMaster.Instance.OnGameStart();

        yield return new WaitForSeconds(1.0f);


        text.text = "";
    }

}
