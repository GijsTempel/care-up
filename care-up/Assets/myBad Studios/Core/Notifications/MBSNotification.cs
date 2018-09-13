namespace MBS
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [System.Obsolete( "mbsNotification is being deprecated. Please use MBSNotification instead" )]
    public class mbsNotification : MBSNotification { }
    public class MBSNotification : MonoBehaviour
    {
        static List<MBSNotification> instances;

        public Text
            header_text,
            message_text;

        public float time_to_live = 1.5f;
        public Image icon_image;
        public AnimationCurve  curve;


        Vector2
            origin,
            destination;

        RectTransform rect_transform;

        static public MBSNotification SpawnInstance( Canvas canvas, Vector2 origin, Vector2 destination, string header, string message, Sprite icon = null )
        {
            MBSNotification note = Instantiate( Resources.Load<MBSNotification>( "NotificationPanel" ) );
            if ( null == note )
                return note;

            note.transform.SetParent( canvas.transform, false );
            note.transform.SetAsLastSibling();

            if ( null == instances )
                instances = new List<MBSNotification>();
            instances.Add( note );

            note.origin = origin;
            note.destination = destination;
            note.Initialize( header, message, icon );
            return note;
        }

        void Initialize( string header, string message, Sprite sprite )
        {
            rect_transform = GetComponent<RectTransform>();
            rect_transform.anchoredPosition = origin;

            if ( null == sprite )
                icon_image.gameObject.SetActive( false );
            else
                icon_image.sprite = sprite;

            header_text.text = header;
            message_text.text = message;

            if ( instances [0] == this )
                GetStarted();
            else
                gameObject.SetActive( false );
        }

        public void GetStarted()
        {
            gameObject.SetActive( true );

            StartCoroutine( RevealAndHide() );
        }

        IEnumerator RevealAndHide()
        {
            yield return StartCoroutine( Tween( 1 ) );
            yield return new WaitForSeconds( time_to_live );
            yield return StartCoroutine( Tween( -1 ) );

            instances.Remove( this );
            if ( null != rect_transform )
                Destroy( gameObject );
            if ( instances.Count > 0 )
                instances [0].GetStarted();
        }

        IEnumerator Tween( int direction, float duration = 1.25f )
        {
            Vector2 s = direction < 0 ? destination : origin;
            Vector2 d = direction < 0 ? origin : destination;
            float position = 0f;
            while ( position < 1f )
            {
                if ( null == rect_transform )
                    break;
                rect_transform.anchoredPosition = Vector2.Lerp( s, d, curve.Evaluate( position ) );
                yield return new WaitForEndOfFrame();
                position += Time.deltaTime;
                if ( position > 1f )
                    position = 1f;
            }
            if ( null != rect_transform )
                rect_transform.anchoredPosition = d;
        }
    }
}