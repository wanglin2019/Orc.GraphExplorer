﻿#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="VertexViewBase.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Views.Base
{
    using System;
    using System.ComponentModel;
    using System.Windows;

    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.MVVM.Providers;
    using Catel.MVVM.Views;
    using Catel.Windows;

    using GraphX;

    using Orc.GraphExplorer.ViewModels;

    public abstract class VertexViewBase : VertexControl, IUserControl
    {
        private readonly UserControlLogic _logic;

        private event EventHandler<EventArgs> _viewLoaded;
        private event EventHandler<EventArgs> _viewUnloaded;
        private event EventHandler<DataContextChangedEventArgs> _viewDataContextChanged;
        private event PropertyChangedEventHandler _propertyChanged;

        protected VertexViewBase(object vertexData, bool tracePositionChange = true, bool bindToDataObject = true)
            : base(vertexData, tracePositionChange, bindToDataObject)
        {
            Argument.IsNotNull(() => vertexData);

            _logic = new UserControlLogic(this);

            _logic.ViewModelChanged += (sender, args) => ViewModelChanged.SafeInvoke(this);
            _logic.Loaded += (sender, args) => _viewLoaded.SafeInvoke(this);
            _logic.Unloaded += (sender, args) => _viewUnloaded.SafeInvoke(this);

            _logic.PropertyChanged += (sender, args) => _propertyChanged.SafeInvoke(this, args);

            this.AddDataContextChangedHandler((sender, e) => _viewDataContextChanged.SafeInvoke(this, new DataContextChangedEventArgs(e.OldValue, e.NewValue)));

            ViewModelChanged += VertexViewBase_ViewModelChanged;
            
            CloseViewModelOnUnloaded = false;
        }


        void VertexViewBase_ViewModelChanged(object sender, EventArgs e)
        {
            DataContext = ViewModel;
        }        

        IViewModel IViewModelContainer.ViewModel
        {
            get { return _logic.ViewModel; }
        }

        public VertexViewModel ViewModel
        {
            get { return _logic.ViewModel as VertexViewModel; }
        }


        public event EventHandler<EventArgs> ViewModelChanged;

        event EventHandler<EventArgs> IView.Loaded
        {
            add { _viewLoaded += value; }
            remove { _viewLoaded -= value; }
        }

        event EventHandler<EventArgs> IView.Unloaded
        {
            add { _viewUnloaded += value; }
            remove { _viewUnloaded -= value; }
        }

        event EventHandler<DataContextChangedEventArgs> IView.DataContextChanged
        {
            add { _viewDataContextChanged += value; }
            remove { _viewDataContextChanged -= value; }
        }

        public bool CloseViewModelOnUnloaded
        {
            get { return _logic.CloseViewModelOnUnloaded; }
            set { _logic.CloseViewModelOnUnloaded = value; }
        }

        public bool SupportParentViewModelContainers
        {
            get { return _logic.SupportParentViewModelContainers; }
            set { _logic.SupportParentViewModelContainers = value; }
        }

        public bool SkipSearchingForInfoBarMessageControl
        {
            get { return _logic.SkipSearchingForInfoBarMessageControl; }
            set { _logic.SkipSearchingForInfoBarMessageControl = value; }
        }

        public bool DisableWhenNoViewModel
        {
            get { return _logic.DisableWhenNoViewModel; }
            set { _logic.DisableWhenNoViewModel = value; }
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { _propertyChanged += value; }
            remove { _propertyChanged -= value; }
        }

        /// <summary>
        /// Content Dependency Property
        /// </summary>
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof (object), typeof (VertexViewBase),
                new FrameworkPropertyMetadata((object) null));

        /// <summary>
        /// Gets or sets the Content property.
        /// </summary>
        public object Content
        {
            get { return (object) GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
    }
}