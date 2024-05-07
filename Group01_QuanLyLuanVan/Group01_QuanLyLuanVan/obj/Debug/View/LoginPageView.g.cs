﻿#pragma checksum "..\..\..\View\LoginPageView.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "71622D80BBA28540A828CA592D0BBA752DC14CBAC7BABAA8CD42D025B2B5D3A1"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Group01_QuanLyLuanVan.View;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using Microsoft.Xaml.Behaviors;
using Microsoft.Xaml.Behaviors.Core;
using Microsoft.Xaml.Behaviors.Input;
using Microsoft.Xaml.Behaviors.Layout;
using Microsoft.Xaml.Behaviors.Media;
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
using System.Windows.Shell;


namespace Group01_QuanLyLuanVan.View {
    
    
    /// <summary>
    /// LoginPageView
    /// </summary>
    public partial class LoginPageView : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\View\LoginPageView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Group01_QuanLyLuanVan.View.LoginPageView LoginWd;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\View\LoginPageView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel PasswordContainer;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\View\LoginPageView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Password11;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\..\View\LoginPageView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox PasswordBox;
        
        #line default
        #line hidden
        
        
        #line 88 "..\..\..\View\LoginPageView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox ShowPasswordCheckBox;
        
        #line default
        #line hidden
        
        
        #line 104 "..\..\..\View\LoginPageView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock LoginButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Group01_QuanLyLuanVan;component/view/loginpageview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\View\LoginPageView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.LoginWd = ((Group01_QuanLyLuanVan.View.LoginPageView)(target));
            return;
            case 2:
            this.PasswordContainer = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 3:
            this.Password11 = ((System.Windows.Controls.TextBox)(target));
            
            #line 66 "..\..\..\View\LoginPageView.xaml"
            this.Password11.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.Password11_TextChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.PasswordBox = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 5:
            this.ShowPasswordCheckBox = ((System.Windows.Controls.CheckBox)(target));
            
            #line 89 "..\..\..\View\LoginPageView.xaml"
            this.ShowPasswordCheckBox.Checked += new System.Windows.RoutedEventHandler(this.ShowPassword_Checked);
            
            #line default
            #line hidden
            
            #line 89 "..\..\..\View\LoginPageView.xaml"
            this.ShowPasswordCheckBox.Unchecked += new System.Windows.RoutedEventHandler(this.ShowPassword_Unchecked);
            
            #line default
            #line hidden
            return;
            case 6:
            this.LoginButton = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            
            #line 123 "..\..\..\View\LoginPageView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OpenSignUp);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

