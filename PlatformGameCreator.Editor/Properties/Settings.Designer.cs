﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PlatformGameCreator.Editor.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool TextureEditor_ShowGrid {
            get {
                return ((bool)(this["TextureEditor_ShowGrid"]));
            }
            set {
                this["TextureEditor_ShowGrid"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool TextureEditor_ShowAllShapes {
            get {
                return ((bool)(this["TextureEditor_ShowAllShapes"]));
            }
            set {
                this["TextureEditor_ShowAllShapes"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool TextureEditor_ShowConvexHull {
            get {
                return ((bool)(this["TextureEditor_ShowConvexHull"]));
            }
            set {
                this["TextureEditor_ShowConvexHull"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool TextureEditor_ShowConvexDecomposition {
            get {
                return ((bool)(this["TextureEditor_ShowConvexDecomposition"]));
            }
            set {
                this["TextureEditor_ShowConvexDecomposition"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool TextureEditor_ShowOrigin {
            get {
                return ((bool)(this["TextureEditor_ShowOrigin"]));
            }
            set {
                this["TextureEditor_ShowOrigin"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string EditorApplication_LastOpenedProject {
            get {
                return ((string)(this["EditorApplication_LastOpenedProject"]));
            }
            set {
                this["EditorApplication_LastOpenedProject"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool SceneEditor_ShowGrid {
            get {
                return ((bool)(this["SceneEditor_ShowGrid"]));
            }
            set {
                this["SceneEditor_ShowGrid"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool SceneEditor_ShowShapes {
            get {
                return ((bool)(this["SceneEditor_ShowShapes"]));
            }
            set {
                this["SceneEditor_ShowShapes"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("40")]
        public int SceneEditor_GridGapSize {
            get {
                return ((int)(this["SceneEditor_GridGapSize"]));
            }
            set {
                this["SceneEditor_GridGapSize"] = value;
            }
        }
    }
}
