using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using SWF = System.Windows.Forms;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.Exceptions;

using COFRevitAPILibs;
using System.IO;
using System.Data.OleDb;
using ExcelDataReader;
using System.Globalization;

namespace InformationInput
{
    public partial class FormReadExcel : SWF.Form
    {
        //
        UIApplication uiApp = null;
        Document doc = null;
        Autodesk.Revit.Creation.Application app = null;

        DataSet result = new DataSet();
        int iX = 3;
        int iY = 4;
        int iR = 6;
        int iV = 2;

        public List<DataList> listdatalist = new List<DataList>();
        public List<XYZ> listXYZ = new List<XYZ>();
        public List<Curve> listCurve = new List<Curve>();
        public List<PairWallXYZs> listPairWallsXYZs = new List<PairWallXYZs>();
        public List<string> listTextValue = new List<string>();
        public List<Wall> listwall = new List<Wall>();
        //

        public FormReadExcel()
        {
            InitializeComponent();
        }
        //        
        public FormReadExcel(UIApplication _uiApp,Document _doc,Autodesk.Revit.Creation.Application _app)
        {
            InitializeComponent();

            uiApp = _uiApp;
            doc = _doc;
            app = _app;

            tiX.Text = iX.ToString();
            tiY.Text = iY.ToString();
            tiR.Text = iR.ToString();
            tiV.Text = iV.ToString();

            WallIselectionFilter wallFilter = new COFRevitAPILibs.WallIselectionFilter();
            BeamSelectionFilter beamFilter = new COFRevitAPILibs.BeamSelectionFilter();

            

            FilteredElementCollector filter = new FilteredElementCollector(doc);
            List<Element> listElems = filter.OfCategory(BuiltInCategory.OST_Walls).WhereElementIsNotElementType().ToElements().ToList();
            listwall = new List<Wall>();
            List<Curve> listwallloccurve = new List<Curve>();
            foreach (Element e in listElems)
            {
                Wall w = e as Wall;
                listwall.Add(w);
                LocationCurve locCurve = w.Location as LocationCurve;
                Line line = locCurve.Curve as Line;
                listwallloccurve.Add(locCurve.Curve);
            }
            listCurve.Clear();
            listCurve.AddRange(listwallloccurve);
        }
         //
        private void bData_Click(object sender, EventArgs e)
        {
            result.Clear();
            using (OpenFileDialog OFD = new OpenFileDialog() { Filter = "Excel File|*.xls", ValidateNames = true })
            {
                if (OFD.ShowDialog() == DialogResult.OK)
                {
                    FileStream fs = File.Open(OFD.FileName, FileMode.Open, FileAccess.Read);
                    IExcelDataReader reader = ExcelReaderFactory.CreateBinaryReader(fs);
                    result = ExcelDataReaderExtensions.AsDataSet(reader, new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = false
                        }
                    });
                    cbSheet.Items.Clear();
                    foreach (DataTable dt in result.Tables)
                    {
                        cbSheet.Items.Add(dt.TableName);
                    }
                    cbSheet.SelectedItem = result.Tables[0].TableName.ToString();
                    dataGridView1.DataSource = result;
                    reader.Close();
                    tDataPath.Text = OFD.FileName;
                }
            }
            
        }
        //
        private void bLocalOrig_Click(object sender, EventArgs e)
        {

        }
        //
        private void bGoto1_Click(object sender, EventArgs e)
        {
            FormProcessCADscheduleNonExcel form = new FormProcessCADscheduleNonExcel();
            form.ShowDialog();
        }
        //
        public List<List<double>> CreateListTextPoint (DataTable dtExcel)
        {
            List<List<double>> listTextPoint = new List<List<double>>();
            StringBuilder sbRows = new StringBuilder();
            while (dtExcel.Rows.GetEnumerator().MoveNext())
            {
                sbRows.AppendLine(dtExcel.Rows.GetEnumerator().Current.ToString());
            }
            return listTextPoint;
        }
        //
        private void cbSheet_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectTable();
        }
        //
        private void bWrite_Click(object sender, EventArgs e)
        {
            
            if (dataGridView1.ColumnCount == 0)
            {
                MessageBox.Show("Please pick data");
            }
            else
            {
                ReadDataGridView(dataGridView1);
                createXYZ();
                createListTextValue();
                find3XYZnearest(listwall,listCurve, listXYZ, listTextValue);
            }
            this.Close();
        }
        //
        private void tiX_TextChanged(object sender, EventArgs e)
        {
            iX = int.Parse(tiX.Text);
        }
        //
        private void tiY_TextChanged(object sender, EventArgs e)
        {
            iY = int.Parse(tiY.Text);
        }
        //
        private void tiR_TextChanged(object sender, EventArgs e)
        {
            iR = int.Parse(tiR.Text);
        }
        //
        private void tiV_TextChanged(object sender, EventArgs e)
        {
            iV = int.Parse(tiV.Text);
        }
        //
        //
        //
        #region Function
        //
        private void createXYZ()
        {
            listXYZ.Clear();
            foreach (DataList data in listdatalist)
            {
                XYZ pt = new XYZ(data.listXY[0], data.listXY[1], 0);
                listXYZ.Add(pt);
            }
        }
        //
        private void createListTextValue()
        {
            listTextValue.Clear();
            foreach (DataList data in listdatalist)
            {
                listTextValue.Add(data.textValue);
            }
        }
        //
        private void SelectTable()
        {
            var tablename = cbSheet.SelectedItem.ToString();
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = result;
            dataGridView1.DataMember = tablename;
        }
        //
        public void find3XYZnearest(List<Wall> listwall,List<Curve> listCurve, List<XYZ>listXYZ, List<string> listTextValue)
        {
            double distIni = listCurve[0].Project(listXYZ[0]).Distance;
            
            for(int i = 0; i < listCurve.Count; i++)
            {
                List<double> listDist = new List<double>();
                List<double> listDist2 = new List<double>();
                List<int> listDistId= new List<int>();

                List<XYZ> result = new List<XYZ>();
                List<string> listText = new List<string>();
                for (int j =0; j<listXYZ.Count;j++)
                {
                    listDist.Add(listCurve[i].Project(listXYZ[j]).Distance);
                    listDist2.Add(listCurve[i].Project(listXYZ[j]).Distance);
                }

                listDist.Sort();

                foreach (double d1 in listDist)
                {
                    foreach (double d2 in listDist2)
                    {
                        if (d1 == d2)
                        {
                            listDistId.Add(listDist2.IndexOf(d2));
                            break;
                        }
                    }
                }

                foreach (int id in listDistId)
                {
                    result.Add(listXYZ[id]);
                    listText.Add(listTextValue[id]);
                }

                PairWallXYZs wallXyzTextV = new PairWallXYZs(result, listwall[i], listText);
                listPairWallsXYZs.Add(wallXyzTextV);
            }
        }
        //
        private void ReadDataGridView(DataGridView dataGrid)
        {
            listdatalist.Clear();
            var fmt = new NumberFormatInfo();
            fmt.NegativeSign = "-";

            for (int r = 1; r < dataGrid.Rows.Count; r++)
            {
                if (dataGrid.Rows[r].Cells[iX].Value != null)
                {
                    List<double> listXY = new List<double>();
                    listXY.Add(mmUnit(dataGrid.Rows[r].Cells[iX].Value.ToString()));
                    listXY.Add(mmUnit(dataGrid.Rows[r].Cells[iY].Value.ToString()));
                    double rot = (mmUnit(dataGrid.Rows[r].Cells[iR].Value.ToString()));
                    string value = dataGrid.Rows[r].Cells[iV].Value.ToString();

                    DataList data = new DataList(listXY, rot, value);
                    listdatalist.Add(data);
                }
                else continue;

            }
        }
        //
        private double mmUnit(string s)
        {
            listdatalist.Clear();
            var fmt = new NumberFormatInfo();
            fmt.NegativeSign = "-";

            return double.Parse(s,fmt) / 304.8;
        }
        #endregion
    }
    public class DataList
    {
        public List<double> listXY = new List<double>();
        public double rotate;
        public string textValue;
        public DataList()
        {
            listXY.Clear();
            rotate = 0;
            textValue = "0";
        }

        public DataList(List<double> _listXY, double _rotate, string _textValue)
        {
            listXY.Clear();
            listXY.AddRange(_listXY);
            rotate = _rotate;
            textValue = _textValue;

        }
    }
    public class PairWallXYZs
    {
        List<XYZ> listXYZNearest = new List<XYZ>();
        Wall wall;
        List<string> listtextvalue = new List<string>();

        public List<XYZ> ListXYZNearest { get => listXYZNearest; set => listXYZNearest = value; }
        public Wall Wall { get => wall; set => wall = value; }
        public List<string> listTextvalue { get => listtextvalue; set => listtextvalue = value; }
        public string Combinetextvalue (){
            string res = "";
            if (listtextvalue.Count != 0)
            {
                foreach (string s in listtextvalue)
                {
                    res += string.Format("#{0}",s);
                }
                return res;
            }
            return "Non Combined";
        }

        public PairWallXYZs(List<XYZ> _listXYZNearest,Wall _wall, List<string> _linktextvalue)
        {
            ListXYZNearest.AddRange(_listXYZNearest);
            Wall = _wall;
            listtextvalue = _linktextvalue;
        }
        public PairWallXYZs()
        {

        }
    }
}
