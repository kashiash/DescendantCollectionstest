using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSolution.Module;

namespace WinWebSolution.Module
{
    [DefaultClassOptions]
    public class Organization : XPObject
    {
        public Organization(Session session) : base(session)
        { }


        string organizationName;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string OrganizationName
        {
            get => organizationName;
            set => SetPropertyValue(nameof(OrganizationName), ref organizationName, value);
        }


        [Association("Organization-Departments"),Aggregated]
        public XPCollection<Department> Departments
        {
            get
            {
                return GetCollection<Department>(nameof(Departments));
            }
        }
    }
}
