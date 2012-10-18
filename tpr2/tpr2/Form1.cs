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

            var list = new PointPairList();
            var class0List = new PointPairList();
            var class1List = new PointPairList();

            for (double x = 0; x <= 8; x += 0.1)
            {
                double f = core.F(x);
                if(f < 8 && f > 0)
                    list.Add(x, f);
            }


            foreach (Entity e in core.TeachingRow)
            {
                if (e.Class == 0)
                    class0List.Add(e.X, e.Y);
                else
                    class1List.Add(e.X, e.Y);
            }

            LineItem myCurve = pane.AddCurve("Решающее правило", list, Color.Blue, SymbolType.None);
            LineItem class0 = pane.AddCurve("Класс 0", class0List, Color.Green, SymbolType.Diamond);
            LineItem class1 = pane.AddCurve("Класс 1", class1List, Color.Red, SymbolType.Triangle);

            class0.Line.IsVisible = false;
            class0.Symbol.Fill.Type = FillType.Solid;
            class1.Line.IsVisible = false;
            class1.Symbol.Fill.Type = FillType.Solid;

            zedGraph.ScrollMaxY = 10;
            zedGraph.ScrollMinY = 0;
            zedGraph.AxisChange();

            zedGraph.Invalidate();
        }
    }
}
