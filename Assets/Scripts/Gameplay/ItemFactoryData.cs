using UnityEngine;
using System.Collections;

public class ItemFactoryData : MonoBehaviour
{
    [SerializeField]
    private ItemEnums mItemEnum;
    public ItemEnums ItemEnum { get { return mItemEnum; } }
}
