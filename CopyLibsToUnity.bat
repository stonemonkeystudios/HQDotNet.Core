copy %~dp0HQDotNet-Core\bin\Release\netstandard2.1\*.dll %~dp0HQDotNet-UnityPackage\Assets\HQDotNet.Unity\Runtime\lib\
echo "Libraries copied to Unity Package project."

::Unity >nul 2>&1 && (
::    echo Unity not found, skipping.
::) || (
::    echo Attempting to export Unity package.
::    run %~dp0ExportUnityPackage.bat
::)

::Unity -projectPath "HQDotNet-UnityPackage" -batchmode -nographics -silent-crashes -exportPackage "Assets/HQDotNet.Unity" "../Release/HQDotNet-Unity.unitypackage" -quit
::echo "Unity Export Command Finished"
:: 