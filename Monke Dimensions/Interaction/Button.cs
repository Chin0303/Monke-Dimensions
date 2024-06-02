#if EDITOR

#else
using Monke_Dimensions.Behaviours;
using Monke_Dimensions.Browser;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace Monke_Dimensions.Interaction;

public class Button : MonoBehaviour
{
    public ButtonType BtnType;
    private const float debounceTime = 0.25f;
    private void Start()
    {
        gameObject.layer = 18;
    }

    private async void OnTriggerEnter(Collider collider)
    {
        if ( debounceTime >= Time.time) return;
        var hand = collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>();
        if (DimensionBrowser.inBrowser && collider.TryGetComponent(out hand))
        {
            switch (BtnType)
            {
                case ButtonType.Left:
                    DimensionBrowser.instance.PreviousPage();
                    break;

                case ButtonType.Right:
                    DimensionBrowser.instance.NextPage();
                    break;

                case ButtonType.Load:
                    Comps.DownloadingText.SetActive(true);
                    await HandleDownload();
                    DimensionManager.Instance.LoadDownloadedDimension();
                    Comps.DownloadingText.SetActive(false);
                    var confetti = Instantiate(Comps.Confetti);
                    Destroy(confetti, 2f);
                    break;

                case ButtonType.Browser:
                    Comps.MainScreen.SetActive(true);
                    DimensionBrowser.instance.UnloadBrowser();
                    break;
            }
            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(211, hand.isLeftHand, 0.12f);
            GorillaTagger.Instance.StartVibration(hand.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
        }
        else
        {
            if (collider.TryGetComponent(out hand))
            {
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(211, hand.isLeftHand, 0.12f);
                GorillaTagger.Instance.StartVibration(hand.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
                switch (BtnType)
                {
                    case ButtonType.Left:
                        DimensionManager.Instance.SwitchPage(-1);
                        break;
                    case ButtonType.Right:
                        DimensionManager.Instance.SwitchPage(1);
                        break;
                    case ButtonType.Load:
                        DimensionManager.Instance.LoadSelectedDimension(DimensionManager.Instance.dimensionNames[DimensionManager.Instance.currentPage]);
                        break;
                    case ButtonType.Browser:
                        DimensionBrowser.inBrowser = true;
                        DimensionBrowser.instance.OnBrowserEnabled();
                        Comps.MainScreen.SetActive(false);
                        break;
                    case ButtonType.Garfield:
                        DimensionManager.Instance.Garfield.SetActive(true);
                        break;
                }
            }
        }
    }

    private async Task HandleDownload()
    {
        using (var httpClient = new HttpClient())
        {
            using (var response = await httpClient.GetAsync(Item.selectedItem.MapDownload, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();

                var filePath = Path.Combine(Path.GetDirectoryName(typeof(Main).Assembly.Location), "Dimensions", $"{Item.selectedItem.MapName}.dimension");

                using (var ms = await response.Content.ReadAsStreamAsync())
                using (var fs = File.Create(filePath))
                {
                    await ms.CopyToAsync(fs);
                    fs.Flush();
                }
            }
        }
    }
}
#endif