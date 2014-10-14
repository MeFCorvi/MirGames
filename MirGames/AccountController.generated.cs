// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
#pragma warning disable 1591, 3008, 3009
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace MirGames.Controllers
{
    public partial class AccountController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected AccountController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<ActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<ActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Activation()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Activation);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult RestorePassword()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.RestorePassword);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public AccountController Actions { get { return MVC.Account; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Account";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Account";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Login = "Login";
            public readonly string Logout = "Logout";
            public readonly string SignUp = "SignUp";
            public readonly string Activation = "Activation";
            public readonly string RestorePassword = "RestorePassword";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Login = "Login";
            public const string Logout = "Logout";
            public const string SignUp = "SignUp";
            public const string Activation = "Activation";
            public const string RestorePassword = "RestorePassword";
        }


        static readonly ActionParamsClass_Activation s_params_Activation = new ActionParamsClass_Activation();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Activation ActivationParams { get { return s_params_Activation; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Activation
        {
            public readonly string key = "key";
        }
        static readonly ActionParamsClass_RestorePassword s_params_RestorePassword = new ActionParamsClass_RestorePassword();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_RestorePassword RestorePasswordParams { get { return s_params_RestorePassword; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_RestorePassword
        {
            public readonly string key = "key";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
                public readonly string _LoginDialog = "_LoginDialog";
                public readonly string _RestorePasswordDialog = "_RestorePasswordDialog";
                public readonly string _SignUpDialog = "_SignUpDialog";
                public readonly string Login = "Login";
            }
            public readonly string _LoginDialog = "~/Views/Account/_LoginDialog.cshtml";
            public readonly string _RestorePasswordDialog = "~/Views/Account/_RestorePasswordDialog.cshtml";
            public readonly string _SignUpDialog = "~/Views/Account/_SignUpDialog.cshtml";
            public readonly string Login = "~/Views/Account/Login.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_AccountController : MirGames.Controllers.AccountController
    {
        public T4MVC_AccountController() : base(Dummy.Instance) { }

        [NonAction]
        partial void LoginOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Login()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Login);
            LoginOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void LogoutOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Logout()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Logout);
            LogoutOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void SignUpOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult SignUp()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SignUp);
            SignUpOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void ActivationOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string key);

        [NonAction]
        public override System.Web.Mvc.ActionResult Activation(string key)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Activation);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "key", key);
            ActivationOverride(callInfo, key);
            return callInfo;
        }

        [NonAction]
        partial void RestorePasswordOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string key);

        [NonAction]
        public override System.Web.Mvc.ActionResult RestorePassword(string key)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.RestorePassword);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "key", key);
            RestorePasswordOverride(callInfo, key);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009
