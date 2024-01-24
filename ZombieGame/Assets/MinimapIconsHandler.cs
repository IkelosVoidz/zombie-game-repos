using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIconsHandler : MonoBehaviour
{
    [SerializeField] private MinimapController _minimap;


    [SerializeField] SpriteRenderer _playerIcon;


    [SerializeField]
    private List<SpriteRenderer> _iconsVisible = new();
    [SerializeField]
    private List<SpriteRenderer> _iconsNearby = new();

    [SerializeField] private LayerMask _visibleIconsLayerMask;

    [SerializeField] private Transform _orientation;

    private void Awake()
    {
        AddPermanentIcon(_playerIcon);
    }


    public void AddPermanentIcon(SpriteRenderer icon)
    {
        if (_iconsNearby.Contains(icon)) return;

        _iconsNearby.Add(icon);
        _iconsVisible.Add(icon);
    }


    private void OnTriggerEnter(Collider other)
    {
        SpriteRenderer iconNearby;
        if ((iconNearby = other.gameObject.GetComponentInChildren<SpriteRenderer>()) == null)
        {
            Debug.LogError(other.name + ": Doesnt have a sprite renderer and it should");
            return;
        }
        if (!other.gameObject.GetComponentInChildren<MinimapIconStats>()._neverDisable)
        {
            _iconsNearby.Add(iconNearby);
        }
        if (_minimap._zoomedIn)
        {
            iconNearby.gameObject.transform.DOScale(iconNearby.gameObject.gameObject.GetComponent<MinimapIconStats>()._zoomedSize, 1.0f);
            iconNearby.gameObject.transform.DOLocalMoveY(_minimap._newHeight, 1.0f);
        }
    }

    private void Update()
    {
        foreach (SpriteRenderer icon in _iconsNearby) //if icons get destroyed they need to be removed from the list, as c# lists allow null objects to be in them (for some reason)
        {
            if (icon == null)
            {
                _iconsNearby.Remove(icon);
                _iconsVisible.Remove(icon);
            }
        }
    }



    public void ZoomIcons(float height)
    {
        foreach (SpriteRenderer icon in _iconsNearby)
        {
            icon.gameObject.transform.DOScale(icon.gameObject.gameObject.GetComponent<MinimapIconStats>()._zoomedSize, 1.0f);
            icon.gameObject.transform.DOLocalMoveY(height, 1.0f);
        }
    }

    public void ResetZoomIcons()
    {
        foreach (SpriteRenderer icon in _iconsNearby)
        {
            icon.gameObject.transform.DOScale(icon.gameObject.GetComponent<MinimapIconStats>()._defaultSize, 1.0f);
            icon.gameObject.transform.DOLocalMoveY(icon.gameObject.GetComponent<MinimapIconStats>()._defaultHeight, 1.0f);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.GetComponentInChildren<MinimapIconStats>()._neverDisable)
        {
            if (Physics.Raycast(_orientation.position, (other.transform.position - _orientation.position), out RaycastHit hitInfo, 45.0f, _visibleIconsLayerMask, QueryTriggerInteraction.Collide))
            {
                SpriteRenderer sr;
                if (hitInfo.collider.gameObject.layer == 13)
                {
                    if (!hitInfo.collider.gameObject.GetComponentInChildren<MinimapIconStats>()._disabled)
                    {
                        sr = hitInfo.collider.gameObject.GetComponentInChildren<SpriteRenderer>();
                        sr.enabled = true;

                        if (!_iconsVisible.Contains(sr))
                        {
                            _iconsVisible.Add(sr);
                        }

                        /* (hitInfo.collider == other)
                        {
                            other.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                        }
                        else if (hitInfo.collider != other)
                        {
                            hitInfo.collider.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                        }*/
                    }
                }
                else
                {
                    sr = other.gameObject.GetComponentInChildren<SpriteRenderer>();
                    sr.enabled = false;
                    _iconsVisible.Remove(sr);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        SpriteRenderer iconNearby;
        if ((iconNearby = other.gameObject.GetComponentInChildren<SpriteRenderer>()) == null)
        {
            Debug.LogError("Doesnt have a sprite renderer and it should");
            return;
        }

        if (!other.gameObject.GetComponentInChildren<MinimapIconStats>()._neverDisable)
        {

            _iconsVisible.Remove(iconNearby);
            iconNearby.enabled = false;
            _iconsNearby.Remove(iconNearby);
        }
        iconNearby.gameObject.transform.DOScale(other.gameObject.GetComponent<MinimapIconStats>()._defaultSize, 1.0f);
        iconNearby.gameObject.transform.DOLocalMoveY(other.gameObject.GetComponent<MinimapIconStats>()._defaultHeight, 1.0f);
    }
}
