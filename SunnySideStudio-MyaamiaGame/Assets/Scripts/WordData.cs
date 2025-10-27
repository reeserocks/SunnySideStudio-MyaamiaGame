using UnityEngine;

[CreateAssetMenu(fileName = "WordData", menuName = "Game/Word Data")]
public class WordData : ScriptableObject
{
    public string myaamiaWord;
    public string englishTranslation;
    [TextArea] public string description;
    public Sprite image;
}
