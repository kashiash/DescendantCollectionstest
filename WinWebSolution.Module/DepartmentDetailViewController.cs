using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSolution.Module;

namespace WinWebSolution.Module
{
  public  class DepartmentDetailViewController : ObjectViewController<DetailView, Department>
    {

        SimpleAction simpleAction;
        public DepartmentDetailViewController()
        {
            simpleAction = new SimpleAction(this, $"{ GetType().FullName}{nameof(simpleAction) }", DevExpress.Persistent.Base.PredefinedCategory.Unspecified)
            {

                Caption = "Add local employees",
                ImageName="BO_Skull",
            };
            simpleAction.Execute += SimpleAction_Execute;
        }

        private void SimpleAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

            var dept = View.CurrentObject as Department;
            LocalEmployee le2 = ObjectSpace.CreateObject<LocalEmployee>();
            le2.Department = dept;
            le2.Name = "LocalEmployee 2";
            LocalEmployee le3 = ObjectSpace.CreateObject<LocalEmployee>();
            le3.Name = "LocalEmployee 3";
            le3.Department = dept;
            LocalEmployee le4 = ObjectSpace.CreateObject<LocalEmployee>();
            le4.Name = "LocalEmployee 4";
            le4.Department = dept;
            LocalEmployee le5 = ObjectSpace.CreateObject<LocalEmployee>();
            le5.Name = "LocalEmployee 5";
            le5.Department = dept;


            dept.LocalEmployees.AddRange(new LocalEmployee[] { le2, le3,le4,le5 });

            View.FindItem("Employees").Refresh();
            View.FindItem("LocalEmployees").Refresh();
            ObjectSpace.SetModified(View.CurrentObject);
        }
    }
}
