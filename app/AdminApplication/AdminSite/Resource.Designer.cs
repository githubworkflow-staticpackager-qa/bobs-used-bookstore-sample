﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AdminSite {
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resource {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("AdminSite.Resource", typeof(Resource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public static System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        public static string AddConditionsMessage {
            get {
                return ResourceManager.GetString("AddConditionsMessage", resourceCulture);
            }
        }
        
        public static string AddGenreMessage {
            get {
                return ResourceManager.GetString("AddGenreMessage", resourceCulture);
            }
        }
        
        public static string AddPublisherMessage {
            get {
                return ResourceManager.GetString("AddPublisherMessage", resourceCulture);
            }
        }
        
        public static string AddTypeMessage {
            get {
                return ResourceManager.GetString("AddTypeMessage", resourceCulture);
            }
        }
        
        public static string CombinationErrorStatus {
            get {
                return ResourceManager.GetString("CombinationErrorStatus", resourceCulture);
            }
        }
        
        public static string ConditionExistsStatus {
            get {
                return ResourceManager.GetString("ConditionExistsStatus", resourceCulture);
            }
        }
        
        public static string GenreExistsStatus {
            get {
                return ResourceManager.GetString("GenreExistsStatus", resourceCulture);
            }
        }
        
        public static string PublisherExistsStatus {
            get {
                return ResourceManager.GetString("PublisherExistsStatus", resourceCulture);
            }
        }
        
        public static string TypeExistsStatus {
            get {
                return ResourceManager.GetString("TypeExistsStatus", resourceCulture);
            }
        }
        
        public static string UpdateSuccessMessage {
            get {
                return ResourceManager.GetString("UpdateSuccessMessage", resourceCulture);
            }
        }
        
        public static string MaxQuantityErrorMessage {
            get {
                return ResourceManager.GetString("MaxQuantityErrorMessage", resourceCulture);
            }
        }
        
        public static string RemoveFailErrorMessage {
            get {
                return ResourceManager.GetString("RemoveFailErrorMessage", resourceCulture);
            }
        }
        
        public static string IntegerQuantityErrorMessage {
            get {
                return ResourceManager.GetString("IntegerQuantityErrorMessage", resourceCulture);
            }
        }
        
        public static string OrderStatusChangeErrorMessage {
            get {
                return ResourceManager.GetString("OrderStatusChangeErrorMessage", resourceCulture);
            }
        }
        
        public static string OrderStatusNoChangeErrorMessage {
            get {
                return ResourceManager.GetString("OrderStatusNoChangeErrorMessage", resourceCulture);
            }
        }
        
        public static string OrderStatusUpdateErrorMessage {
            get {
                return ResourceManager.GetString("OrderStatusUpdateErrorMessage", resourceCulture);
            }
        }
    }
}