module plugins

import os

const ios_path = '${os.dir(os.executable())}/../../../plugins/ios/project'

pub struct IOS {}

pub fn IOS.build() ! {
}

pub fn IOS.clean() ! {
}
