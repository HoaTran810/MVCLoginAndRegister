using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSolution.Data.Constants
{
    public static class SystemConstants
    {
        /// <summary>
        /// ConnectionString key
        /// </summary>
        public static string ConnectionString = "NTSolutionDatabase";

        /// <summary>
        /// Token
        /// </summary>
        public const string Token = "Token";
    }

    public static class ControllerName
    {
        public static string Home = "Home";
        public static string Userlogin = "Userlogin";
    }

    public static class ActionName
    {
        public static string Login = "Login";
        public static string Logout = "Logout";
        public static string Register = "Register";
        public static string Index = "Index";
    }
}
