﻿#pragma checksum "..\..\NewSprite.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6125E1816482F51668164B88A3DAADCC"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:2.0.50727.3053
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace MapEditor {
    
    
    /// <summary>
    /// NewSprite
    /// </summary>
    public partial class NewSprite : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 5 "..\..\NewSprite.xaml"
        internal MapEditor.NewSprite Window;
        
        #line default
        #line hidden
        
        
        #line 9 "..\..\NewSprite.xaml"
        internal System.Windows.Controls.Grid LayoutRoot;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\NewSprite.xaml"
        internal System.Windows.Controls.TextBox tbImageName;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\NewSprite.xaml"
        internal System.Windows.Controls.CheckBox cbIsAnimation;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\NewSprite.xaml"
        internal System.Windows.Controls.TextBox tbFrameNum;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\NewSprite.xaml"
        internal System.Windows.Controls.TextBox tbFrameSpeed;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\NewSprite.xaml"
        internal System.Windows.Controls.Button btnView;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\NewSprite.xaml"
        internal System.Windows.Controls.Button btnOk;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\NewSprite.xaml"
        internal System.Windows.Controls.Button btnCancel;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\NewSprite.xaml"
        internal System.Windows.Controls.Canvas canvasView;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/MapEditor;component/newsprite.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\NewSprite.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Window = ((MapEditor.NewSprite)(target));
            return;
            case 2:
            this.LayoutRoot = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.tbImageName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.cbIsAnimation = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 5:
            this.tbFrameNum = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.tbFrameSpeed = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.btnView = ((System.Windows.Controls.Button)(target));
            return;
            case 8:
            this.btnOk = ((System.Windows.Controls.Button)(target));
            return;
            case 9:
            this.btnCancel = ((System.Windows.Controls.Button)(target));
            return;
            case 10:
            this.canvasView = ((System.Windows.Controls.Canvas)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}