using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xbim.Ifc;
using Xbim.Common.Collections;
using Xbim.Common.Step21;
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
                if (model.SchemaVersion == XbimSchemaVersion.Ifc2X3)
                {
                    Schema = "Ifc 2x3";
                    SchemaIsSupported = true;
                }
                else if (model.SchemaVersion == XbimSchemaVersion.Ifc4)
                {
                    Schema = "Ifc4";
                    SchemaIsSupported = false;
                    return;
                }
                else
                {
                    Schema = model.SchemaVersion.ToString();
                    SchemaIsSupported = false;
                    return;
                }
                IIfcProject project = model.Instances.FirstOrDefault<IIfcProject>();
                string applicationFullname = project.OwnerHistory.OwningApplication.ApplicationFullName;
                if (applicationFullname == null)
                {
                    AuthoringTool = "N/A";
                }
                else
                {
                    AuthoringTool = project.OwnerHistory.OwningApplication.ApplicationFullName;
                }


                IIfcSIUnit lengtUnit = project.UnitsInContext.Units.FirstOrDefault<IIfcSIUnit>(q => q.UnitType == IfcUnitEnum.LENGTHUNIT);
                LengthUnit = lengtUnit.FullName;

                IIfcSite site = model.Instances.FirstOrDefault<IIfcSite>();
                if (site.RefElevation.HasValue)
                {
                    refElevation = site.RefElevation.Value;
                }
                else
                {
                    refElevation = double.NaN;
                }
                IIfcLocalPlacement placement = site.ObjectPlacement as IIfcLocalPlacement;
                IIfcAxis2Placement3D axis2Placement = placement.RelativePlacement as IIfcAxis2Placement3D;
                double x = axis2Placement.RefDirection.DirectionRatios.GetAt(0);
                double y = axis2Placement.RefDirection.DirectionRatios.GetAt(1);
                double angle = 360 - Math.Atan2(y, x) * 180 / Math.PI;
                if (angle.Equals(360))
                {
                    angle = 0;
                }
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
        public string Schema { get; set; }
        public bool SchemaIsSupported {get; set; }

    }
}
