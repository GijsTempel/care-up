namespace CareUpAvatar
{
    public class PlayerAvatarData
    {
        public enum Gender
        {
            Male,
            Female
        };

        public enum Actions
        {
            Idle,
            Dance,
            Sad,
            Happy,
            Posing
        };

        public Gender gender = new Gender();
        public int headType;
        public int bodyType;
        public int glassesType;
        public int mouthType;
        public int eyeType;

        public PlayerAvatarData(Gender _gender, int _headType, int _bodyType, int _glassesType, int _mouthType, int _eyeType)
        {
            gender = _gender;
            headType = _headType;
            bodyType = _bodyType;
            glassesType = _glassesType;
            mouthType = _mouthType;
            eyeType = _eyeType;
        }

        public PlayerAvatarData()
        {
            gender = Gender.Male;
            headType = 0;
            bodyType = 0;
            glassesType = 0;
        }
    }
     

}