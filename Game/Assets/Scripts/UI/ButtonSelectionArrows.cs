using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelectionArrows : Button
{
    private SelectionArrow[] _selectionArrows;

    protected override void Awake()
    {
        base.Awake();

        _selectionArrows = GetComponentsInChildren<SelectionArrow>(true);

        SetArrowsActive(false);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        SetArrowsActive(true);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);

        SetArrowsActive(false);
    }

    private void SetArrowsActive(bool active)
    {
        foreach (var arrow in _selectionArrows)
        {
            arrow.gameObject.SetActive(active);
        }
    }
}
