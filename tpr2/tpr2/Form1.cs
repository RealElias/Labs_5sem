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

        public Form1()
        {
            core = new Core();
            InitializeComponent();
        }

        private void bCalculate_Click(object sender, EventArgs e)
        {
            if (maskedTextBox1.Text != "")
            {
                Cursor = Cursors.WaitCursor;
                Entity weightVector = core.CalculateLine(double.Parse(maskedTextBox1.Text));
                Cursor = Cursors.Arrow;
                textBox1.Text = weightVector.X.ToString() + "x + " + weightVector.Y.ToString() + "y + " + weightVector.Class.ToString() + " = 0";
                core.SetCoefficientes(weightVector);
                DrawGraph();
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

        public void DrawGraph()
        {
            GraphPane pane = zedGraph.GraphPane;
            pane.CurveList.Clear();

            PointPairList list = new PointPairList();
            PointPairList class0list = new PointPairList();
            PointPairList class1list = new PointPairList();

            double xmin = 0;
            double xmax = 8;

            for (double x = xmin; x <= xmax; x += 0.1)
            {
                list.Add(x, core.f(x));
            }


            foreach (Entity e in core.teachingRow)
            {
                if (e.Class == 0)
                    class0list.Add(e.X, e.Y);
                else
                    class1list.Add(e.X, e.Y);
            }

            LineItem myCurve = pane.AddCurve("Решающее правило", list, Color.Blue, SymbolType.None);
            LineItem class0 = pane.AddCurve("Класс 0", class0list, Color.Green, SymbolType.Diamond);
            LineItem class1 = pane.AddCurve("Класс 1", class1list, Color.Red, SymbolType.Triangle);

            class0.Line.IsVisible = false;
            class0.Symbol.Fill.Type = FillType.Solid;
            class1.Line.IsVisible = false;
            class1.Symbol.Fill.Type = FillType.Solid;

            zedGraph.AxisChange();

            zedGraph.Invalidate();
        }
    }
}
