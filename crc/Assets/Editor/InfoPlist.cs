using System.Collections;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

public class MyPluginPostProcessBuild {
    [PostProcessBuild]
    public static void ChangeXcodePlist (BuildTarget buildTarget, string pathToBuiltProject) {
        if (buildTarget == BuildTarget.iOS) {
            // Get plist
            string plistPath = pathToBuiltProject + "/Info.plist";
            PlistDocument plist = new PlistDocument ();
            plist.ReadFromString (File.ReadAllText (plistPath));

            // Get root
            PlistElementDict rootDict = plist.root;

            // background location useage key (new in iOS 8)
            // rootDict.SetString("NSPhotoLibraryAddUsageDescription", "Needs access to your photos library.");
            rootDict.SetBoolean ("ITSAppUsesNonExemptEncryption", false);

            // // background modes
            // PlistElementArray bgModes = rootDict.CreateArray("UIBackgroundModes");
            // bgModes.AddString("location");
            // bgModes.AddString("fetch");

            // Write to file
            File.WriteAllText (plistPath, plist.WriteToString ());
        }
    }
}