using UnityEngine;
using UnityEngine.UI;

namespace MBS
{
    public class WUUTabGroup : MonoBehaviour
    {
        [SerializeField] GameObject[] tabs = null;
        [SerializeField] Button[] tab_buttons = null;

        public void SelectTab( int index )
        {
            if ( index >= tab_buttons.Length )
                return;
            int counter = 0;
            for ( counter = 0; counter < tab_buttons.Length; counter++ )
            {
                tab_buttons [counter].interactable = index != counter;
                tabs [counter].SetActive(index == counter);
            }
        }

        void Start()
        {
            int runner = 0;
            foreach ( Button b in tab_buttons )
            {
                int tempval = runner;
                b.onClick.AddListener( () => SelectTab( tempval ) );
                runner++;
            }
            SelectTab( 0 );
        }
    }
}