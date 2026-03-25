using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
namespace MiniClassRoom
{
    [CreateAssetMenu(fileName = "Meeting Person", menuName = "Scriptable Objcts/MiniClass/Actor")]
    public class Actor : ScriptableObject
    {
        public string id;
        public List<HeadData> headList;
        public List<BodyData> bodyList;
    }

    [System.Serializable]
    public class HeadData
    {
        public string id;
        public Sprite head;
    }

    [System.Serializable]
    public class BodyData
    {
        public string id;
        public Sprite body;
    }
}