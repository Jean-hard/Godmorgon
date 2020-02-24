#!/usr/bin/env bash

set -e
set -x
mkdir -p /root/.cache/unity3d
mkdir -p /root/.local/share/unity3d/Unity/

# Remove the 2 lines below if you have a Unity Pro serial
export UNITY_LICENSE_CONTENT=`cat ./Unity_v2018.x.ulf`
cp ./Unity_v2018.x.ulf /root/.local/share/unity3d/Unity/Unity_lic.ulf

export UNITY_USERNAME=WRITE_UNITY_USERNAME_HERE
export UNITY_PASSWORD=WRITE_UNITY_PASSWORD_HERE 

# Remove the line below if you have a Unity Pro serial
export UNITY_LICENSE_CONTENT=`cat ./Unity_v2018.x.ulf`

export TEST_PLATFORM=linux
export WORKDIR=$pwd
env
set +x
echo 'Writing $UNITY_LICENSE_CONTENT to license file /root/.local/share/unity3d/Unity/Unity_lic.ulf'
echo "$UNITY_LICENSE_CONTENT" | tr -d '\r' > /root/.local/share/unity3d/Unity/Unity_lic.ulf
