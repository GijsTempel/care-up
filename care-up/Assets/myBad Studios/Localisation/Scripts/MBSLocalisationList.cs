using UnityEngine;
using System;

namespace MBS
{
    public class MBSLocalisationList : MonoBehaviour
    {
        #region Instancing
        [Serializable]
        public class MBSLocalSection
        {
            [SerializeField] MBSLocalisationBase[] localisations = null;
            int selected;
            public int Selected => selected;
            public MBSLocalisationBase [] Localisations => localisations;
            public T Localisation<T >() where T : MBSLocalisationBase => ((selected >= 0 && selected < localisations.Length) ? (localisations [selected] as T) : null) ?? null;

            public void SelectLocalisation( int index ) => selected = index >= localisations.Length ? 0 : index; 
        }

        static MBSLocalisationList _instance;
        static public MBSLocalisationList Instance
        {
            get
            {
                if ( null == _instance )
                {
                    _instance = FindObjectOfType<MBSLocalisationList>();
                    if ( null == _instance )
                        _instance = Instantiate<MBSLocalisationList>( Resources.Load<MBSLocalisationList>( "LocalisationListPrefab" ) );
                }
                return _instance;
            }
        }
        #endregion

        void Awake()
        {
            if ( null == _instance )
                _instance = this;
            else
            {
                if ( _instance != this )
                    Destroy( gameObject );
                else
                    if (dont_destroy)
                        DontDestroyOnLoad( gameObject );
            }
        }

        [SerializeField] bool dont_destroy = true;

        [SerializeField] MBSLocalSection login = default(MBSLocalSection);
        static public MBSLocalSection Login => Instance.login;
        static public WULLocalisation LoginLocal => Instance.login.Localisation<WULLocalisation>();
        static public MBSLocalisationBase [] AllLogin => Instance.login.Localisations;

#if WUI
        [SerializeField] MBSLocalSection inventory;
        static public MBSLocalSection Inventory => Instance.inventory;
        static public MBSLocalisationInventory InventoryLocal => Instance.inventory.Localisation<MBSLocalisationInventory>();
        static public MBSLocalisationBase [] AllInventory => Instance.inventory.Localisations;
#endif
    }
}

