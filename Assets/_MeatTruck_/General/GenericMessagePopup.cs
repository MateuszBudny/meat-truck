using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GenericMessagePopup : SingleBehaviour<GenericMessagePopup>
{
    [SerializeField]
    private TextMeshProUGUI messageText;
    [SerializeField]
    private float defaultDisappearingTimeInSeconds = 7f;

    public void ShowMessage(string message, float disappearingTimeInSeconds = -1)
    {
        disappearingTimeInSeconds = disappearingTimeInSeconds < 0 ? defaultDisappearingTimeInSeconds : disappearingTimeInSeconds;
        messageText.SetText(message);
        messageText.DOColor(new Color(1f, 1f, 1f, 0f), disappearingTimeInSeconds).From(Color.white).SetEase(Ease.InCubic);
    }
}
