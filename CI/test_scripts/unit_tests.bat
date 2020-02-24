set UNITY_PATH=D:\Unity\Editor\
set PROJECT_PATH=D:\Profiles\rassere\Projects\docker-unity-ci\
set RESULT_PATH=%PROJECT_PATH%ci\test_results\

%UNITY_PATH%Unity.exe -batchmode -runTests -projectPath %PROJECT_PATH% -testResults %RESULT_PATH%results_unit.xml -testPlatform editmode