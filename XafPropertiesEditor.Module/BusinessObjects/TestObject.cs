using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using static DevExpress.CodeParser.CodeStyle.Formatting.Rules.LineBreaks;

namespace XafPropertiesEditor.Module.BusinessObjects
{
    public interface IDataType: INotifyPropertyChanged
    {
        Type DataType { get; set; }
        string SelectedProperty { get; set; }
    }
   
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class TestObject : BaseObject,IXafEntityObject//, ICheckedListBoxItemsProvider, INotifyPropertyChanged
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public TestObject(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        public void OnCreated()
        {
            this.Member=new PropertyObject();
            this.Member.Session = this.Session;
            this.Member.PropertyChanged += Member_PropertyChanged;
        }

        private void Member_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.SelectedProperty= Member.SelectedProperty;
        }
        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if(propertyName == nameof(DataType))
            {
                if(!this.IsLoading && !this.IsSaving && this.Member!=null)
                    this.Member.DataType = this.DataType;
            }
        }
        void IXafEntityObject.OnSaving()
        {
            this.SelectedProperty = Member.SelectedProperty;
        }

        void IXafEntityObject.OnLoaded()
        {
            if(this.Member!=null)
            {
                this.Member.DataType = this.DataType;
                this.Member.Session = this.Session;
                this.Member.SelectedProperty = this.SelectedProperty;

            }
        }

        [ValueConverter(typeof(TypeToStringConverter))]
        [TypeConverter(typeof(LocalizedClassInfoTypeConverter))]
        [Size(SizeAttribute.Unlimited)]
        public Type DataType
        {
            get { return GetPropertyValue<Type>(nameof(DataType)); }
            set { SetPropertyValue<Type>(nameof(DataType), value); }
        }

        PropertyObject member;
        string selectedProperty;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string SelectedProperty
        {
            get => selectedProperty;
            set => SetPropertyValue(nameof(SelectedProperty), ref selectedProperty, value);
        }
        
        public PropertyObject Member
        {
            get => member;
            set => SetPropertyValue(nameof(Member), ref member, value);
        }
        


    }
}