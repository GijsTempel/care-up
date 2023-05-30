mergeInto(LibraryManager.library, {

	setWebGLCookie: function(cname, cvalue, cdomain, cpath, cexpire) {
		
		cname = UTF8ToString(cname);
		cvalue = UTF8ToString(cvalue);
		cdomain = UTF8ToString(cdomain);
		cpath = UTF8ToString(cpath);
		cexpire = UTF8ToString(cexpire);
		document.cookie = cname + "=" + cvalue + ";domain=" + cdomain + ";path=" + cpath + ";expires=" + cexpire;
	},
 
	findWebGLCookieName: function(cname) {
		var ret="";
		var name = UTF8ToString(cname);
		var decodedCookie = decodeURIComponent(document.cookie);
		var ca = decodedCookie.split(';');
		for(var i = 0; i <ca.length; i++) {
			var c = ca[i];
			while (c.charAt(0) == ' ') {
				c = c.substring(1);
			}
			if (c.indexOf(name) == 0) {
				var endIndex = c.indexOf('=');
				ret=c.substring(0, endIndex);
				break;
			}
		}
		var bufferSize = lengthBytesUTF8(ret) + 1;
		var buffer = _malloc(bufferSize);
		stringToUTF8(ret, buffer, bufferSize);
		return buffer;
	},
 
    getWebGLCookie: function (cname) {
		var ret="";
		var name = UTF8ToString(cname) + "=";
		var decodedCookie = decodeURIComponent(document.cookie);
		var ca = decodedCookie.split(';');
		for(var i = 0; i <ca.length; i++) {
			var c = ca[i];
			while (c.charAt(0) == ' ') {
				c = c.substring(1);
			}
			if (c.indexOf(name) == 0) {
				ret=c.substring(name.length, c.length);
				break;
			}
		}
		var bufferSize = lengthBytesUTF8(ret) + 1;
		var buffer = _malloc(bufferSize);
		stringToUTF8(ret, buffer, bufferSize);
		return buffer;
    },

});
