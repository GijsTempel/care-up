mergeInto(LibraryManager.library, {

    GetStringParams: function() {
        var url_string = window.location.href; //"http://www.example.com/t.html?a=1&b=3&c=m2-m3-m4-m5"; 
        var url = new URL(url_string);
        var c = url.searchParams.get("k");
        if (c == null)
        {
            c = ""
        }
        var bufferSize = lengthBytesUTF8(c) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(c, buffer, bufferSize);
        return buffer;
    },

});