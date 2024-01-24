using System.Collections.Generic;
using UnityEngine;

public class MinimapIconsHandler : MonoBehaviour
{
    [SerializeField] private MinimapController _minimap;


    private List<SpriteRenderer> _iconsVisible = new();
    private List<SpriteRenderer> _iconsNearby = new();

    [SerializeField] private LayerMask _visibleIconsLayerMask;

    [SerializeField] private Transform _orientation;


    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent(out SpriteRenderer iconNearby))
        {
            Debug.LogError(other.name + ": Doesnt have a sprite renderer and it should");
            return;
        }

        _iconsNearby.Add(iconNearby);
    }


    private void Update()
    {
        foreach (SpriteRenderer icon in _iconsNearby)
        {
            if (icon == null)
            {
                _iconsNearby.Remove(icon);
                _iconsVisible.Remove(icon);
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (Physics.Raycast(_orientation.position, (other.transform.position - _orientation.position), out RaycastHit hitInfo, 30.0f, _visibleIconsLayerMask, QueryTriggerInteraction.Collide))
        {
            SpriteRenderer sr;

            if (hitInfo.collider.gameObject.layer == 13)
            {
                sr = hitInfo.collider.gameObject.GetComponent<SpriteRenderer>();
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
            else
            {
                sr = other.gameObject.GetComponent<SpriteRenderer>();
                sr.enabled = false;
                _iconsVisible.Remove(sr);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.TryGetComponent(out SpriteRenderer iconNearby))
        {
            Debug.LogError("Doesnt have a sprite renderer and it should");
            return;
        }

        _iconsVisible.Remove(iconNearby);
        iconNearby.enabled = false;
        _iconsNearby.Remove(iconNearby);
    }
}
