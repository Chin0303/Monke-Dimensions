#if EDITOR

#else

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Monke_Dimensions.Browser;

public class Item : MonoBehaviour
{
    public string MapName { get; set; }
    public string MapDownload { get; set; }
    public string MapImageUrl { get; set; }

    public RawImage mapImage;
    public float scaleFactor = 1.1f;
    public float duration = 0.2f;

    private Vector3 originalScale;

    public static Item selectedItem;

    private void Start()
    {
        transform.GetChild(0).GetComponent<Text>().text = MapName;
        gameObject.layer = 18;
        originalScale = transform.localScale;
    }

    private IEnumerator ScaleOverTime(Vector3 targetScale, float duration)
    {
        Vector3 initialScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (0.25f >= Time.time) return;

        var hand = collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>();
        if (collider.TryGetComponent(out hand))
        {
            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(211, hand.isLeftHand, 0.12f);
            GorillaTagger.Instance.StartVibration(hand.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);

            if (selectedItem != null && selectedItem != this)
            {
                StartCoroutine(selectedItem.ScaleOverTime(selectedItem.originalScale, duration));
            }

            if (selectedItem == this)
            {
                StartCoroutine(ScaleOverTime(originalScale, duration));
                selectedItem = null;
            }
            else
            {
                Vector3 targetScaleVector = originalScale * scaleFactor;
                StartCoroutine(ScaleOverTime(targetScaleVector, duration));
                selectedItem = this;
            }
        }
    }

    [System.Serializable]
    public class DimensionItemData
    {
        public string Name;
        public string Download;
        public string Image;
    }

    [System.Serializable]
    public class DimensionItemDataWrapper
    {
        public List<DimensionItemData> Dimensions;
    }
}
#endif