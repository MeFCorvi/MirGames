﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18213
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MirGames.Domain.Users {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Localization {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Localization() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MirGames.Domain.Users.Localization", typeof(Localization).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;!DOCTYPE HTML PUBLIC &quot;-//W3C//DTD XHTML 1.0 Transitional //EN&quot; &quot;http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd&quot;&gt;
        ///&lt;html&gt;
        ///&lt;head&gt;
        ///    &lt;title&gt;&lt;/title&gt;
        ///    &lt;meta http-equiv=&quot;Content-Type&quot; content=&quot;text/html; charset=utf-8&quot;&gt;
        ///    &lt;meta name=&quot;viewport&quot; content=&quot;width=320, target-densitydpi=device-dpi&quot;&gt;
        ///&lt;/head&gt;
        ///&lt;body&gt;
        ///    &lt;table width=&quot;100%&quot; cellpadding=&quot;0&quot; cellspacing=&quot;0&quot; border=&quot;0&quot; data-mobile=&quot;true&quot; dir=&quot;ltr&quot; data-width=&quot;600&quot; style=&quot;background-color: rgb(255, 255, 255);&quot;&gt;
        ///        &lt;tbody&gt;
        ///      [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ActivationRequestedNotification_Body {
            get {
                return ResourceManager.GetString("ActivationRequestedNotification.Body", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Подтверждение регистрации на сайте mirgames.ru.
        /// </summary>
        internal static string ActivationRequestedNotification_Title {
            get {
                return ResourceManager.GetString("ActivationRequestedNotification.Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;!DOCTYPE HTML PUBLIC &quot;-//W3C//DTD XHTML 1.0 Transitional //EN&quot; &quot;http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd&quot;&gt;
        ///&lt;html&gt;
        ///&lt;head&gt;
        ///    &lt;title&gt;&lt;/title&gt;
        ///    &lt;meta http-equiv=&quot;Content-Type&quot; content=&quot;text/html; charset=utf-8&quot;&gt;
        ///    &lt;meta name=&quot;viewport&quot; content=&quot;width=320, target-densitydpi=device-dpi&quot;&gt;
        ///&lt;/head&gt;
        ///&lt;body&gt;
        ///    &lt;table width=&quot;100%&quot; cellpadding=&quot;0&quot; cellspacing=&quot;0&quot; border=&quot;0&quot; data-mobile=&quot;true&quot; dir=&quot;ltr&quot; data-width=&quot;600&quot; style=&quot;background-color: rgb(255, 255, 255);&quot;&gt;
        ///        &lt;tbody&gt;
        ///      [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string PasswordRestoreRequestedNotification_Body {
            get {
                return ResourceManager.GetString("PasswordRestoreRequestedNotification.Body", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Восстановление пароля на сайте mirgames.ru.
        /// </summary>
        internal static string PasswordRestoreRequestedNotification_Title {
            get {
                return ResourceManager.GetString("PasswordRestoreRequestedNotification.Title", resourceCulture);
            }
        }
    }
}