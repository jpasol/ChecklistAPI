using EquipmentChecklistDataAccess;
using EquipmentChecklistDataAccess.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ChecklistAPI.Helpers
{
    public class Validator
    {
        public static async Task Ensure5Characters(Component component)
        {
            if (component.ID.Length > 5) throw new Exception("Component ID has more than 5 characters");
        }
        public static async Task Ensure5Characters(Condition condition)
        {
            if (condition.ID.Length > 5) throw new Exception("Condition ID has more than 5 characters");
        }

        public static async Task Ensure5Characters(Equipment equipment)
        {
            if (equipment.ID.Length > 5) throw new Exception("Equipment ID has more than 5 characters");
        }

        public static async Task EnsureEquipmentTypeID(Equipment equipment)
        {
            if (equipment.Equipment_TypeID == null || equipment.Equipment_TypeID.Trim().Length == 0) throw new Exception("Equipment Type ID is null or empty ");
        }

        public static async Task EnsureEquipmentIsUnusedInContext(EquipmentChecklistDBContext context, Equipment equipment)
        {
            var checklistFound = context.Checklists.Where(x => x.EquipmentID == equipment.ID).ToArray();
            if (checklistFound != null) throw new InvalidOperationException("Equipment ID is used in a checklist, doing this will corrupt the data's integrity");
        }
    }
}
