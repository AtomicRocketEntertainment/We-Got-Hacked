using UnityEngine;

public class CharacterThoughtsManager : MonoBehaviour
{
    [SerializeField] private SO_CharacterThoughts _characterThoughts;

    public string GetThought(ThoughtKey key)
    {
        return _characterThoughts.GetThought(key);
    }
}
