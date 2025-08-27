using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;

public static class DOTweenTextMeshProExtensions
{
    public static TweenerCore<string, string, StringOptions> DOText(this TMP_Text target, string endValue, float duration, bool richTextEnabled = true, ScrambleMode scrambleMode = ScrambleMode.None, string scrambleChars = null)
    {
        TweenerCore<string, string, StringOptions> t = DOTween.To(() => target.text, x => target.text = x, endValue, duration);
        t.SetOptions(richTextEnabled, scrambleMode, scrambleChars);
        t.target = target;
        return t;
    }
}
