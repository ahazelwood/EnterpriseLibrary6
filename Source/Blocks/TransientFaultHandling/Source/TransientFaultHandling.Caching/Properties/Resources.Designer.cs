﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Properties {
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
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Properties.Resources" +
                            "", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Assembly mismatch for type with name &apos;Microsoft.ApplicationServer.Caching.DataCacheException&apos;. Expected partial assembly name is &apos;Microsoft.ApplicationServer.Caching.Core, PublicKeyToken=31bf3856ad364e35&apos;. The actual assembly name found at runtime for the type is &apos;{0}&apos;..
        /// </summary>
        internal static string AssemblyMismatchException {
            get {
                return ResourceManager.GetString("AssemblyMismatchException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expected type mismatch for type with partial name &apos;Microsoft.ApplicationServer.Caching.DataCacheException, Microsoft.ApplicationServer.Caching.Core, PublicKeyToken=31bf3856ad364e35&apos;. The supported versions of the assembly defining the type are 1.0.0.0 and 101.0.0.0..
        /// </summary>
        internal static string TypeMismatchException {
            get {
                return ResourceManager.GetString("TypeMismatchException", resourceCulture);
            }
        }
    }
}
