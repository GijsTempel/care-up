﻿namespace CareUpAvatar
{
    public enum Actions
    {
        Idle,
        Dance,
        Sad,
        Happy,
        Posing
    };

    public enum Gender
    {
        Male,
        Female
    };   

    public class PlayerAvatarData
    {
        public Gender gender = new Gender();
        public int headType;
        public int bodyType;
        public int glassesType;
        public int mouthType;
        public int eyeType;
        public string hat;

        public PlayerAvatarData(Gender _gender, int _headType, int _bodyType, int _glassesType, string _hatType = "", int _mouthType = 0, int _eyeType = 0)
        {
            gender = _gender;
            headType = _headType;
            bodyType = _bodyType;
            glassesType = _glassesType;
            mouthType = _mouthType;
            eyeType = _eyeType;
            hat = _hatType;
        }


        public int GetHatOffsetIndex()
        {
            int headIndex = headType;
            if (gender == Gender.Female)
                headIndex += 1000000;
            return headIndex;
        }
        public PlayerAvatarData()
        {
            gender = Gender.Male;
            headType = 0;
            bodyType = 0;
            glassesType = 0;
            hat = "";
        }
    }
}