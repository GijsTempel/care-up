namespace MBS
{
    using UnityEngine;
    using System;
#if !UNITY_WSA
    using System.IO;
#endif

    public class CMLData : CMLDataBase
    {

        static public bool operator true( CMLData v ) { return null != v; }
        static public bool operator false( CMLData v ) { return null == v; }

        public CMLData() : base( 0 ) { }
        public CMLData( int id ) : base( id ) { }

        [Obsolete( "Vector is obsolete. Please use Vector3 instead" )]
        virtual public Vector2 Vector( string named = "value" ) => Vector3( named );
        virtual public Vector3 Vector3( string named = "value" )
        {
            Vector3 result = UnityEngine.Vector3.zero;
            return result.FromString( String( named ) );
        }

        [Obsolete( "Vect2 is obsolete. Please use Vector2 instead" )]
        virtual public Vector2 Vect2( string named = "value" ) => Vector2( named );
        virtual public Vector2 Vector2( string named = "value" )
        {
            Vector2 result = UnityEngine.Vector2.zero;
            return result.FromString( String( named ) );
        }

        [Obsolete( "Quat is obsolete. Please use Quaternion instead" )]
        virtual public Quaternion Quat( string named = "value" ) => Quaternion( named );
        virtual public Quaternion Quaternion( string named = "value" )
        {
            Quaternion result = UnityEngine.Quaternion.identity;
            return result.FromString( String( named ) );
        }

        [Obsolete( "Rect4 is obsolete. Please use Rect instead" )]
        virtual public Rect Rect4( string named = "value" ) => Rect( named );
        virtual public Rect Rect( string named = "value" )
        {
            Rect result = UnityEngine.Rect.zero;
            return result.FromString( String( named ) );
        }

        virtual public Color Color( string named = "value" )
        {
            Color result = UnityEngine.Color.black;
            return result.FromString( String( named ) );
        }

        //returns a duplicate of this CMLData object
        new public CMLData Copy( CMLCopyMode mode = CMLCopyMode.no_id, string id_value = "-1" )
        {
            CMLData result = new CMLData();
            CopyTo( result );
            return result;
        }
    }
}