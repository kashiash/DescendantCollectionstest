using System;

using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;

namespace WinWebSolution.Module.Win {
    public class Updater : ModuleUpdater {
        public Updater(DevExpress.ExpressApp.IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
        }
    }
}
