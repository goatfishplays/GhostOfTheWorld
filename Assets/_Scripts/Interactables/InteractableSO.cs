using UnityEngine;

[CreateAssetMenu(fileName = "InteractableSO", menuName = "ScriptableObjects/Interactable")]
public class InteractableSO : ScriptableObject
{
    public bool destroyOnInteract = false;
    public bool interactOnContact = false;
    public bool nonPlayersInteractable = false;
}
