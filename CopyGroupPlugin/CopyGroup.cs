using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyGroupPlugin
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class CopyGroup : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //обращаемся к текущему документу
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            //получаем ссылку на группу
            Reference reference = uiDoc.Selection.PickObject(ObjectType.Element, "Выберете группу объектов");
            Element element = doc.GetElement(reference);//преобразуем в тип Element
            Group group = element as Group;//преобразуем элемент в группу
            //выбираем точку куда будем копировать группу
            XYZ point = uiDoc.Selection.PickPoint("Выберите точку");
            //создаем транзакцию
            Transaction transaction = new Transaction(doc);
            transaction.Start("Копирование группы объектов");
            doc.Create.PlaceGroup(point, group.GroupType);//создаем новую группу в указанной точке
            transaction.Commit();

            return Result.Succeeded;
        }
    }
}
