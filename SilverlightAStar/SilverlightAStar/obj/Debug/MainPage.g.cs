#pragma checksum "F:\My Documents\Visual Studio 2008\Projects\SilverlightAStar\SilverlightAStar\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "87A20A7CC3C84A5DE1CD25719BA97386"
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
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace SilverlightAStar {
    
    
    public partial class MainPage : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Canvas LayoutRoot;
        
        internal System.Windows.Controls.Button btnStartPoint;
        
        internal System.Windows.Controls.Button btnEndPoint;
        
        internal System.Windows.Controls.Button btnImpediment;
        
        internal System.Windows.Controls.TextBox tbGridSize;
        
        internal System.Windows.Controls.Button btnUpdate;
        
        internal System.Windows.Controls.Button btnStartFindPath;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SilverlightAStar;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Canvas)(this.FindName("LayoutRoot")));
            this.btnStartPoint = ((System.Windows.Controls.Button)(this.FindName("btnStartPoint")));
            this.btnEndPoint = ((System.Windows.Controls.Button)(this.FindName("btnEndPoint")));
            this.btnImpediment = ((System.Windows.Controls.Button)(this.FindName("btnImpediment")));
            this.tbGridSize = ((System.Windows.Controls.TextBox)(this.FindName("tbGridSize")));
            this.btnUpdate = ((System.Windows.Controls.Button)(this.FindName("btnUpdate")));
            this.btnStartFindPath = ((System.Windows.Controls.Button)(this.FindName("btnStartFindPath")));
        }
    }
}
