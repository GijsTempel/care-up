using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CAServer
{
    public static string ReleaseBundleAddress = "https://leren.careup.online/Addressables";
    public static string DevBundleAddress = /*"http://careup.sharpminds.com/Addressables"*/"https://ab.3dvit.in.ua/webgl/NewGraphicTest/StreamingAssets/aa/AddressablesLink";
    public static string Version = Application.version;

    public static string BundleAddress = ReleaseBundleAddress;
    //public static string BundleAddress = DevBundleAddress;

}

