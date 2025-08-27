using UnityEngine;
using UnityEngine.UI;

public class AmmoSelected : MonoBehaviour
{
    [SerializeField] private Image _selectedAmmoImage;
    [SerializeField] private Image _ammoImage;

    /// <summary>
    /// Sets the selected ammo image to be visible.
    /// </summary>
    public void SetSelectedAmmo()
    {
        if (_selectedAmmoImage != null && !_selectedAmmoImage.enabled)
            _selectedAmmoImage.enabled = true;
    }
    /// <summary>
    /// Unselects the ammo by disabling the selected ammo image.
    /// </summary>
    public void UnselectAmmo()
    {
        if (_selectedAmmoImage != null && _selectedAmmoImage.enabled)
            _selectedAmmoImage.enabled = false;
    }
    /// <summary>
    /// disable the ammo image to indicate that it has been used.
    /// </summary>
    public void SetUsedAmmo()
    {
        if (_ammoImage != null)
            _ammoImage.enabled = false;
        else
        {
            Debugger.AppendText($"No ammo image found");
        }
    }
    /// <summary>
    /// Resets the used ammo image to be visible again.
    /// </summary>
    public void ResetUsedAmmo()
    {
        if (_ammoImage != null)
            _ammoImage.enabled = true;
        else
        {
            Debugger.AppendText($"No ammo image found");
        }
    } 

}
