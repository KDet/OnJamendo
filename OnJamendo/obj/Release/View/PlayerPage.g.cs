﻿

#pragma checksum "E:\Copy\Projects\OnJamendo\OnJamendo\View\PlayerPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "25448E827FD39449FE1F667A27A2F043"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnJamendo.View
{
    partial class PlayerPage : global::OnJamendo.View.BaseView, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 95 "..\..\View\PlayerPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.PlayListlistView_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 48 "..\..\View\PlayerPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.GoBack;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 54 "..\..\View\PlayerPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.GoForward;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


