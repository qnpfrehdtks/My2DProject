using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui_Scene : UI_Base
{
    enum GoldText
    {
        GOLD_TEXT
    }

    enum GoldImage
    {
        GOLD_IMAGE
    }

    public override void Initialize(GameObject factory)
    {
        base.Initialize(factory);

        BindUI<Image>(typeof(GoldImage));
        BindUI<TMPro.TextMeshProUGUI>(typeof(GoldText));

        GameMaster.Instance.InitGold();
        GetText((int)GoldText.GOLD_TEXT).text = GameMaster.Instance.Gold.ToString();
    }

    public override void OnPopFromQueue()
    {
        base.OnPopFromQueue();
        TMPro.TextMeshProUGUI goldText = GetText((int)GoldText.GOLD_TEXT);

        if (goldText != null)
        {
            goldText.text = GameMaster.Instance.Gold.ToString();
        }
    }
}
