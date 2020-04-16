using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openlan
{
    public static class PluginAttributes
    {
        //Declare PluginAttributes
        public static readonly string CreateMessage = "Create";
        public static readonly string UpdateMessage = "Update";
        public static readonly string Assign = "Assign";
        public static readonly string DeleteMessage = "Delete";
        public static readonly string SetState = "SetState";
        public static readonly string SetStateDynamic = "SetStateDynamicEntity";
        public static readonly string Target = "Target";
        public enum Stage
        {
            PreValidation = 10,
            PreOperation = 20,
            MainOperation = 30,
            PostOperation = 40
        }

    }
}
