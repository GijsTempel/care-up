using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;
using System.Runtime.InteropServices;

namespace MBS
{
    static public class WUCookie
    {
        [DllImport("__Internal")]
        private static extern void setWebGLCookie(string cname, string cvalue, string cdomain, string cpath, string cexpire);
        [DllImport("__Internal")]
        private static extern string getWebGLCookie(string cname);
        [DllImport("__Internal")]
        private static extern string findWebGLCookieName(string cname);

        public const string cookie_key = "wordpress_logged_in";

        static Dictionary<string, string> _cookie;
        static public string CookieValWebGL;

        static public Dictionary<string, string>
            Cookie
        { get { if (null == _cookie) _cookie = new Dictionary<string, string>(); return _cookie; } set { _cookie = value; } }

        static public string
            CookieVal
        { get { return (Cookie.ContainsKey("Cookie")) ? Cookie["Cookie"] : string.Empty; } set { Cookie["Cookie"] = value; } }

        static public void ClearCookie()
        {
            _cookie = null;
            CookieValWebGL = "";
        }
        static public bool CookieIsSet => null != CookieVal;

        static public void LoadStoredCookie()
        {
            CookieVal = PlayerPrefs.GetString("Cookie");

            // due to security reasons, webgl needs a completely different way of handling cookies
#if UNITY_WEBGL && !UNITY_EDITOR
            CookieValWebGL = PlayerPrefs.GetString("CookieWebGL");
            if (string.Empty == CookieValWebGL)
            {
                WUCookie.GetCookieFromBrowser();
            }
#endif
        }
        static public void StoreCookie()
        {
            string value = CookieVal;
            if (string.Empty == value)
                PlayerPrefs.DeleteKey("Cookie");
            else
                PlayerPrefs.SetString("Cookie", value);

            // due to security reasons, webgl needs a completely different way of handling cookies
#if UNITY_WEBGL && !UNITY_EDITOR
            if (string.Empty == CookieValWebGL)
                PlayerPrefs.DeleteKey("CookieWebGL");
            else
                PlayerPrefs.SetString("CookieWebGL", CookieValWebGL);
#endif
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

        class WebGLAuthCookies
        {
            public string auth_cookies_name;
            public string auth_cookies_value;
            public string auth_cookies_domain;
            public string auth_cookies_path;
            public string auth_cookies_expire;
        }

        // due to security reasons, webgl needs a completely different way of handling cookies
        static public void ExtractWebGLCookie(CML results, bool store = true)
        {
            if (results.Elements.Count <= 0) return;
            int index = Array.IndexOf(results.Elements[0].Keys, "auth_cookies");
            if (index >= 0)
            {
                WebGLAuthCookies cookies = JsonUtility.FromJson<WebGLAuthCookies>(results.Elements[0].Values[index]);

                // setWebGLCookie is in webGL_cookie.jslib
                setWebGLCookie(cookies.auth_cookies_name,
                               cookies.auth_cookies_value,
                               cookies.auth_cookies_domain,
                               cookies.auth_cookies_path,
                               cookies.auth_cookies_expire);

                // now we create a duplicate cookie, but without _INSECURE bit
                cookies.auth_cookies_name = cookies.auth_cookies_name.Replace("_INSECURE", "");

                // and send it
                setWebGLCookie(cookies.auth_cookies_name,
                               cookies.auth_cookies_value,
                               cookies.auth_cookies_domain,
                               cookies.auth_cookies_path,
                               cookies.auth_cookies_expire);

                CookieValWebGL = results.Elements[0].Values[index];
                if (store)
                     StoreCookie();
            }
        }

        static public void GetCookieFromBrowser(bool store = true)
        {
            WebGLAuthCookies cookies = new WebGLAuthCookies();
            cookies.auth_cookies_name = findWebGLCookieName("wordpress_logged_in");
            cookies.auth_cookies_value = getWebGLCookie(cookies.auth_cookies_name);
            cookies.auth_cookies_domain = ".careup.online";
            cookies.auth_cookies_path = "/";
            cookies.auth_cookies_expire = (DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 9999999).ToString();

            CookieValWebGL = JsonUtility.ToJson(cookies);
            if (store)
                StoreCookie();
        }
    }
}