﻿using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;
using SharpMap.Layers;
using System.Drawing;
#if DotSpatialProjections
using DotSpatial.Projections;
using SharpMap.Styles;

#else
using ProjNet.CoordinateSystems.Transformations;
using ProjNet.CoordinateSystems;
#endif

namespace WinFormSamples
{
    static class LayerTools
    {

        private static ICoordinateTransformation dhdn2towgs84;
        private static ICoordinateTransformation wgs84toGoogle;
        private static ICoordinateTransformation googletowgs84;

        /// <summary>
        /// Wgs84 to Google Mercator Coordinate Transformation
        /// </summary>
        public static ICoordinateTransformation Wgs84toGoogleMercator
        {
            get
            {

                if (wgs84toGoogle == null)
                {
#if DotSpatialProjections
                    wgs84toGoogle = new CoordinateTransformation()
                                        {
                                            Source = KnownCoordinateSystems.Geographic.World.WGS1984,
                                            Target = KnownCoordinateSystems.Projected.World.WebMercator
                                        };
#else
                    CoordinateSystemFactory csFac = new ProjNet.CoordinateSystems.CoordinateSystemFactory();
                    CoordinateTransformationFactory ctFac = new CoordinateTransformationFactory();

                    IGeographicCoordinateSystem wgs84 = csFac.CreateGeographicCoordinateSystem(
                      "WGS 84", AngularUnit.Degrees, HorizontalDatum.WGS84, PrimeMeridian.Greenwich,
                      new AxisInfo("north", AxisOrientationEnum.North), new AxisInfo("east", AxisOrientationEnum.East));

                    List<ProjectionParameter> parameters = new List<ProjectionParameter>();
                    parameters.Add(new ProjectionParameter("semi_major", 6378137.0));
                    parameters.Add(new ProjectionParameter("semi_minor", 6378137.0));
                    parameters.Add(new ProjectionParameter("latitude_of_origin", 0.0));
                    parameters.Add(new ProjectionParameter("central_meridian", 0.0));
                    parameters.Add(new ProjectionParameter("scale_factor", 1.0));
                    parameters.Add(new ProjectionParameter("false_easting", 0.0));
                    parameters.Add(new ProjectionParameter("false_northing", 0.0));
                    IProjection projection = csFac.CreateProjection("Google Mercator", "mercator_1sp", parameters);

                    IProjectedCoordinateSystem epsg900913 = csFac.CreateProjectedCoordinateSystem(
                      "Google Mercator", wgs84, projection, LinearUnit.Metre, new AxisInfo("East", AxisOrientationEnum.East),
                      new AxisInfo("North", AxisOrientationEnum.North));

                    wgs84toGoogle = ctFac.CreateFromCoordinateSystems(wgs84, epsg900913);
#endif
                }

                return wgs84toGoogle;

            }
        }


        /// <summary>
        /// Wgs84 to Google Mercator Coordinate Transformation
        /// </summary>
        public static ICoordinateTransformation Dhdn2ToWgs84
        {
            get
            {

                if (wgs84toGoogle == null)
                {
#if DotSpatialProjections
                    var piSource = new ProjectionInfo();
                    piSource.ReadEPSGCode(31466);

                    wgs84toGoogle = new CoordinateTransformation()
                    {
                        Target = KnownCoordinateSystems.Geographic.World.WGS1984,
                        Source = piSource
                    };
#else
                    CoordinateSystemFactory csFac = new ProjNet.CoordinateSystems.CoordinateSystemFactory();
                    CoordinateTransformationFactory ctFac = new CoordinateTransformationFactory();

                    IGeographicCoordinateSystem wgs84 = csFac.CreateGeographicCoordinateSystem(
                      "WGS 84", AngularUnit.Degrees, HorizontalDatum.WGS84, PrimeMeridian.Greenwich,
                      new AxisInfo("north", AxisOrientationEnum.North), new AxisInfo("east", AxisOrientationEnum.East));

                    List<ProjectionParameter> parameters = new List<ProjectionParameter>();
                    parameters.Add(new ProjectionParameter("semi_major", 6378137.0));
                    parameters.Add(new ProjectionParameter("semi_minor", 6378137.0));
                    parameters.Add(new ProjectionParameter("latitude_of_origin", 0.0));
                    parameters.Add(new ProjectionParameter("central_meridian", 0.0));
                    parameters.Add(new ProjectionParameter("scale_factor", 1.0));
                    parameters.Add(new ProjectionParameter("false_easting", 0.0));
                    parameters.Add(new ProjectionParameter("false_northing", 0.0));
                    IProjection projection = csFac.CreateProjection("Google Mercator", "mercator_1sp", parameters);

                    IProjectedCoordinateSystem epsg900913 = csFac.CreateProjectedCoordinateSystem(
                      "Google Mercator", wgs84, projection, LinearUnit.Metre, new AxisInfo("East", AxisOrientationEnum.East),
                      new AxisInfo("North", AxisOrientationEnum.North));

                    wgs84toGoogle = ctFac.CreateFromCoordinateSystems(wgs84, epsg900913);
#endif
                }

                return dhdn2towgs84;

            }
        }

        public static ICoordinateTransformation GoogleMercatorToWgs84
        {
            get
            {


                if (googletowgs84 == null)
                {
#if DotSpatialProjections
                    googletowgs84 = new CoordinateTransformation()
                                        {
                                            Source = KnownCoordinateSystems.Projected.World.WebMercator,
                                            Target = KnownCoordinateSystems.Geographic.World.WGS1984
                                        };
#else

                    CoordinateSystemFactory csFac = new ProjNet.CoordinateSystems.CoordinateSystemFactory();
                    CoordinateTransformationFactory ctFac = new CoordinateTransformationFactory();

                    IGeographicCoordinateSystem wgs84 = csFac.CreateGeographicCoordinateSystem(
                      "WGS 84", AngularUnit.Degrees, HorizontalDatum.WGS84, PrimeMeridian.Greenwich,
                      new AxisInfo("north", AxisOrientationEnum.North), new AxisInfo("east", AxisOrientationEnum.East));

                    List<ProjectionParameter> parameters = new List<ProjectionParameter>();
                    parameters.Add(new ProjectionParameter("semi_major", 6378137.0));
                    parameters.Add(new ProjectionParameter("semi_minor", 6378137.0));
                    parameters.Add(new ProjectionParameter("latitude_of_origin", 0.0));
                    parameters.Add(new ProjectionParameter("central_meridian", 0.0));
                    parameters.Add(new ProjectionParameter("scale_factor", 1.0));
                    parameters.Add(new ProjectionParameter("false_easting", 0.0));
                    parameters.Add(new ProjectionParameter("false_northing", 0.0));
                    IProjection projection = csFac.CreateProjection("Google Mercator", "mercator_1sp", parameters);

                    IProjectedCoordinateSystem epsg900913 = csFac.CreateProjectedCoordinateSystem(
                      "Google Mercator", wgs84, projection, LinearUnit.Metre, new AxisInfo("East", AxisOrientationEnum.East),
                      new AxisInfo("North", AxisOrientationEnum.North));

                    googletowgs84 = ctFac.CreateFromCoordinateSystems(epsg900913, wgs84);
#endif
                }

                return googletowgs84;

            }
        }

        public static LabelLayer CreateLabelLayer(VectorLayer originalLayer, string labelColumnName)
        {
            SharpMap.Layers.LabelLayer labelLayer = new SharpMap.Layers.LabelLayer(originalLayer.LayerName + ":Labels");
            labelLayer.DataSource = originalLayer.DataSource;
            labelLayer.LabelColumn = labelColumnName;
            labelLayer.Style.CollisionDetection = true;
            labelLayer.Style.CollisionBuffer = new SizeF(10F, 10F);
            labelLayer.LabelFilter = SharpMap.Rendering.LabelCollisionDetection.ThoroughCollisionDetection;
            labelLayer.Style.Offset = new PointF(0, -5F);
            labelLayer.MultipartGeometryBehaviour = SharpMap.Layers.LabelLayer.MultipartGeometryBehaviourEnum.CommonCenter;
            labelLayer.Style.Font = new Font(FontFamily.GenericSansSerif, 12);
            labelLayer.MaxVisible = originalLayer.MaxVisible;
            labelLayer.MinVisible = originalLayer.MinVisible;
            labelLayer.Style.Halo = new Pen(Color.White, 2);
            labelLayer.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            labelLayer.CoordinateTransformation = originalLayer.CoordinateTransformation;
            return labelLayer;
        }

        private static readonly Random Rnd = new Random();
        public static Color GetRandomColor()
        {
            return  Color.FromArgb(Rnd.Next(0, 127), Rnd.Next(0, 255), Rnd.Next(0,255), Rnd.Next(0,255));
        }

        public static Boolean GetRandomBoolean()
        {
            return Rnd.Next(0, 2) == 1;
        }

        public static Brush GetRandomBrush()
        {
            return new SolidBrush(GetRandomColor());
        }

        public static Pen GetRandomPen()
        {
            return GetRandomPen(1, 4);
        }

        /// <summary>
        /// Generates a random symbol.
        /// </summary>
        /// <returns></returns>
        public static Bitmap GetRandomSymbol()
        {
            var s = Rnd.Next(4, 12);
            var f = new Font("WingDings", 2*s, GraphicsUnit.Pixel);
            var bmp = new Bitmap(2*s + 2, 2*s + 2, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                g.DrawString(new string(Convert.ToChar((byte) Rnd.Next(15, 127)), 1), f, GetRandomBrush(), new PointF(s, s) ,
                             new StringFormat
                                 {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center});
            }

            return bmp;
        }

        public static SharpMap.Styles.VectorStyle GetRandomVectorStyle()
        {
            return new SharpMap.Styles.VectorStyle
                       {
                           EnableOutline = GetRandomBoolean(),
                           Outline = GetRandomPen(3, 7),
                           Line = GetRandomPen(1, 4),
                           Fill = GetRandomBrush(),
                           Symbol = GetRandomSymbol(),
                           //SymbolRotation = Rnd.Next(0, 359),
                       };
        }

        private static Pen GetRandomPen(int min, int max)
        {
            return new Pen(GetRandomColor(), Rnd.Next(min, max));
        }

        public static SharpMap.Map GetMapForProviders(IEnumerable<SharpMap.Data.Providers.IProvider> providers)
        {
            var m = new SharpMap.Map();
            foreach (var provider in providers)
            {
                var l = new VectorLayer(provider.ConnectionID, provider)
                            {
                                Style = GetRandomVectorStyle()
                            };
                m.Layers.Add(l);
            }
            m.ZoomToExtents();
            return m;
        }
    }
}
