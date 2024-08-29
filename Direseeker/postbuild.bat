rem weaver patch for mp compatibility
rem gives an error for some reason, that's why the build event isn't enabled currently. Need to use BepInEx patcher until this is fixed.
cd Weaver\
Unity.UNetWeaver.exe "..\libs\UnityEngine.CoreModule.dll" "..\libs\com.unity.multiplayer-hlapi.Runtime.dll" "..\bin\Debug\netstandard2.1" "..\bin\Debug\netstandard2.1\Direseeker.dll" "..\libs"