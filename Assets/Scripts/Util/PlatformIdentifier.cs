namespace Util
{
    public static class PlatformIdentifier
    {
        public enum Platform
        {
            PC,
            Mobile
        }

        public static Platform GetPlatform()
        {
            
            #if UNITY_IOS || UNITY_ANDROID
            return Platform.Mobile
            #else
            return Platform.PC;
            #endif
        }
    }
}