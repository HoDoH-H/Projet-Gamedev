using UnityEngine;

public class CharacterGraphic : MonoBehaviour
{
    [SerializeField] private MainHandItem[] mainHandItems;
    [SerializeField] private OffHandItem[] offHandItems;
    [SerializeField] private HeadItem[] headItems;

    public void SetMainHandItem(Character.MainHandState state)
    {
        foreach (MainHandItem item in mainHandItems)
        {
            item.gameObject.SetActive(item.state == state);
        }
    }

    public void SetOffHandItem(Character.OffHandState state)
    {
        foreach (OffHandItem item in offHandItems)
        {
            item.gameObject.SetActive(item.state == state);
        }
    }

    public void SetHeadItem(Character.HeadState state)
    {
        foreach (HeadItem item in headItems)
        {
            item.gameObject.SetActive(item.state == state);
        }
    }
}
