#!/usr/bin/env bash

set -x

echo "Testng for $TEST_PLATFORM"

PROJECT_PATH=/root/project/
RESULT_PATH=${PROJECT_PATH}ci/test_results/

${UNITY_EXECUTABLE:-xvfb-run --auto-servernum --server-args='-screen 0 640x480x24' /opt/Unity/Editor/Unity} \
  -projectPath $PROJECT_PATH \
  -runTests \
  -testPlatform playmode \
  -testResults ${RESULT_PATH}results_integration.xml \
  -logFile \
  -batchmode

UNITY_EXIT_CODE=$?

if [ $UNITY_EXIT_CODE -eq 0 ]; then
  echo "Run succeeded, no failures occurred";
elif [ $UNITY_EXIT_CODE -eq 2 ]; then
  echo "Run succeeded, some tests failed";
elif [ $UNITY_EXIT_CODE -eq 3 ]; then
  echo "Run failure (other failure)";
else
  echo "Unexpected exit code $UNITY_EXIT_CODE";
fi

cat ${RESULT_PATH}results_integration.xml | grep test-run | grep Passed
exit $UNITY_TEST_EXIT_CODE
