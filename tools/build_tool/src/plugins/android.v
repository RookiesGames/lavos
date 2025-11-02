module plugins

import os

const android_path = '${os.dir(os.executable())}/../../../plugins/android/project'

pub struct Android {}

pub fn Android.build() ! {
	gradlew := '${android_path}/gradlew -p ${android_path} build'
	//
	mut cmd := '${gradlew} assembleDebug'
	executor(cmd)!
	cmd = '${gradlew} assembleRelease'
	executor(cmd)!
}

pub fn Android.clean() ! {
	cmd := '${android_path}/gradlew -p ${android_path} clean'
	executor(cmd)!
}
