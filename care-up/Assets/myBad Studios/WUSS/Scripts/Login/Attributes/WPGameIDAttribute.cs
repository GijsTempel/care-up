using UnityEngine;

namespace MBS
{
    [System.AttributeUsage( System.AttributeTargets.Field )]
    public class WPGameIdAttribute : PropertyAttribute
    {
        public int game_id = 1;
    }
}
