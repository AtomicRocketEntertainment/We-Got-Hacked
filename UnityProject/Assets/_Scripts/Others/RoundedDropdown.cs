using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundedDropdown : TMP_Dropdown
{
    [SerializeField] private Sprite firstSprite;
    [SerializeField] private Sprite lastSprite;
    
    protected override DropdownItem CreateItem(DropdownItem itemTemplate)
    {
        var item = base.CreateItem(itemTemplate);

        int index = item.transform.GetSiblingIndex();
        int total = this.options.Count;

        Image background = item.GetComponent<Image>();
        if (background != null)
        {
            if (index == 0)
                background.sprite = firstSprite;
            else if (index == total - 1)
                background.sprite = lastSprite;
            else
                background.sprite = null;
        }

        return item;
    }
}

