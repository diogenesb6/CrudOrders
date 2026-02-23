namespace CrudOrders.Domain.Resources {
    using System;
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "18.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CrudOrders.Domain.Resources.Messages", typeof(Messages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        internal static string ExcMSG1 {
            get {
                return ResourceManager.GetString("ExcMSG1", resourceCulture);
            }
        }
        
        internal static string ExcMSG2 {
            get {
                return ResourceManager.GetString("ExcMSG2", resourceCulture);
            }
        }
        
        internal static string ExcMSG3 {
            get {
                return ResourceManager.GetString("ExcMSG3", resourceCulture);
            }
        }
        
        internal static string ExcMSG4 {
            get {
                return ResourceManager.GetString("ExcMSG4", resourceCulture);
            }
        }
        
        internal static string ExcMSG5 {
            get {
                return ResourceManager.GetString("ExcMSG5", resourceCulture);
            }
        }
    }
}
