using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;

namespace MBS
{

    static public class WUCookie
    {

        public const string cookie_key = "wordpress_logged_in";

        static public void LoadStoredCookie() => CookieVal = PlayerPrefs.GetString("Cookie");
        static public void ClearCookie() => _cookie = null;
        static public bool CookieIsSet => null != CookieVal;

        static Dictionary<string, string> _cookie;

        static public Dictionary<string, string>
            Cookie
        { get { if (null == _cookie) _cookie = new Dictionary<string, string>(); return _cookie; } set { _cookie = value; } }

        static public string
            CookieVal
        { get { return (Cookie.ContainsKey("Cookie")) ? Cookie["Cookie"] : string.Empty; } set { Cookie["Cookie"] = value; } }

        static public void StoreCookie()
        {
            string value = CookieVal;
            if (string.Empty == value)
                PlayerPrefs.DeleteKey("Cookie");
            else
                PlayerPrefs.SetString("Cookie", value);
        }

        static public void ExtractCookie(UnityWebRequest w, bool store = true)
        {
            var responseHeaders = w.GetResponseHeaders();
            if (!responseHeaders.ContainsKey("SET-COOKIE"))
                return;

            string set_cookie_val = responseHeaders["SET-COOKIE"];
            int colIdx = set_cookie_val.IndexOf(cookie_key, StringComparison.CurrentCulture);
            if (colIdx < 1) return;
            string the_cookie = set_cookie_val.Substring(colIdx).Trim();

            if (!string.IsNullOrEmpty(the_cookie))
            {
                string[] lines = the_cookie.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var l in lines)
                {
                    if (l.IndexOf(cookie_key, StringComparison.InvariantCulture) == 0)
                    {
                        if (!string.IsNullOrEmpty(l))
                        {
                            CookieVal = l;
                            if (store)
                                StoreCookie();
                        }
                        return;
                    }
                }
            }
        }
    }
}