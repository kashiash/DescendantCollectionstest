using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSolution.Module;

namespace WinWebSolution.Module
{
    public class EmployeeBaseViewController : ObjectViewController<ListView, EmployeeBase>
    {


        private NewObjectViewController newObjectViewController;


        protected override void OnActivated()
        {
            base.OnActivated();
            NestedFrame nestedFrame = Frame as NestedFrame;
            if (nestedFrame != null)
            {
                newObjectViewController = Frame.GetController<NewObjectViewController>();
                newObjectViewController.ObjectCreated += newObjectViewController_ObjectCreated;

            }
        }

        void newObjectViewController_ObjectCreated(object sender, ObjectCreatedEventArgs e)
        {
            var objectSpace = e.ObjectSpace;
            var parent = objectSpace.GetObject(((NestedFrame)Frame).ViewItem.CurrentObject as Department);
            var item = e.CreatedObject as EmployeeBase;
            item.Department = parent;
        }
    }
}
