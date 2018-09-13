
namespace MBS
{
    using UnityEngine;
    using System;

    public enum ESlideDirection { Up = 0, Down, Right, Left }
    public enum ESlideState { Closed = 0, Opening, Opened, Closing }

    /*	-- FUNCTION LIST -----------------------------------------

        void Init()
        public void Activate()
        public void Deactivate()
        public void Update ()
        public void FadeGUI(bool fade=true)

        -- PROPERTIES -------------------------------------------- 
        -- protected ---
        Rect			curPos;

        -- public ------
        Rect 							Pos	* read only
        float							alpha 
        MBSStateMachine<ESlideState>	slideState
        public Rect 					targetPos
        public bool 					Fade			
        public bool 					Slide			
        public ESlideDirection			slideInDirection
        public ESlideDirection			slideOutDirection
        public float					slideSpeed 		

        -- CALL BACKS -------------------------------------------- 
        public FunctionCall 	OnActivating;
        public FunctionCall 	OnActivated;
        public FunctionCall 	OnDeactivated;
        public FunctionCall 	OnDeactivating;

        -- FUNCTION LIST ----------------------------------------- 
        */

    [Obsolete( "mbsSlider is deprecated. Please use MBSSlider instead" )]
    public class mbsSlider : MBSSlider { public mbsSlider() : base() { } }

    [System.Serializable]
    public class MBSSlider
    {

        public Rect targetPos;
        public bool Fade                            = true;
        public bool Slide                           = true;
        public ESlideDirection  slideInDirection    = ESlideDirection.Right;
        public ESlideDirection  slideOutDirection   = ESlideDirection.Right;
        public float            slideSpeed          = 300.0f;

        public Action  
            OnActivating,
            OnActivated,
            OnDeactivated,
            OnDeactivating;

        protected Rect curPos;

        public float alpha { get; set; }
        public MBSStateMachine<ESlideState> slideState { get; set; }

        public Rect Pos => curPos; 

        public void FadeGUI( bool fade = true )
        {

            Color temp = GUI.color;
            temp.a = fade ? alpha : 1;
            GUI.color = temp;
        }

        public MBSSlider()
        {
            InitStateMachine();
        }

        void InitStateMachine()
        {
            if ( null != slideState )
                return;
            slideState = new MBSStateMachine<ESlideState>();
            slideState.AddState( ESlideState.Closed );
            slideState.AddState( ESlideState.Opening, StateOpening );
            slideState.AddState( ESlideState.Opened );
            slideState.AddState( ESlideState.Closing, StateClosing );
            slideState.SetState( ESlideState.Closed );
            Deactivate( true );
        }

        // this function is called while the state is Opening
        void StateOpening()
        {
            bool isDone = false;
            switch ( slideInDirection )
            {
                case ESlideDirection.Up:
                    curPos.y -= slideSpeed * Time.deltaTime;
                    if ( curPos.y <= targetPos.y )
                        curPos.y = targetPos.y;
                    isDone = curPos.y == targetPos.y;
                    break;

                case ESlideDirection.Down:
                    curPos.y += slideSpeed * Time.deltaTime;
                    if ( curPos.y >= targetPos.y )
                        curPos.y = targetPos.y;
                    isDone = curPos.y == targetPos.y;
                    break;

                case ESlideDirection.Right:
                    curPos.x += slideSpeed * Time.deltaTime;
                    if ( curPos.x >= targetPos.x )
                        curPos.x = targetPos.x;
                    isDone = curPos.x == targetPos.x;
                    break;

                case ESlideDirection.Left:
                    curPos.x -= slideSpeed * Time.deltaTime;
                    if ( curPos.x <= targetPos.x )
                        curPos.x = targetPos.x;
                    isDone = curPos.x == targetPos.x;
                    break;
            }

            DetermineAlpha();
            if ( !isDone )
                isDone = ( Fade && alpha == 1 );

            if ( isDone )
            {
                alpha = 1;
                curPos = targetPos;
                slideState.SetState( ESlideState.Opened );
                OnActivated?.Invoke();
            }
        }

        // this function is called while the state is Closing
        void StateClosing()
        {
            bool isDone = false;
            switch ( slideOutDirection )
            {
                case ESlideDirection.Up:
                    curPos.y -= slideSpeed * Time.deltaTime;
                    isDone = curPos.y <= targetPos.y - targetPos.height;
                    break;

                case ESlideDirection.Down:
                    curPos.y += slideSpeed * Time.deltaTime;
                    isDone = curPos.y >= targetPos.y + targetPos.height;
                    break;

                case ESlideDirection.Right:
                    curPos.x += slideSpeed * Time.deltaTime;
                    isDone = curPos.x >= targetPos.x + targetPos.width;
                    break;

                case ESlideDirection.Left:
                    curPos.x -= slideSpeed * Time.deltaTime;
                    isDone = curPos.x <= targetPos.x - targetPos.width;
                    break;
            }
            DetermineAlpha();
            if ( !isDone )
                isDone = ( Fade && alpha == 0 );

            if ( isDone )
            {
                Init();
                alpha = 0;
                slideState.SetState( ESlideState.Closed );
                if ( null != OnDeactivated )
                    OnDeactivated();
            }
        }

        //set the menu to start at a proper offset to allow for smooth fading
        // also, prepare the state machine
        public void Init()
        {
            InitStateMachine();
            switch ( slideInDirection )
            {
                case ESlideDirection.Up:
                    curPos = new Rect( targetPos.x, targetPos.y + targetPos.height, targetPos.width, targetPos.height );
                    break;

                case ESlideDirection.Down:
                    curPos = new Rect( targetPos.x, targetPos.y - targetPos.height, targetPos.width, targetPos.height );
                    break;

                case ESlideDirection.Right:
                    curPos = new Rect( targetPos.x - targetPos.width, targetPos.y, targetPos.width, targetPos.height );
                    break;

                case ESlideDirection.Left:
                    curPos = new Rect( targetPos.x + targetPos.width, targetPos.y, targetPos.width, targetPos.height );
                    break;
            }
        }

        //start the process of fading in
        public void Activate( bool force = false )
        {
            //force trigger "OnActivated" but skip "OnActivating"
            if ( force )
            {
                curPos = targetPos;
                slideState.SetState( ESlideState.Opening );
                return;
            }

            if ( slideState.CompareState( ESlideState.Closed ) || slideState.CompareState( ESlideState.Closing ) )
            {
                slideState.SetState( ESlideState.Opening );
                if ( null != OnActivating )
                    OnActivating();

                if ( !Slide )
                    curPos = targetPos;
            }
        }

        //start the process of fading out
        public void Deactivate( bool force = false )
        {
            if ( force )
            {
                curPos = targetPos;
                switch ( slideOutDirection )
                {
                    case ESlideDirection.Up:
                        curPos.y -= targetPos.height;
                        break;
                    case ESlideDirection.Down:
                        curPos.y += targetPos.height;
                        break;
                    case ESlideDirection.Left:
                        curPos.x -= targetPos.width;
                        break;
                    case ESlideDirection.Right:
                        curPos.x += targetPos.width;
                        break;
                }
                slideState.SetState( ESlideState.Closing );
                return;
            }
            if ( slideState.CompareState( ESlideState.Opened ) || slideState.CompareState( ESlideState.Opening ) )
            {
                slideState.SetState( ESlideState.Closing );
                if ( null != OnDeactivating )
                    OnDeactivating();

                if ( !Slide )
                {
                    curPos = targetPos;

                    switch ( slideOutDirection )
                    {
                        case ESlideDirection.Up:
                            curPos.y -= targetPos.height;
                            break;
                        case ESlideDirection.Down:
                            curPos.y += targetPos.height;
                            break;
                        case ESlideDirection.Left:
                            curPos.x -= targetPos.width;
                            break;
                        case ESlideDirection.Right:
                            curPos.x += targetPos.width;
                            break;
                    }
                }
            }
        }

        public void ForceState( ESlideState new_state )
        {
            slideState.SetState( new_state );
            switch ( slideState.CurrentState )
            {
                case ESlideState.Closed:
                    Init();
                    break;
                case ESlideState.Opened:
                    curPos = targetPos;
                    break;
            }
        }

        void DetermineAlpha()
        {
            if ( !Fade )
            {
                alpha = 1;
                return;
            }

            float dist = 0;
            float perc = 0;

            //determine wether themenu is sliding in or out to determine which axis to use for alpha testing
            ESlideDirection slideDirection = ( slideState.CompareState( ESlideState.Opening ) ) ? slideInDirection : slideOutDirection;
            switch ( slideDirection )
            {
                case ESlideDirection.Up:
                case ESlideDirection.Down:
                    dist = targetPos.y - curPos.y;
                    break;

                case ESlideDirection.Right:
                case ESlideDirection.Left:
                    dist = targetPos.x - curPos.x;
                    break;
            }

            //speed up fading so the menu fades in half the travel distance
            if ( dist < 0 )
                dist *= -1;
            if ( dist != 0 )
                dist *= 2;

            //test the difference between current pos and target location
            //target location being the position the menu should display at
            //or the position right next to it, offset by exactly the size of the menu
            //return the distance as a percentage and set alpha to that percentage
            switch ( slideDirection )
            {
                case ESlideDirection.Up:
                case ESlideDirection.Down:
                    perc = 1 - ( dist / targetPos.height );
                    break;

                case ESlideDirection.Right:
                case ESlideDirection.Left:
                    perc = 1 - ( dist / targetPos.width );
                    break;
            }

            if ( perc > 1 )
                perc = 1;
            if ( perc < 0 )
                perc = 0;
            alpha = perc;
        }

        public void Update()
        {
            slideState.PerformAction();
        }
    }
}
