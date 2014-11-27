﻿#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="VertexViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Media;
    using Catel;
    using Catel.Data;
    using Catel.Fody;
    using Catel.MVVM;

    using Orc.GraphExplorer.Models;
    using Services;

    public class VertexViewModel : ViewModelBase
    {
        private readonly IGraphAreaEditorService _graphAreaEditorService;

        public VertexViewModel()
        {
            
        }

        public VertexViewModel(DataVertex dataVertex, IGraphAreaEditorService graphAreaEditorService)
        {
            Argument.IsNotNull(() => dataVertex);
            Argument.IsNotNull(() => graphAreaEditorService);

            _graphAreaEditorService = graphAreaEditorService;
            DataVertex = dataVertex;
            AddCommand = new Command(OnAddCommandExecute);
            DeleteCommand = new Command(OnDeleteCommandExecute, OnDeleteCommandCanExecute);

            DeleteVertexCommand = new Command(OnDeleteVertexCommandExecute, OnDeleteVertexCommandCanExecute);
        }

        protected override void Initialize()
        {
            base.Initialize();
            SyncWithAreaProperties();
        }

        private void SyncWithAreaProperties()
        {
            var graphAreaViewModel = GraphAreaViewModel;
            if (graphAreaViewModel == null)
            {
                return;
            }
            IsInEditing = graphAreaViewModel.IsInEditing;
            IsDragEnabled = graphAreaViewModel.IsDragEnabled;
        }

        public GraphAreaViewModel GraphAreaViewModel
        {
            get { return base.ParentViewModel as GraphAreaViewModel; }
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [Model]
        [Expose("Icon")]
        [Expose("Title")]
        [Expose("X")]
        [Expose("Y")]
        [Expose("IsVisible")]
        [Expose("IsExpanded")]
        [Expose("IsDragging")]
        public DataVertex DataVertex { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("DataVertex")]
        public ObservableCollection<Property> Properties { get; set; }

        /// <summary>
        /// Gets the AddCommand command.
        /// </summary>
        public Command AddCommand { get; private set; }

        /// <summary>
        /// Method to invoke when the AddCommand command is executed.
        /// </summary>
        private void OnAddCommandExecute()
        {
            // TODO: Handle command logic here
        }

        /// <summary>
        /// Gets the DeleteCommand command.
        /// </summary>
        public Command DeleteCommand { get; private set; }

        /// <summary>
        /// Method to check whether the DeleteCommand command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnDeleteCommandCanExecute()
        {
            return Properties != null && Properties.Count > 0;
        }

        /// <summary>
        /// Method to invoke when the DeleteCommand command is executed.
        /// </summary>
        private void OnDeleteCommandExecute()
        {
            // TODO: Handle command logic here
        }

        /// <summary>
        /// Gets the DeleteVertexCommand command.
        /// </summary>
        public Command DeleteVertexCommand { get; private set; }

        /// <summary>
        /// Method to check whether the DeleteVertexCommand command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnDeleteVertexCommandCanExecute()
        {
            return IsInEditing;
        }

        /// <summary>
        /// Method to invoke when the DeleteVertexCommand command is executed.
        /// </summary>
        private void OnDeleteVertexCommandExecute()
        {
            var areaViewModel = AreaViewModel;

            if (areaViewModel != null)
            {
                _graphAreaEditorService.RemoveVertex(areaViewModel.Area, DataVertex);
            }
        }

        public GraphAreaViewModel AreaViewModel
        {
            get
            {
                return ParentViewModel as GraphAreaViewModel;
            }
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [DefaultValue(true)]
        public bool IsHighlightEnabled { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [DefaultValue(false)]
        public bool IsHighlighted { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [DefaultValue(false)]
        public bool IsDragEnabled { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [DefaultValue(false)]
        public bool IsInEditing { get; set; }

        private void OnIsInEditingChanged()
        {
            foreach (var property in Properties)
            {
                property.IsInEditing = IsInEditing;
            }
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [DefaultValue(true)]
        public bool IsEnabled { get; set; }
    }
}