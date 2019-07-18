//
//  Smartlook.h
//  Smartlook iOS SDK 0.1.12
//
//  Copyright © 2018 Smartsupp.com, s.r.o. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <Foundation/Foundation.h>
#import <AVFoundation/AVFoundation.h>
#import <CoreGraphics/CoreGraphics.h>
#import <CoreMedia/CoreMedia.h>
#import <CoreVideo/CoreVideo.h>
#import <QuartzCore/QuartzCore.h>
#import <SystemConfiguration/SystemConfiguration.h>
#import <WebKit/WebKit.h>

#import "UIView+Smartlook.h"

// Smartlook SDK. To use, call "startWithApiKey:" method from "applicationDidFinishLaunching:withOptions:" in your AppDelegate class

@interface Smartlook : NSObject

/** Starts Smartlook. This method initializes and start Smartlook SDK recording. Call this method once in your "applicationDidFinishLaunching:withOptions:".
 @param key The application (project) specific SDK key, available in your Smartlook dashboard.
 */
+ (void)startWithKey:(nonnull NSString*)key;

/**
 Initializes and starts Smartlook. Call this method once in your "applicationDidFinishLaunching:withOptions:".

 @param key The application (project) specific SDK key, available in your Smartlook dashboard.
 @param framerate Screen recording framerate in fps.
 */
+ (void)startWithKey:(nonnull NSString*)key framerate:(NSInteger)framerate;

/** Initializes Smartlook. This method initializes Smartlook SDK without starting the recording. Call this method once in your "applicationDidFinishLaunching:withOptions:". Call `[Smartlook resumeRecording]` to start recording events and the screen.
 @param key The application (project) specific SDK key, available in your Smartlook dashboard.
 */
+ (void)initializeWithKey:(nonnull NSString*)key;

/**
 Initializes Smartlook. This method initializes Smartlook SDK without starting the recording. Call this method once in your "applicationDidFinishLaunching:withOptions:". Call `[Smartlook resumeRecording]` to start recording events and the screen.

 @param key The application (project) specific SDK key, available in your Smartlook dashboard.
 @param framerate Screen recording framerate in fps.
 */
+ (void)initializeWithKey:(nonnull NSString*)key framerate:(NSInteger)framerate;

/** Marks view as sensitive. This view will not be shown in video recordings.
 @param view The view that will not be shown in video recordings
 @param overlayColor Optional overlay color which will be drawn in recordings instead of the sensitive view
 */
+ (void)markViewAsSensitive:(nonnull UIView*)view overlayColor:(nullable UIColor*)overlayColor;

/** Unmarks view as a sensitive view.
 @param view The view that will be unmarked as a sensitive view.
 */
+ (void)unmarkViewAsSensitive:(nonnull UIView*)view;

/** Records timestamped custom event with optional properties.
 @param eventName Name of the event.
 @param propertiesDictionary Optional dictionary with additional information. All values in propertiesDictionary must be NSStrings.
 */
+ (void)recordCustomEventWithEventName:(nonnull NSString*)eventName propertiesDictionary:(nullable NSDictionary<NSString*, NSString*>*)propertiesDictionary;

/**
 Start timer for custom event.
 
 This method does not record an event. It is the subsequent `recordCustomEvent` call with the same `eventName` that does.
 
 In the resulting event, the property dictionaries of `start` and `record` are merged (the `record` values override the `start` ones if the key is the same), and a `duration` property is added to them.

 @param eventName Name of the event.
 @param propertiesDictionary Optional dictionary with additional information. All values in propertiesDictionary must be NSStrings.
 */
+ (void)startCustomEventWithEventName:(nonnull NSString*)eventName propertiesDictionary:(nullable NSDictionary<NSString*, NSString*>*)propertiesDictionary;

/** Set the app's user identifier.
  @param userIdentifier The application-specific user identifier.
  */
+ (void)setUserIdentifier:(nullable NSString*)userIdentifier;

/** Set the app's user properties.
 @param userPropertiesDictionary The application-specific user properties dictionary. All values in userPropertiesDictionary must be NSStrings.
 */
+ (void)setUserPropertiesDictionary:(nullable NSDictionary<NSString*, NSString*>*)userPropertiesDictionary;

/** Pauses recording.
 */
+ (void)pauseRecording;

/** Resumes recording.
 */
+ (void)resumeRecording;

/** Current paused state.
 */
+ (BOOL)isPaused;

@end

