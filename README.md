# DescendantCollectionstest

based on devexpress xaf example: https://supportcenter.devexpress.com/ticket/details/e975/how-to-display-detail-collections-with-descendants-filtered-by-an-object-type



After remove      remove[ModelDefault("AllowEdit", "False")] i can add records to descendant collections but there department is not stored.

```csharp
protected override XPCollection<T> CreateCollection<T>(XPMemberInfo property)
     {
         XPCollection<T> collection = base.CreateCollection<T>(property);
         if (property.Name == "Employees")
         {
             collection.CollectionChanged += collectionEmployees_CollectionChanged;
         }

         if (property.Name == "ForeignEmployees")
         {
             collection.CollectionChanged += collectionForeignEmployees_CollectionChanged;
         }

         if (property.Name == "LocalEmployees")
         {
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
```


but while debugging i realized that CreateCollection is not called for my descendant collection:



```csharp
...
XPCollection<T> collection = base.CreateCollection<T>(property);
if (property.Name == "Employees")
{
    collection.CollectionChanged += collectionEmployees_CollectionChanged;
}

if (property.Name == "ForeignEmployees")
{
    //this never happens
    collection.CollectionChanged += collectionForeignEmployees_CollectionChanged;
}

if (property.Name == "LocalEmployees")
{
    //this never happens
    collection.CollectionChanged += collection_LocalEmployeesCollectionChanged;
}
...
```


The question is how to process this event for descendant collect  if it is not fired ?

Now I'm trying to another direction: controller on EmployeeBase  newObjectCreated:

```csharp
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
```


Now I can Add records to descendant collections and parent is assigned!

but ...

