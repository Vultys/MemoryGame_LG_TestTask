using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings", order = 0)]
public class GameSettings : ScriptableObject 
{
    public float rememberingDelay = 5f;

    public float checkDelay = 1f;

    public string cardFacesJsonUrl = "https://drive.usercontent.google.com/download?id=1bSpNwfDG2jp7luEf0GYXyxcAZEsftOdv&export=download&authuser=0&confirm=t&uuid=b1d33e76-2415-432f-a413-4a8f6047733d&at=AEz70l7NNKaOKeuD-IVAx9c1qixW:1740990691029";
}