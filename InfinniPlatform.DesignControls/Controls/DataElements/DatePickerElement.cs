﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.DesignControls.Controls.Properties;
using InfinniPlatform.DesignControls.Layout;
using InfinniPlatform.DesignControls.ObjectInspector;
using InfinniPlatform.DesignControls.PropertyDesigner;
using InfinniPlatform.DesignControls.PropertyEditors;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.DesignControls.Controls.DataElements
{
    public partial class DatePickerElement : UserControl, IPropertiesProvider, ILayoutProvider, IClientHeightProvider,
        IInspectedItem
    {
        private readonly Dictionary<string, IControlProperty> _simpleProperties =
            new Dictionary<string, IControlProperty>();

        public DatePickerElement()
        {
            InitializeComponent();

            DateEdit.EditValue = DateTime.Now;

            InitProperties();
        }

        public int GetClientHeight()
        {
            return 32;
        }

        public bool IsFixedHeight()
        {
            return true;
        }

        public ObjectInspectorTree ObjectInspector { get; set; }

        public dynamic GetLayout()
        {
            dynamic instanceLayout = new DynamicWrapper();
            DesignerExtensions.SetSimplePropertiesToInstance(_simpleProperties, instanceLayout);
            return instanceLayout;
        }

        public void SetLayout(dynamic value)
        {
            //no inner layout
        }

        public string GetPropertyName()
        {
            return "DatePicker";
        }

        public void ApplySimpleProperties()
        {
        }

        public void ApplyCollections()
        {
        }

        public Dictionary<string, IControlProperty> GetSimpleProperties()
        {
            return _simpleProperties;
        }

        public Dictionary<string, CollectionProperty> GetCollections()
        {
            return new Dictionary<string, CollectionProperty>();
        }

        public void LoadProperties(dynamic value)
        {
            DesignerExtensions.SetSimplePropertiesFromInstance(_simpleProperties, value);
        }

        public Dictionary<string, Func<IPropertyEditor>> GetPropertyEditors()
        {
            return new Dictionary<string, Func<IPropertyEditor>>
            {
                {"Mode", () => new ValueListEditor(new[] {"Date", "Time", "DateTime"})}
            }
                .InheritBaseElementPropertyEditors(ObjectInspector)
                .InheritBindingPropertyEditors(ObjectInspector);
        }

        public Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> GetValidationRules()
        {
            return new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>
            {
                {"Mode", Common.CreateNullOrEmptyValidator("DatePicker", "Mode")}
            }.InheritBaseElementValidators("DatePicker");
        }

        private void InitProperties()
        {
            _simpleProperties.InheritBaseElementSimpleProperties();
            _simpleProperties.InheritBaseElementValueBinding();
            _simpleProperties.Add("Mode", new SimpleProperty("Date"));
            _simpleProperties.Add("MinDate", new SimpleProperty(null));
            _simpleProperties.Add("MaxDate", new SimpleProperty(null));
            _simpleProperties.Add("ReadOnly", new SimpleProperty(false));
            _simpleProperties.Add("OnValueChanged", new ObjectProperty(new Dictionary<string, IControlProperty>
            {
                {"Name", new SimpleProperty(string.Empty)}
            }, new Dictionary<string, CollectionProperty>()));
        }
    }
}