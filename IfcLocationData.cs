using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xbim.Ifc;
using Xbim.Common.Collections;
using Xbim.Ifc2x3.Interfaces;
using Xbim.Ifc2x3.MeasureResource;

namespace IfcSiteLocationCoordinates
{
    class IfcLocationData
    {
        public IfcLocationData(string path)
        {

            using (IfcStore model = IfcStore.Open(path))
            {
                IIfcProject project = model.Instances.FirstOrDefault<IIfcProject>();
                AuthoringTool = project.OwnerHistory.OwningApplication.ApplicationFullName;

                IIfcSIUnit lengtUnit = project.UnitsInContext.Units.FirstOrDefault<IIfcSIUnit>(q => q.UnitType == IfcUnitEnum.LENGTHUNIT);
                LengthUnit = lengtUnit.FullName;

                IIfcSite site = model.Instances.FirstOrDefault<IIfcSite>();
                refElevation = site.RefElevation.Value;
                IIfcLocalPlacement placement = site.ObjectPlacement as IIfcLocalPlacement;
                IIfcAxis2Placement3D axis2Placement = placement.RelativePlacement as IIfcAxis2Placement3D;
                double x = axis2Placement.RefDirection.DirectionRatios.GetAt(0);
                double y = axis2Placement.RefDirection.DirectionRatios.GetAt(1);
                double angle = 360-Math.Atan2(y, x)*180/Math.PI;
                Orientation = angle;

                EW = axis2Placement.Location.Coordinates.GetAt(0);
                NS = axis2Placement.Location.Coordinates.GetAt(1);
                elevation = axis2Placement.Location.Coordinates.GetAt(2);

            }
        }

        public double refElevation { get; set; }
        public double Orientation { get; set; }
        public double EW { get; set; }
        public double NS { get; set; }
        public double elevation { get; set; }

        public string AuthoringTool { get; set; }
        public string LengthUnit { get; set; }

    }
}
