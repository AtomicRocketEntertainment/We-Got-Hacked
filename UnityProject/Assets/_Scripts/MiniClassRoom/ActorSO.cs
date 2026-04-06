using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
namespace MiniClassRoom
{
    [CreateAssetMenu(fileName = "Meeting Person", menuName = "Scriptable Objcts/MiniClass/Actor")]
    public class ActorSO : ScriptableObject
    {
        public string id;
        public Color corAtor;
        public List<HeadData> headList;
        public List<BodyData> bodyList;

        public Sprite GetBodySprite(string id)
        {
            Sprite body = null;
            foreach(var item in bodyList)
            {
                if (item.id == id)
                {
                    body = item.body;
                    break;
                }
            }
            return body;
        }

        public Sprite GetHeadSprite(string id)
        {
            Sprite head = null;
            foreach (var item in headList)
            {
                if (item.id == id)
                {
                    head = item.head;
                    break;
                }
            }
            return head;
        }
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