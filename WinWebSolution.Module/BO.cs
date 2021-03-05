using DevExpress.ExpressApp.Model;
using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Data.Filtering;
using System.ComponentModel;
using System.Collections;
using DevExpress.Xpo.Metadata;

namespace WinSolution.Module {
    [DefaultClassOptions]
    [System.ComponentModel.DefaultProperty("Title")]
    public class Department : BaseObject {
        WinWebSolution.Module.Organization organization;
        private string title;
        private string office;
        public Department(Session session) : base(session) { }
        public string Title
        {
            get { return title; }
            set
            {
                SetPropertyValue("Title", ref title, value);
            }
        }
        public string Office
        {
            get { return office; }
            set
            {
                SetPropertyValue("Office", ref office, value);
            }
        }

               
        [Association("Organization-Departments")]
        public WinWebSolution.Module.Organization Organization
        {
            get => organization;
            set => SetPropertyValue(nameof(Organization), ref organization, value);
        }


        [Association("Department-Employees"), Aggregated]
        public XPCollection<EmployeeBase> Employees {
            get { return GetCollection<EmployeeBase>("Employees"); }
        }
        private XPCollection<LocalEmployee> _LocalEmployees;
        // [ModelDefault("AllowEdit", "False")]
        [Aggregated]
        public XPCollection<LocalEmployee> LocalEmployees {
            get {
                if (_LocalEmployees == null)
                    _LocalEmployees = new XPCollection<LocalEmployee>(PersistentCriteriaEvaluationBehavior.InTransaction, Session,
                        new GroupOperator(
                        new BinaryOperator(BaseObject.Fields.ObjectType.TypeName, new OperandValue(typeof(LocalEmployee).FullName), BinaryOperatorType.Equal),
                        new BinaryOperator("Department", this)));
                return _LocalEmployees;
            }
        }
        private XPCollection<ForeignEmployee> _ForeignEmployees;
        // [ModelDefault("AllowEdit", "False")]
        [Aggregated]
        public XPCollection<ForeignEmployee> ForeignEmployees {
            get {
                if (_ForeignEmployees == null)
                    _ForeignEmployees = new XPCollection<ForeignEmployee>(PersistentCriteriaEvaluationBehavior.InTransaction, Session,
                        new GroupOperator(
                        new BinaryOperator(BaseObject.Fields.ObjectType.TypeName, new OperandValue(typeof(ForeignEmployee).FullName), BinaryOperatorType.Equal),
                        new BinaryOperator("Department", this)));

                return _ForeignEmployees;
            }
        }
        protected override void OnLoaded() {
            base.OnLoaded();
            UpdateCollections();
        }
        public void UpdateCollections() {
            LocalEmployees.Reload();
            ForeignEmployees.Reload();
        }


        protected override XPCollection<T> CreateCollection<T>(XPMemberInfo property)
        {
            XPCollection<T> collection = base.CreateCollection<T>(property);
            if (property.Name == "Employees")
            {
                collection.CollectionChanged += collectionEmployees_CollectionChanged;
            }

            if (property.Name == "ForeignEmployees")
            {
                //this never happens
              
            }

            if (property.Name == "LocalEmployees")
            {
                //this never happens
                collection.CollectionChanged += collection_LocalEmployeesCollectionChanged;
            }
            return collection;
        }

        private void collection_LocalEmployeesCollectionChanged(object sender, XPCollectionChangedEventArgs e)
        {
            ProceessChange(e);
        }

        private void collectionForeignEmployees_CollectionChanged(object sender, XPCollectionChangedEventArgs e)
        {
            ProceessChange(e);
        }

        void collectionEmployees_CollectionChanged(object sender, XPCollectionChangedEventArgs e)
        {
            ProceessChange(e);
        }

        private void ProceessChange(XPCollectionChangedEventArgs e)
        {
            if (e.CollectionChangedType == XPCollectionChangedType.AfterAdd || e.CollectionChangedType == XPCollectionChangedType.AfterRemove)
            {
                var employee = e.ChangedObject as EmployeeBase;
                if (employee != null)
                {
                    employee.Department = this;
                }
            }
        }
    }





    [DefaultClassOptions]
    public abstract class EmployeeBase : BaseObject {
        public EmployeeBase(Session session) : base(session) { }
        private string name;
        private string email;
        public string Name {
            get { return name; }
            set {
                SetPropertyValue("Name", ref name, value);
            }
        }
        public string Email {
            get { return email; }
            set {
                SetPropertyValue("Email", ref email, value);
            }
        }
        private Department department;
        [Association("Department-Employees")]
        public Department Department {
            get { return department; }
            set {
                Department oldDepartment = department;
                SetPropertyValue("Department", ref department, value);
                if (!IsLoading && !IsSaving && oldDepartment != department) {
                    if (oldDepartment != null) oldDepartment.UpdateCollections();
                    if (department != null) department.UpdateCollections();
                }
            }
        }
        protected override void OnSaved() {
            base.OnSaved();
            if (Department != null) this.Department.UpdateCollections();
        }
    }
    public class LocalEmployee : EmployeeBase {
        public LocalEmployee(Session session) : base(session) { }
        private string insurancePolicyNumber;
        public string InsurancePolicyNumber {
            get { return insurancePolicyNumber; }
            set {
                SetPropertyValue("InsurancePolicyNumber", ref insurancePolicyNumber, value);
            }
        }
    }
    public class ForeignEmployee : EmployeeBase {
        public ForeignEmployee(Session session) : base(session) { }
        private DateTime visaExpirationDate;
        public DateTime VisaExpirationDate {
            get { return visaExpirationDate; }
            set {
                SetPropertyValue("VisaExpirationDate", ref visaExpirationDate, value);
            }
        }
    }

}