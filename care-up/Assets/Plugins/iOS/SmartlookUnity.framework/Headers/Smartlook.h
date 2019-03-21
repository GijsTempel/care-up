//
//  Smartlook.h
//  Smartlook iOS SDK 0.1.11
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

/** Starts Smartlook. Call this method once in your "applicationDidFinishLaunching:withOptions:".
 @param key The application (project) specific SDK key, available in your Smartlook dashboard.
 */
+ (void)startWithKey:(nonnull NSString*)key;

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

@end

