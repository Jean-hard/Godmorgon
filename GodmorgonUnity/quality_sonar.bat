@ECHO ---------------------------------------
@ECHO --            SONARQUBE              --
@ECHO ---------------------------------------

@ECHO BEGIN analysis
SonarScanner.MSBuild.exe begin /k:"%PROJECT_KEY%" /d:sonar.host.url="%SONAR_URL%" /d:sonar.login="%SONAR_USER%" /d:sonar.password="%SONAR_PASS%"

@ECHO PARSE files
"c:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\amd64\MSBuild.exe" "%PROJECT_SOLUTION%" /t:Rebuild

@ECHO CREATE reporting
SonarScanner.MSBuild.exe end /d:sonar.login="%SONAR_USER%" /d:sonar.password="%SONAR_PASS%"
