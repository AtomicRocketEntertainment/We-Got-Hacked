using UnityEngine;

public class PointEmailManager : MonoBehaviour
{
    [SerializeField] private SO_PointEmailMap _emailMap;

    public PointEmailEntry GetEmail(PointEmailKey key)
    {
        return _emailMap.GetEmail(key);
    }
}
