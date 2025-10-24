using UnityEngine;

public class GridObjectActiveHandler : MonoBehaviour
{
    [SerializeField] private Transform _feedbackToAvoidActive;

    public void Active()
    {
        Component[] components = GetComponents<Component>();
        foreach (Component comp in components)
        {
            if (comp is Behaviour)
            {
                ((Behaviour)comp).enabled = true;
            }
            else if (comp is Renderer)
            {
                ((Renderer)comp).enabled = true;
            }
        }

        foreach (Transform child in this.transform)
        {
            if(_feedbackToAvoidActive != child)
                child.gameObject.SetActive(true);
        }
    }
}
