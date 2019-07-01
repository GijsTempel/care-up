//
//  SmartlookCBridge.h
//  Smartlook iOS SDK 0.1.11
//
//  Copyright © 2018 Smartsupp.com, s.r.o. All rights reserved.
//

#pragma once

#if defined __cplusplus
extern "C" {
#endif
	
	void SmartlookInit(const char* key);
	void SmartlookInitWithFramerate(const char* key, int framerate);
	void SmartlookRecordEvent(const char* eventName);
	void SmartlookRecordEventWithProperties(const char* eventName, const char* properties);
	void SmartlookSetUserIdentifier(const char* userIdentifier);
	void SmartlookPauseRecording();
	void SmartlookResumeRecording();

#if defined __cplusplus
};
#endif
