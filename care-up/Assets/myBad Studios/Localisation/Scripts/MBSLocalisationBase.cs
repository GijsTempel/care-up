using UnityEngine;

namespace MBS
{
    public class MBSLocalisationBase : ScriptableObject
    {
        [SerializeField] protected string language_name = "English";
        [SerializeField] protected Sprite localisation_graphic;
        [SerializeField] protected string Author = "JansenSensei";
        public string LanguageName => language_name;
        public Sprite LocalisationGraphic => localisation_graphic;

 
    }
}