using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleParameterRenumbering
{
    class FamilyOption : IFamilyLoadOptions
    {
        public bool OnFamilyFound(
          bool familyInUse,
          out bool overwriteParameterValues)
        {
            overwriteParameterValues = true;
            return true;
        }

        public bool OnSharedFamilyFound(
          Family sharedFamily,
          bool familyInUse,
          out FamilySource source,
          out bool overwriteParameterValues)
        {
            source = FamilySource.Project;
            overwriteParameterValues = sharedFamily != null;
            return true;
        }
    }
}
