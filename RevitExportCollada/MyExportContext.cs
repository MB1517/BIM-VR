#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using System.Text;
#endregion

namespace RevitExportCollada
{
    class MyExportContext : IExportContext
    {
        #region Custom Information
        Document doc = null;

        string elementID="0";
        
        Dictionary<uint, ElementId> EXTRAINFO = new Dictionary<uint, ElementId>();
        ElementId elemId;
        
        string guid, cat, fam, typ, comm, mark, vol, SA, FA, FP;

        string RevitElemIdStart = "<RevitElemId>";
        string RevitElemIdEnd = "</RevitElemId>";

        string RevitElemGuidStart = "<RevitElemGuid>";
        string RevitElemGuidEnd = "</RevitElemGuid>";

        string RevitElemCatStart = "<RevitElemCat>";
        string RevitElemCatEnd = "</RevitElemCat>";

        string RevitElemFamilyStart = "<RevitElemFamily>";
        string RevitElemFamilyEnd = "</RevitElemFamily>";

        string RevitElemTypeStart = "<RevitElemType>";
        string RevitElemTypeEnd = "</RevitElemType>";

        string RevitElemCommentStart = "<RevitElemComment>";
        string RevitElemCommentEnd = "</RevitElemComment>";

        string RevitElemMarkStart = "<RevitElemMark>";
        string RevitElemMarkEnd = "</RevitElemMark>";

        string RevitElemVolumeStart = "<RevitElemVolume>";
        string RevitElemVolumeEnd = "</RevitElemVolume>";

        string RevitElemSurfaceAreaStart = "<RevitElemSurfaceArea>";
        string RevitElemSurfaceAreaEnd = "</RevitElemSurfaceArea>";

        string FaceAreaStart = "<FaceArea>";
        string FaceAreaEnd = "</FaceArea>";

        string FacePerimeterStart = "<FacePerimeter>";
        string FacePerimeterEnd = "</FacePerimeter>";

        #endregion


        private Document exportedDocument = null;
        public uint CurrentPolymeshIndex { get; set; }
        ElementId CurrentElementId
        {
            get
            {
                return (elementStack.Count > 0)
                  ? elementStack.Peek()
                  : ElementId.InvalidElementId;
            }
        }
        Element CurrentElement
        {
            get
            {
                return exportedDocument.GetElement(
                  CurrentElementId);
            }
        }

        Transform CurrentTransform
        {
            get
            {
                return transformationStack.Peek();
            }
        }

        private bool isCancelled = false;

        Stack<ElementId> elementStack = new Stack<ElementId>();

        private Stack<Transform> transformationStack = new Stack<Transform>();

        ElementId currentMaterialId = ElementId.InvalidElementId;

        StreamWriter streamWriter = null;

        Dictionary<uint, ElementId> polymeshToMaterialId = new Dictionary<uint, ElementId>();


        public MyExportContext(
          Document document,
          string filepath)
        {
            doc = document;
            this.exportedDocument = document;
            transformationStack.Push(Transform.Identity);

            streamWriter = new StreamWriter(filepath);
        }

        public bool Start()
        {
            CurrentPolymeshIndex = 0;
            polymeshToMaterialId.Clear();
            EXTRAINFO.Clear();

            WriteXmlColladaBegin();
            WriteXmlAsset();

            WriteXmlLibraryGeometriesBegin();

            return true;
        }

        public void Finish()
        {
            WriteXmlLibraryGeometriesEnd();

            WriteXmlLibraryMaterials();
            WriteXmlLibraryEffects();
            WriteXmlLibraryVisualScenes();
            WriteXmlColladaEnd();

            streamWriter.Close();
        }

        private void WriteXmlColladaBegin()
        {
            streamWriter.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n");
            streamWriter.Write("<COLLADA xmlns=\"http://www.collada.org/2005/11/COLLADASchema\" version=\"1.4.1\">\n");
        }

        private void WriteXmlColladaEnd()
        {
            streamWriter.Write("</COLLADA>\n");
        }

        private void WriteXmlAsset()
        {
            streamWriter.Write("\t<asset>\n");
            streamWriter.Write("\t\t<contributor>\n");
            streamWriter.Write("\t\t\t<authoring_tool>Cofico Revit COLLADA exporter</authoring_tool>\n");
            streamWriter.Write("\t\t</contributor>\n");
            streamWriter.Write("\t\t<created>" + DateTime.Now.ToString() + "</created>\n");

            //Units
            streamWriter.Write("\t\t<unit name=\"meter\" meter=\"1.00\"/>\n");
            streamWriter.Write("\t\t<up_axis>Z_UP</up_axis>\n");
            streamWriter.Write("\t</asset>\n");
        }

        private void WriteXmlLibraryGeometriesBegin()
        {
            streamWriter.Write("\t<library_geometries>\n");
        }

        private void WriteXmlLibraryGeometriesEnd()
        {
            streamWriter.Write("\t</library_geometries>\n");
        }

        

        private void WriteXmlGeometryBegin()
        {
            streamWriter.Write("\t\t<geometry id=\"geom-" + CurrentPolymeshIndex + "\" name=\"" + GetCurrentElementName() + "\">\n");
            streamWriter.Write("\t\t\t<mesh>\n");
        }

        private string GetCurrentElementName()
        {
            Element element = CurrentElement;
            if (element != null)
                return element.Name;

            return ""; //default name
        }

        private void WriteXmlGeometryEnd()
        {
            streamWriter.Write("\t\t\t</mesh>\n");
            streamWriter.Write("\t\t</geometry>\n");
        }
        /*
        private void WriteXmlGeometrySourcePositions(PolymeshTopology polymesh)
        {
            streamWriter.Write("\t\t\t\t<source id=\"geom-" + CurrentPolymeshIndex + "-positions" + "\">\n");
            streamWriter.Write("\t\t\t\t\t<float_array id=\"geom-" + CurrentPolymeshIndex + "-positions" + "-array" + "\" count=\"" + (polymesh.NumberOfPoints * 3) + "\">\n");

            XYZ point;
            Transform currentTransform = transformationStack.Peek();

            for (int iPoint = 0; iPoint < polymesh.NumberOfPoints; ++iPoint)
            {
                point = polymesh.GetPoint(iPoint);
                point = currentTransform.OfPoint(point);
                streamWriter.Write("\t\t\t\t\t\t{0:0.0000} {1:0.0000} {2:0.0000}\n", point.X, point.Y, point.Z);
            }

            streamWriter.Write("\t\t\t\t\t</float_array>\n");
            streamWriter.Write("\t\t\t\t\t<technique_common>\n");
            streamWriter.Write("\t\t\t\t\t\t<accessor source=\"#geom-" + CurrentPolymeshIndex + "-positions" + "-array\"" + " count=\"" + polymesh.NumberOfPoints + "\" stride=\"3\">\n");
            streamWriter.Write("\t\t\t\t\t\t\t<param name=\"X\" type=\"float\"/>\n");
            streamWriter.Write("\t\t\t\t\t\t\t<param name=\"Y\" type=\"float\"/>\n");
            streamWriter.Write("\t\t\t\t\t\t\t<param name=\"Z\" type=\"float\"/>\n");
            streamWriter.Write("\t\t\t\t\t\t</accessor>\n");
            streamWriter.Write("\t\t\t\t\t</technique_common>\n");
            streamWriter.Write("\t\t\t\t</source>\n");
        }
        */
        //

        public int totalNumberOfPoint = 0;
        public int totalNumberOfNormal = 0;
        public int totalNormalCount = 0;

        public StringBuilder sbPoints = new StringBuilder();
        public StringBuilder sbNormals = new StringBuilder();
        

        private void contentBuilder_postion(PolymeshTopology polymesh)
        {            
            XYZ point;
            Transform currentTransform = transformationStack.Peek();
            for (int iPoint = 0; iPoint < polymesh.NumberOfPoints; ++iPoint)
            {
                point = polymesh.GetPoint(iPoint);
                point = currentTransform.OfPoint(point);
                sbPoints.AppendLine(string.Format("\t\t\t\t\t\t{0:0.0000} {1:0.0000} {2:0.0000}\n", point.X, point.Y, point.Z));
            }
        }

        private void WriteXmlGeometrySourcePositions(StringBuilder sbPoints)
        {
            streamWriter.Write("\t\t\t\t<source id=\"geom-" + elementID + "-positions" + "\">\n");
            streamWriter.Write("\t\t\t\t\t<float_array id=\"geom-" + elementID + "-positions" + "-array" + "\" count=\"" + totalNumberOfPoint + "\">\n");
            streamWriter.Write(sbPoints.ToString());
            streamWriter.Write("\t\t\t\t\t</float_array>\n");
            streamWriter.Write("\t\t\t\t\t<technique_common>\n");
            streamWriter.Write("\t\t\t\t\t\t<accessor source=\"#geom-" + elementID + "-positions" + "-array\"" + " count=\"" + totalNumberOfPoint/3 + "\" stride=\"3\">\n");
            streamWriter.Write("\t\t\t\t\t\t\t<param name=\"X\" type=\"float\"/>\n");
            streamWriter.Write("\t\t\t\t\t\t\t<param name=\"Y\" type=\"float\"/>\n");
            streamWriter.Write("\t\t\t\t\t\t\t<param name=\"Z\" type=\"float\"/>\n");
            streamWriter.Write("\t\t\t\t\t\t</accessor>\n");
            streamWriter.Write("\t\t\t\t\t</technique_common>\n");
            streamWriter.Write("\t\t\t\t</source>\n");
        }

        /*private void WriteXmlGeometrySourceNormals(PolymeshTopology polymesh)
        {
            int nNormals = 0;
            switch (polymesh.DistributionOfNormals)
            {
                case DistributionOfNormals.AtEachPoint:
                    nNormals = polymesh.NumberOfPoints;
                    break;
                case DistributionOfNormals.OnePerFace:
                    nNormals = 1;
                    break;
                case DistributionOfNormals.OnEachFacet:
                    //TODO : DistributionOfNormals.OnEachFacet
                    nNormals = 1;
                    break;
            }
            streamWriter.Write("\t\t\t\t<source id=\"geom-" + CurrentPolymeshIndex + "-normals" + "\">\n");
            streamWriter.Write("\t\t\t\t\t<float_array id=\"geom-" + CurrentPolymeshIndex + "-normals" + "-array" + "\" count=\"" + (nNormals * 3) + "\">\n");
            XYZ point;
            Transform currentTransform = transformationStack.Peek();
            for (int iNormal = 0; iNormal < nNormals; ++iNormal)
            {
                point = polymesh.GetNormal(iNormal);
                point = currentTransform.OfVector(point);
                streamWriter.Write("\t\t\t\t\t\t{0:0.0000} {1:0.0000} {2:0.0000}\n", point.X, point.Y, point.Z);
            }
            streamWriter.Write("\t\t\t\t\t</float_array>\n");
            streamWriter.Write("\t\t\t\t\t<technique_common>\n");
            streamWriter.Write("\t\t\t\t\t\t<accessor source=\"#geom-" + CurrentPolymeshIndex + "-normals" + "-array\"" + " count=\"" + nNormals + "\" stride=\"3\">\n");
            streamWriter.Write("\t\t\t\t\t\t\t<param name=\"X\" type=\"float\"/>\n");
            streamWriter.Write("\t\t\t\t\t\t\t<param name=\"Y\" type=\"float\"/>\n");
            streamWriter.Write("\t\t\t\t\t\t\t<param name=\"Z\" type=\"float\"/>\n");
            streamWriter.Write("\t\t\t\t\t\t</accessor>\n");
            streamWriter.Write("\t\t\t\t\t</technique_common>\n");
            streamWriter.Write("\t\t\t\t</source>\n");
        }*/

        private void contentBuilder_normal(PolymeshTopology polymesh)
        {
            sbNormals.AppendLine();
            int nNormals = 0;

            switch (polymesh.DistributionOfNormals)
            {
                case DistributionOfNormals.AtEachPoint:
                    nNormals = polymesh.NumberOfPoints;
                    break;
                case DistributionOfNormals.OnePerFace:
                    nNormals = 1;
                    break;
                case DistributionOfNormals.OnEachFacet:
                    //TODO : DistributionOfNormals.OnEachFacet
                    nNormals = 1;
                    break;
            }
            totalNumberOfNormal += nNormals * 3;
            totalNormalCount += nNormals;
            XYZ point;
            Transform currentTransform = transformationStack.Peek();
            for (int iNormal = 0; iNormal < nNormals; ++iNormal){
                point = polymesh.GetNormal(iNormal);
                point = currentTransform.OfVector(point);
                sbNormals.AppendLine(string.Format("\t\t\t\t\t\t{0:0.0000} {1:0.0000} {2:0.0000}\n", point.X, point.Y, point.Z));
            }
            
        }
        private void WriteXmlGeometrySourceNormals(StringBuilder sbNormals)
        {
            streamWriter.Write("\t\t\t\t<source id=\"geom-" + elementID + "-normals" + "\">\n");
            streamWriter.Write("\t\t\t\t\t<float_array id=\"geom-" + elementID + "-normals" + "-array" + "\" count=\"" + totalNumberOfNormal + "\">\n");
            streamWriter.Write(sbNormals.ToString());
            streamWriter.Write("\t\t\t\t\t</float_array>\n");
            streamWriter.Write("\t\t\t\t\t<technique_common>\n");
            streamWriter.Write("\t\t\t\t\t\t<accessor source=\"#geom-" + elementID + "-normals" + "-array\"" + " count=\"" + totalNormalCount + "\" stride=\"3\">\n");
            streamWriter.Write("\t\t\t\t\t\t\t<param name=\"X\" type=\"float\"/>\n");
            streamWriter.Write("\t\t\t\t\t\t\t<param name=\"Y\" type=\"float\"/>\n");
            streamWriter.Write("\t\t\t\t\t\t\t<param name=\"Z\" type=\"float\"/>\n");
            streamWriter.Write("\t\t\t\t\t\t</accessor>\n");
            streamWriter.Write("\t\t\t\t\t</technique_common>\n");
            streamWriter.Write("\t\t\t\t</source>\n");
        }
        /*
        private void WriteXmlGeometrySourceMap(PolymeshTopology polymesh)
        {
            streamWriter.Write("\t\t\t\t<source id=\"geom-" + CurrentPolymeshIndex + "-map" + "\">\n");
            streamWriter.Write("\t\t\t\t\t<float_array id=\"geom-" + CurrentPolymeshIndex + "-map" + "-array" + "\" count=\"" + (polymesh.NumberOfUVs * 2) + "\">\n");
            UV uv;
            for (int iUv = 0; iUv < polymesh.NumberOfUVs; ++iUv)
            {
                uv = polymesh.GetUV(iUv);
                streamWriter.Write("\t\t\t\t\t\t{0:0.0000} {1:0.0000}\n", uv.U, uv.V);
            }
            streamWriter.Write("\t\t\t\t\t</float_array>\n");
            streamWriter.Write("\t\t\t\t\t<technique_common>\n");
            streamWriter.Write("\t\t\t\t\t\t<accessor source=\"#geom-" + CurrentPolymeshIndex + "-map" + "-array\"" + " count=\"" + polymesh.NumberOfPoints + "\" stride=\"2\">\n");
            streamWriter.Write("\t\t\t\t\t\t\t<param name=\"S\" type=\"float\"/>\n");
            streamWriter.Write("\t\t\t\t\t\t\t<param name=\"T\" type=\"float\"/>\n");
            streamWriter.Write("\t\t\t\t\t\t</accessor>\n");
            streamWriter.Write("\t\t\t\t\t</technique_common>\n");
            streamWriter.Write("\t\t\t\t</source>\n");
        }*/
        public StringBuilder sbSourceMap, sbTriWOutmap, sbTriWithmap;
        public int totalNumberofUV = 0;
        public int totalNumberofPolyFacet = 0;

        private void contentBuilder_sourceMap(PolymeshTopology polymesh)
        {
            totalNumberofUV += polymesh.NumberOfUVs * 2;
            UV uv;
            for (int iUv = 0; iUv < polymesh.NumberOfUVs; ++iUv)
            {
                uv = polymesh.GetUV(iUv);
                sbSourceMap.AppendLine(string.Format("\t\t\t\t\t\t{0:0.0000} {1:0.0000}\n", uv.U, uv.V));
            }
        }

         private void WriteXmlGeometrySourceMap(StringBuilder sbSourceMap)
        {
            streamWriter.Write("\t\t\t\t<source id=\"geom-" + elementID + "-map" + "\">\n");
            streamWriter.Write("\t\t\t\t\t<float_array id=\"geom-" + elementID + "-map" + "-array" + "\" count=\"" + totalNumberofUV + "\">\n");
            streamWriter.Write(sbSourceMap.ToString());
            streamWriter.Write("\t\t\t\t\t</float_array>\n");
            streamWriter.Write("\t\t\t\t\t<technique_common>\n");
            streamWriter.Write("\t\t\t\t\t\t<accessor source=\"#geom-" + elementID + "-map" + "-array\"" + " count=\"" + totalNumberOfPoint + "\" stride=\"2\">\n");
            streamWriter.Write("\t\t\t\t\t\t\t<param name=\"S\" type=\"float\"/>\n");
            streamWriter.Write("\t\t\t\t\t\t\t<param name=\"T\" type=\"float\"/>\n");
            streamWriter.Write("\t\t\t\t\t\t</accessor>\n");
            streamWriter.Write("\t\t\t\t\t</technique_common>\n");
            streamWriter.Write("\t\t\t\t</source>\n");
        }
        
        private void contentBuilder_triangleWithOutMap(PolymeshTopology polymesh)
        {
            totalNumberofPolyFacet += polymesh.NumberOfFacets;
            PolymeshFacet facet;
            switch (polymesh.DistributionOfNormals)
            {
                case DistributionOfNormals.AtEachPoint:
                    for (int i = 0; i < polymesh.NumberOfFacets; ++i)
                    {
                        facet = polymesh.GetFacet(i);
                        sbTriWOutmap.AppendLine("\t\t\t\t\t\t" + facet.V1 + " " + facet.V1 + " " +
                                    facet.V2 + " " + facet.V2 + " " +
                                    facet.V3 + " " + facet.V3 + " " +
                                    "\n");
                    }
                    break;
                case DistributionOfNormals.OnEachFacet:
                //TODO : DistributionOfNormals.OnEachFacet
                case DistributionOfNormals.OnePerFace:
                    for (int i = 0; i < polymesh.NumberOfFacets; ++i)
                    {
                        facet = polymesh.GetFacet(i);
                        sbTriWOutmap.AppendLine("\t\t\t\t\t\t" + facet.V1 + " 0 " +
                                    facet.V2 + " 0 " +
                                    facet.V3 + " 0 " +
                                    "\n");
                    }
                    break;
            }
        }

        private void WriteXmlGeometryTrianglesWithoutMap(StringBuilder sbTriWOutmap)
        {
            
            streamWriter.Write("\t\t\t\t<triangles count=\"" + totalNumberofPolyFacet + "\"");

            if (IsMaterialValid(currentMaterialId))
                sbTriWOutmap.AppendLine(" material=\"material-" + currentMaterialId.ToString() + "\"");
            streamWriter.Write(">\n");
            streamWriter.Write("\t\t\t\t\t<input offset=\"0\" semantic=\"VERTEX\" source=\"#geom-" + elementID + "-vertices" + "\"/>\n");
            streamWriter.Write("\t\t\t\t\t<input offset=\"1\" semantic=\"NORMAL\" source=\"#geom-" + elementID + "-normals" + "\"/>\n");
            streamWriter.Write("\t\t\t\t\t<p>\n");

            streamWriter.Write(sbTriWOutmap.ToString());

            streamWriter.Write("\t\t\t\t\t</p>\n");
            streamWriter.Write("\t\t\t\t</triangles>\n");
        }

        /*
        private void WriteXmlGeometryTrianglesWithoutMap(PolymeshTopology polymesh)
        {
            streamWriter.Write("\t\t\t\t<triangles count=\"" + polymesh.NumberOfFacets + "\"");
            if (IsMaterialValid(currentMaterialId))
                streamWriter.Write(" material=\"material-" + currentMaterialId.ToString() + "\"");
            streamWriter.Write(">\n");
            streamWriter.Write("\t\t\t\t\t<input offset=\"0\" semantic=\"VERTEX\" source=\"#geom-" + CurrentPolymeshIndex + "-vertices" + "\"/>\n");
            streamWriter.Write("\t\t\t\t\t<input offset=\"1\" semantic=\"NORMAL\" source=\"#geom-" + CurrentPolymeshIndex + "-normals" + "\"/>\n");
            streamWriter.Write("\t\t\t\t\t<p>\n");
            PolymeshFacet facet;
            switch (polymesh.DistributionOfNormals)
            {
                case DistributionOfNormals.AtEachPoint:
                    for (int i = 0; i < polymesh.NumberOfFacets; ++i)
                    {
                        facet = polymesh.GetFacet(i);
                        streamWriter.Write("\t\t\t\t\t\t" + facet.V1 + " " + facet.V1 + " " +
                                    facet.V2 + " " + facet.V2 + " " +
                                    facet.V3 + " " + facet.V3 + " " +
                                    "\n");
                    }
                    break;
                case DistributionOfNormals.OnEachFacet:
                //TODO : DistributionOfNormals.OnEachFacet
                case DistributionOfNormals.OnePerFace:
                    for (int i = 0; i < polymesh.NumberOfFacets; ++i)
                    {
                        facet = polymesh.GetFacet(i);
                        streamWriter.Write("\t\t\t\t\t\t" + facet.V1 + " 0 " +
                                    facet.V2 + " 0 " +
                                    facet.V3 + " 0 " +
                                    "\n");
                    }
                    break;
            }

            streamWriter.Write("\t\t\t\t\t</p>\n");
            streamWriter.Write("\t\t\t\t</triangles>\n");
        }
        */
        private void contentBuilder_triangleWithMap(PolymeshTopology polymesh)
        {
            PolymeshFacet facet;
            switch (polymesh.DistributionOfNormals)
            {
                case DistributionOfNormals.AtEachPoint:
                    for (int i = 0; i < polymesh.NumberOfFacets; ++i)
                    {
                        facet = polymesh.GetFacet(i);
                        sbTriWithmap.AppendLine("\t\t\t\t\t\t" + facet.V1 + " " + facet.V1 + " " + facet.V1 + " " +
                                    facet.V2 + " " + facet.V2 + " " + facet.V2 + " " +
                                    facet.V3 + " " + facet.V3 + " " + facet.V3 + " " +
                                    "\n");
                    }
                    break;

                case DistributionOfNormals.OnEachFacet:
                //TODO : DistributionOfNormals.OnEachFacet
                case DistributionOfNormals.OnePerFace:
                    for (int i = 0; i < polymesh.NumberOfFacets; ++i)
                    {
                        facet = polymesh.GetFacet(i);
                        sbTriWithmap.AppendLine("\t\t\t\t\t\t" + facet.V1 + " 0 " + facet.V1 + " " +
                                    facet.V2 + " 0 " + facet.V2 + " " +
                                    facet.V3 + " 0 " + facet.V3 + " " +
                                    "\n");
                    }
                    break;
            }

        }

        private void WriteXmlGeometryTrianglesWithMap(StringBuilder sbTriWithmap)
        {
            streamWriter.Write("\t\t\t\t<triangles count=\"" + totalNumberofPolyFacet + "\"");
            if (IsMaterialValid(currentMaterialId))
                streamWriter.Write(" material=\"material-" + currentMaterialId.ToString() + "\"");
            streamWriter.Write(">\n");
            streamWriter.Write("\t\t\t\t\t<input offset=\"0\" semantic=\"VERTEX\" source=\"#geom-" + elementID + "-vertices" + "\"/>\n");
            streamWriter.Write("\t\t\t\t\t<input offset=\"1\" semantic=\"NORMAL\" source=\"#geom-" + elementID + "-normals" + "\"/>\n");
            streamWriter.Write("\t\t\t\t\t<input offset=\"2\" semantic=\"TEXCOORD\" source=\"#geom-" + elementID + "-map" + "\" set=\"0\"/>\n");
            streamWriter.Write("\t\t\t\t\t<p>\n");

            streamWriter.Write(sbTriWithmap.ToString());

            streamWriter.Write("\t\t\t\t\t</p>\n");
            streamWriter.Write("\t\t\t\t</triangles>\n");
        }


        /*
        private void WriteXmlGeometryTrianglesWithMap(PolymeshTopology polymesh)
        {
            streamWriter.Write("\t\t\t\t<triangles count=\"" + polymesh.NumberOfFacets + "\"");
            if (IsMaterialValid(currentMaterialId))
                streamWriter.Write(" material=\"material-" + currentMaterialId.ToString() + "\"");
            streamWriter.Write(">\n");
            streamWriter.Write("\t\t\t\t\t<input offset=\"0\" semantic=\"VERTEX\" source=\"#geom-" + CurrentPolymeshIndex + "-vertices" + "\"/>\n");
            streamWriter.Write("\t\t\t\t\t<input offset=\"1\" semantic=\"NORMAL\" source=\"#geom-" + CurrentPolymeshIndex + "-normals" + "\"/>\n");
            streamWriter.Write("\t\t\t\t\t<input offset=\"2\" semantic=\"TEXCOORD\" source=\"#geom-" + CurrentPolymeshIndex + "-map" + "\" set=\"0\"/>\n");
            streamWriter.Write("\t\t\t\t\t<p>\n");
            PolymeshFacet facet;

            switch (polymesh.DistributionOfNormals)
            {
                case DistributionOfNormals.AtEachPoint:
                    for (int i = 0; i < polymesh.NumberOfFacets; ++i)
                    {
                        facet = polymesh.GetFacet(i);
                        streamWriter.Write("\t\t\t\t\t\t" + facet.V1 + " " + facet.V1 + " " + facet.V1 + " " +
                                    facet.V2 + " " + facet.V2 + " " + facet.V2 + " " +
                                    facet.V3 + " " + facet.V3 + " " + facet.V3 + " " +
                                    "\n");
                    }
                    break;

                case DistributionOfNormals.OnEachFacet:
                //TODO : DistributionOfNormals.OnEachFacet
                case DistributionOfNormals.OnePerFace:
                    for (int i = 0; i < polymesh.NumberOfFacets; ++i)
                    {
                        facet = polymesh.GetFacet(i);
                        streamWriter.Write("\t\t\t\t\t\t" + facet.V1 + " 0 " + facet.V1 + " " +
                                    facet.V2 + " 0 " + facet.V2 + " " +
                                    facet.V3 + " 0 " + facet.V3 + " " +
                                    "\n");
                    }
                    break;
            }

            streamWriter.Write("\t\t\t\t\t</p>\n");
            streamWriter.Write("\t\t\t\t</triangles>\n");
        }
        */

        private void WriteXmlGeometryVertices()
        {
            streamWriter.Write("\t\t\t\t<vertices id=\"geom-" + elementID + "-vertices" + "\">\n");
            streamWriter.Write("\t\t\t\t\t<input semantic=\"POSITION\" source=\"#geom-" + elementID + "-positions" + "\"/>\n");
            streamWriter.Write("\t\t\t\t</vertices>\n");
        }      
        

        private void WriteXmlLibraryMaterials()
        {
            streamWriter.Write("\t<library_materials>\n");

            foreach (var materialId in polymeshToMaterialId.Values.Distinct())
            {
                if (IsMaterialValid(materialId) == false)
                    continue;

                streamWriter.Write("\t\t<material id=\"material-" + materialId.ToString() + "\" name=\"" + GetMaterialName(materialId) + "\">\n");
                streamWriter.Write("\t\t\t<instance_effect url=\"#effect-" + materialId.ToString() + "\" />\n");
                streamWriter.Write("\t\t</material>\n");
            }

            streamWriter.Write("\t</library_materials>\n");
        }

        private string GetMaterialName(ElementId materialId)
        {
            Material material = exportedDocument.GetElement(materialId) as Material;
            if (material != null)
                return material.Name;

            return ""; //default material name
        }

        private bool IsMaterialValid(ElementId materialId)
        {
            Material material = exportedDocument.GetElement(materialId) as Material;
            if (material != null)
                return true;

            return false;
        }

        private void WriteXmlLibraryEffects()
        {
            streamWriter.Write("\t<library_effects>\n");

            foreach (var materialId in polymeshToMaterialId.Values.Distinct())
            {
                if (IsMaterialValid(materialId) == false)
                    continue;

                Material material = exportedDocument.GetElement(materialId) as Material;

                streamWriter.Write("\t\t<effect id=\"effect-" + materialId.ToString() + "\" name=\"" + GetMaterialName(materialId) + "\">\n");
                streamWriter.Write("\t\t\t<profile_COMMON>\n");

                streamWriter.Write("\t\t\t\t<technique sid=\"common\">\n");
                streamWriter.Write("\t\t\t\t\t<phong>\n");
                streamWriter.Write("\t\t\t\t\t\t<ambient>\n");
                streamWriter.Write("\t\t\t\t\t\t\t<color>0.1 0.1 0.1 1.000000</color>\n");
                streamWriter.Write("\t\t\t\t\t\t</ambient>\n");


                //diffuse
                streamWriter.Write("\t\t\t\t\t\t<diffuse>\n");
                streamWriter.Write("\t\t\t\t\t\t\t<color>" + material.Color.Red + " " + material.Color.Green + " " + material.Color.Blue + " 1.0</color>\n");
                streamWriter.Write("\t\t\t\t\t\t</diffuse>\n");


                streamWriter.Write("\t\t\t\t\t\t<specular>\n");
                streamWriter.Write("\t\t\t\t\t\t\t<color>1.000000 1.000000 1.000000 1.000000</color>\n");
                streamWriter.Write("\t\t\t\t\t\t</specular>\n");

                streamWriter.Write("\t\t\t\t\t\t<shininess>\n");
                streamWriter.Write("\t\t\t\t\t\t\t<float>" + material.Shininess + "</float>\n");
                streamWriter.Write("\t\t\t\t\t\t</shininess>\n");

                streamWriter.Write("\t\t\t\t\t\t<reflective>\n");
                streamWriter.Write("\t\t\t\t\t\t\t<color>0 0 0 1.000000</color>\n");
                streamWriter.Write("\t\t\t\t\t\t</reflective>\n");
                streamWriter.Write("\t\t\t\t\t\treflectivity>\n");
                streamWriter.Write("\t\t\t\t\t\t\t<float>1.000000</float>\n");
                streamWriter.Write("\t\t\t\t\t\t</reflectivity>\n");

                streamWriter.Write("\t\t\t\t\t\t<transparent opaque=\"RGB_ZERO\">\n");
                streamWriter.Write("\t\t\t\t\t\t\t<color>1.000000 1.000000 1.000000 1.000000</color>\n");
                streamWriter.Write("\t\t\t\t\t\t</transparent>\n");

                streamWriter.Write("\t\t\t\t\t\t<transparency>\n");
                streamWriter.Write("\t\t\t\t\t\t\t<float>" + material.Transparency + "</float>\n");
                streamWriter.Write("\t\t\t\t\t\t</transparency>\n");

                streamWriter.Write("\t\t\t\t\t</phong>\n");
                streamWriter.Write("\t\t\t\t</technique>\n");


                streamWriter.Write("\t\t\t</profile_COMMON>\n");
                streamWriter.Write("\t\t</effect>\n");
            }

            streamWriter.Write("\t</library_effects>\n");
        }

        public void WriteXmlLibraryVisualScenes()
        {
            streamWriter.Write("\t<library_visual_scenes>\n");
            streamWriter.Write("\t\t<visual_scene id=\"Revit_project\">\n");
            
            foreach (var pair in polymeshToMaterialId)
            {
                streamWriter.Write("\t\t\t<node id=\"node-" + pair.Key + "\" name=\"" + EXTRAINFO[pair.Key] + "\">\n");
                streamWriter.Write("\t\t\t\t<instance_geometry url=\"#geom-" + pair.Key + "\">\n");
                if (IsMaterialValid(pair.Value))
                {
                    streamWriter.Write("\t\t\t\t\t<bind_material>\n");
                    streamWriter.Write("\t\t\t\t\t\t<technique_common>\n");
                    streamWriter.Write("\t\t\t\t\t\t\t<instance_material target=\"#material-" + pair.Value + "\" symbol=\"material-" + pair.Value + "\" >\n");
                    streamWriter.Write("\t\t\t\t\t\t\t</instance_material>\n");
                    streamWriter.Write("\t\t\t\t\t\t</technique_common>\n");
                    streamWriter.Write("\t\t\t\t\t</bind_material>\n");
                }
                streamWriter.Write("\t\t\t\t</instance_geometry>\n");
                streamWriter.Write("\t\t\t</node>\n");
            }
            /* backup
            foreach (var pair in polymeshToMaterialId)
            {
                streamWriter.Write("<node id=\"node-" + pair.Key + "\" name=\"" + "elementName" + "\">\n");
                streamWriter.Write("<instance_geometry url=\"#geom-" + pair.Key + "\">\n");
                if (IsMaterialValid(pair.Value))
                {
                    streamWriter.Write("<bind_material>\n");
                    streamWriter.Write("<technique_common>\n");
                    streamWriter.Write("<instance_material target=\"#material-" + pair.Value + "\" symbol=\"material-" + pair.Value + "\" >\n");
                    streamWriter.Write("</instance_material>\n");
                    streamWriter.Write("</technique_common>\n");
                    streamWriter.Write("</bind_material>\n");
                }
                streamWriter.Write("</instance_geometry>\n");
                streamWriter.Write("</node>\n");
            }*/

            streamWriter.Write("\t\t</visual_scene>\n");
            streamWriter.Write("\t</library_visual_scenes>\n");

            streamWriter.Write("\t<scene>\n");
            streamWriter.Write("\t\t<instance_visual_scene url=\"#Revit_project\"/>\n");
            streamWriter.Write("\t</scene>\n");
        }

        public bool IsCanceled()
        {
            // This method is invoked many times during the export process.
            return isCancelled;
        }

        //public void OnDaylightPortal( DaylightPortalNode node )
        //{
        //}
        public void OnPolymesh(PolymeshTopology polymesh)
        {
            totalNumberOfPoint += polymesh.NumberOfPoints * 3;

            contentBuilder_postion(polymesh);





            /*CurrentPolymeshIndex++;
            WriteXmlGeometryBegin();
            WriteXmlGeometrySourcePositions(polymesh);
            WriteXmlGeometrySourceNormals(polymesh);
            */
            if (polymesh.NumberOfUVs > 0)
                WriteXmlGeometrySourceMap(polymesh);
            WriteXmlGeometryVertices();
            if (polymesh.NumberOfUVs > 0)
                WriteXmlGeometryTrianglesWithMap(polymesh);
            else
                WriteXmlGeometryTrianglesWithoutMap(polymesh);
            /*
            //writeExtraInformation();
            WriteXmlGeometryEnd();
            polymeshToMaterialId.Add(CurrentPolymeshIndex, currentMaterialId);
            EXTRAINFO.Add(CurrentPolymeshIndex, elemId);*/
        }

        public void OnMaterial(MaterialNode node)
        {
            // OnMaterial method can be invoked for every single out-coming mesh
            // even when the material has not actually changed. Thus it is usually
            // beneficial to store the current material and only get its attributes
            // when the material actually changes.
            currentMaterialId = node.MaterialId;
            //MessageBox.Show(node.NodeName);
            //MessageBox.Show(exportedDocument.GetElement(node.MaterialId).Name);
        }
        public void OnRPC(RPCNode node)
        {
        }
        public RenderNodeAction OnViewBegin(ViewNode node)
        {
            return RenderNodeAction.Proceed;
        }
        public void OnViewEnd(ElementId elementId)
        {
            // Note: This method is invoked even for a view that was skipped.
        }
        public RenderNodeAction OnElementBegin(ElementId elementId)
        {
            elementStack.Push(elementId);
            //MessageBox.Show(elementId.ToString());
            elemId = elementId;
            guid = doc.GetElement(elementId).UniqueId.ToString();
            cat = doc.GetElement(elementId).Category.Name;
            FamilyInstance fInstance = doc.GetElement(elementId) as FamilyInstance;
            FamilySymbol FType = fInstance.Symbol;
            fam = FType.Family.Name;
            typ = doc.GetElement(doc.GetElement(elementId).GetTypeId()).Name;
            comm = doc.GetElement(elementId).LookupParameter("Comment").ToString();
            mark = doc.GetElement(elementId).LookupParameter("Mark").ToString();
            vol =  doc.GetElement(elementId).LookupParameter("Volume").ToString();
            /*SA;
            FA;
            FP;*/

            string s = string.Format("{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}", elemId, guid, cat, fam, typ, comm, mark, vol);

            elementID = s;
            return RenderNodeAction.Proceed;
        }
        


        public void OnElementEnd(ElementId elementId)            
        {
            CurrentPolymeshIndex++;

            WriteXmlGeometryBegin();
            WriteXmlGeometrySourcePositions(sbPoints);
            WriteXmlGeometrySourceNormals(sbNormals);





            
            
            //writeExtraInformation();

            WriteXmlGeometryEnd();

            polymeshToMaterialId.Add(CurrentPolymeshIndex, currentMaterialId);
            EXTRAINFO.Add(CurrentPolymeshIndex, elemId);

            elementID = "0";

            totalNumberOfPoint = 0;
            totalNumberOfNormal = 0;
            totalNormalCount = 0;

            totalNumberofUV = 0;
            totalNumberofPolyFacet = 0;

            sbPoints.Clear();
            sbNormals.Clear();
            sbSourceMap.Clear();
            

            // Note: this method is invoked even for elements that were skipped.
            elementStack.Pop();
        }

        private void TotalWriteGeometrySourceMap(PolymeshTopology polymesh)
        {
            WriteXmlGeometrySourceMap(sbSourceMap);

            if (polymesh.NumberOfUVs > 0)
                WriteXmlGeometrySourceMap(sbSourceMap);

            WriteXmlGeometryVertices();

            if (polymesh.NumberOfUVs > 0)
                WriteXmlGeometryTrianglesWithMap(sbTriWithmap);
            else
                WriteXmlGeometryTrianglesWithoutMap(sbTriWOutmap);
        }

        public RenderNodeAction OnFaceBegin(FaceNode node)
        {
            // This method is invoked only if the custom exporter was set to include faces.

            return RenderNodeAction.Proceed;
        }

        public void OnFaceEnd(FaceNode node)
        {
            // This method is invoked only if the custom exporter was set to include faces.
            // Note: This method is invoked even for faces that were skipped.
        }

        public RenderNodeAction OnInstanceBegin(InstanceNode node)
        {
            // This method marks the start of processing a family instance
            transformationStack.Push(transformationStack.Peek().Multiply(node.GetTransform()));

            // We can either skip this instance or proceed with rendering it.
            return RenderNodeAction.Proceed;
        }

        public void OnInstanceEnd(InstanceNode node)
        {
            // Note: This method is invoked even for instances that were skipped.
            transformationStack.Pop();
        }

        public RenderNodeAction OnLinkBegin(LinkNode node)
        {
            transformationStack.Push(transformationStack.Peek().Multiply(node.GetTransform()));
            return RenderNodeAction.Proceed;
        }

        public void OnLinkEnd(LinkNode node)
        {
            // Note: This method is invoked even for instances that were skipped.
            transformationStack.Pop();
        }

        public void OnLight(LightNode node)
        {
        }
    }
}