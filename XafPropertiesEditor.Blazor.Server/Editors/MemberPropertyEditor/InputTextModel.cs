using DevExpress.ExpressApp.Blazor.Components.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using XafPropertiesEditor.Module.BusinessObjects;

namespace XafPropertiesEditor.Blazor.Server.Editors.MemberPropertyEditor
{


    [PropertyEditor(typeof(PropertyObject), true)]
    public class CustomStringPropertyEditor : BlazorPropertyEditorBase
    {
        public CustomStringPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) { }
        public override InputTextModel ComponentModel => (InputTextModel)base.ComponentModel;
        protected override IComponentModel CreateComponentModel()
        {
            
            var model = new InputTextModel();
            model.ValueExpression = () => model.Value;
            model.ValueChanged = EventCallback.Factory.Create<PropertyObject>(this, value => {
                model.Value = value;
                OnControlValueChanged();
                WriteValue();
            });
            return model;
        }
        protected override void ReadValueCore()
        {
            base.ReadValueCore();
            ComponentModel.Value = (PropertyObject)PropertyValue;
        }
        protected override object GetControlValueCore() => ComponentModel.Value;
        protected override void ApplyReadOnly()
        {
            base.ApplyReadOnly();
            ComponentModel?.SetAttribute("readonly", !AllowEdit);
        }
    }
    public class InputTextModel : ComponentModelBase
    {

        public PropertyObject Value
        {
            get => GetPropertyValue<PropertyObject>();
            set => SetPropertyValue(value);
        }
        public EventCallback<PropertyObject> ValueChanged
        {
            get => GetPropertyValue<EventCallback<PropertyObject>>();
            set => SetPropertyValue(value);
        }
        public Expression<Func<PropertyObject>> ValueExpression
        {
            get => GetPropertyValue<Expression<Func<PropertyObject>>>();
            set => SetPropertyValue(value);
        }
        public override Type ComponentType => typeof(MemberComponent);
    }
}
