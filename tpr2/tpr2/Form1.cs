using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace tpr2
{
    public partial class Form1 : Form
    {
        public Core core;
        private bool Calculate;
        private Entity CurRule { get; set; }
        private PointPairList ControlPoints; 

        public Form1()
        {
            InitializeComponent();
            ControlPoints = new PointPairList();
            CurRule = new Entity();
            Calculate = false;
            core = new Core();
        }

        private void bCalculate_Click(object sender, EventArgs e)
        {
            if (maskedTextBox1.Text != "")
            {
                if (double.Parse(maskedTextBox1.Text) <= 0 || double.Parse(maskedTextBox1.Text) > 1)
                {
                    textBox1.Text = "Коэффициент должен быть 0 < a <= 1";
                }
                else
                {
                    Cursor = Cursors.WaitCursor;
                    Entity weightVector = core.TeachCalculateLine(double.Parse(maskedTextBox1.Text));
                    textBox1.Text = weightVector.X.ToString() + "     " + weightVector.Y.ToString() + "     " +
                                    weightVector.Class.ToString();
                    CurRule = weightVector;
                    DrawGraph(core.AnalitCalculateLine(), weightVector);
                    Cursor = Cursors.Arrow;
                    Calculate = true;
                }
            }
            else
            {
                textBox1.Text = "Введите коэффициент обучаемости!";
            }
        }

        private void maskedTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter) bCalculate_Click(button1, new EventArgs());
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
                return;
            }
        }

        public void DrawGraph(Entity avector, Entity tvector)
        {
            GraphPane pane = zedGraph.GraphPane;
            pane.CurveList.Clear();
            ControlPoints.Clear();

            var lista = new PointPairList();
            var listt = new PointPairList();
            var class0List = new PointPairList();
            var class1List = new PointPairList();

            core.SetCoefficientes(avector);
            for (double x = 2; x <= 6; x += 0.1)
            {
                double f = core.F(x);
                //if (f < 16 && f > 0)
                    lista.Add(x, f);
            }
            pane.AddCurve("Решающее правило(аналит)", lista, Color.Chocolate, SymbolType.None);

            core.SetCoefficientes(tvector);
            for (double x = 2; x <= 6; x += 0.1)
            {
                double f = core.F(x);
                if (f < 16 && f > 0)
                    listt.Add(x, f);
            }
            pane.AddCurve("Решающее правило(обуч)", listt, Color.Blue, SymbolType.None);

            foreach (Entity e in core.TeachingRow)
            {
                if (e.Class == 0)
                    class0List.Add(e.X, e.Y);
                else
                    class1List.Add(e.X, e.Y);
            }

            LineItem class0 = pane.AddCurve("Класс 0", class0List, Color.Green, SymbolType.Diamond);
            LineItem class1 = pane.AddCurve("Класс 1", class1List, Color.Red, SymbolType.Triangle);

            class0.Line.IsVisible = false;
            class0.Symbol.Fill.Type = FillType.Solid;
            class1.Line.IsVisible = false;
            class1.Symbol.Fill.Type = FillType.Solid;

            zedGraph.AxisChange();

            zedGraph.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!Calculate) return;
            textBox4.Text = CheckClass(double.Parse(textBox2.Text), double.Parse(textBox3.Text));
            DrawPoint();
        }

        public void DrawPoint()
        {
            GraphPane pane = zedGraph.GraphPane;
            if(ControlPoints.Count != 1)           
                pane.CurveList.RemoveAt(pane.CurveList.Count-1);
            LineItem Points = pane.AddCurve("Контрольные значения", ControlPoints, Color.DarkOrchid, SymbolType.Square);
            Points.Line.IsVisible = false;
            Points.Symbol.Fill.Type = FillType.Solid;
            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        private string CheckClass(double p1, double p2)
        {
            ControlPoints.Add(p1, p2);
            return CurRule.X*p1 + CurRule.Y*p2 + CurRule.Class < 0 ? "0" : "1";
        }
    }
}
