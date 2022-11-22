using System.CodeDom;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace Gravity {
    public partial class Form1 : Form {
        private bool isRunning = false;
        private int steps = 0;
        private List<Planet> planets = new List<Planet>();

        private Point mousePos = new Point(0, 0);
        private int pictureBoxClicks = 0;

        private Point npPos = new Point(0, 0);
        private Point npVel = new Point(0, 0);

        private Bitmap bmp;
        private Graphics graphics;
        private SolidBrush brush = new SolidBrush(Color.Red);
        private Pen pen = new Pen(Color.Red);

        public Form1() {
            InitializeComponent();

            KnownColor[] colors = Enum.GetValues(typeof(KnownColor)).Cast<KnownColor>().ToArray();
            comboBox1.DataSource = colors;

            //default planets
            planets.Add(new Planet(new Vector(600, 300), new Vector(0, 0), 2000, 70, new SolidBrush(Color.Red)));
            planets.Add(new Planet(new Vector(400, 300), new Vector(1.5, -2.5), 300, 20, new SolidBrush(Color.Blue)));

            bmp = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            graphics = Graphics.FromImage(bmp);
            pictureBox1.Image = bmp;

            VisualUpdate();
        }

        //updates pictureBox1, labal1 (steps: )
        private void VisualUpdate() {
            graphics.Clear(Color.Black);
            foreach (Planet p in planets) {
                graphics.FillEllipse(p.solidBrush, (int)Math.Round(p.position.x - p.size / 2), (int)Math.Round(p.position.y - p.size / 2), p.size, p.size);
            }
            label1.Text = "steps: " + steps.ToString();
            pictureBox1.Refresh();
        }

        private void Step() {
            //calculate the new velocity for each planet
            foreach (Planet pa in planets) {
                foreach (Planet pb in planets) {
                    if (pa != pb) {
                        double d = (pa.position - pb.position).Length();
                        pa.velocity += ((pb.position - pa.position) / d) * (pb.mass / (d * d));
                    }
                }
            }
            //move each planet
            foreach (Planet p in planets) p.position += p.velocity;
            //update visual
            steps++;
            VisualUpdate();
        }

        //start
        private void button1_Click(object sender, EventArgs e) {
            isRunning = true;
            while (isRunning) {
                Step();
                Application.DoEvents();
                Thread.Sleep(20);
            }
        }

        //stop
        private void button2_Click(object sender, EventArgs e) => isRunning = false;

        //step
        private void button3_Click(object sender, EventArgs e) => Step();

        //new planet
        private void button4_Click(object sender, EventArgs e) {
            isRunning = false;
            pictureBoxClicks = 0;
            while (pictureBoxClicks == 0) {
                npPos = mousePos;
                label2.Text = npPos.X.ToString() + ";" + npPos.Y.ToString();
                VisualUpdate();
                graphics.FillEllipse(brush, mousePos.X - 5, mousePos.Y - 5, 10, 10);
                pictureBox1.Refresh();
                Application.DoEvents();
                Thread.Sleep(10);
            }
            while (pictureBoxClicks == 1) {
                npVel = dividePoint(subtractPoints(mousePos, npPos),10);
                label3.Text = npVel.X.ToString() + ";" + npVel.X.ToString();
                VisualUpdate();
                graphics.DrawLine(pen, npPos, mousePos);
                pictureBox1.Refresh();
                Application.DoEvents();
                Thread.Sleep(10);
            }
            textBox2.Visible = true;
            textBox3.Visible = true;
            comboBox1.Visible = true;
            button5.Visible = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e) { pictureBoxClicks++; }

        //upedate mousePos when the mouse is moved
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e) { mousePos = e.Location; }

        //create nem planet
        private void button5_Click(object sender, EventArgs e) {

            int mass = 0;
            try {
                mass = int.Parse(textBox2.Text);
                if (mass < 1) throw new Exception("this isn't positive");
            } catch {
                MessageBox.Show("mass must be a positive int");
                return;
            }

            int size = 0;
            try {
                size = int.Parse(textBox3.Text);
                if (size < 1) throw new Exception("this isn't positive");
            } catch {
                MessageBox.Show("size must be a positive int");
                return;
            }

            Color color = Color.Red;
            try {
                if (comboBox1.SelectedItem == null) throw new Exception("you must select a color");
                KnownColor knownColor = (KnownColor)comboBox1.SelectedItem;
                color = Color.FromKnownColor(knownColor);
            } catch {
                MessageBox.Show("you must select a color");
                return;
            }
            
            planets.Add(new Planet(new Vector(npPos), new Vector(npVel), mass, size, new SolidBrush(color)));

            textBox2.Visible = false;
            textBox3.Visible = false;
            comboBox1.Visible = false;
            button5.Visible = false;
            label2.Text = "";
            label3.Text = "";

            VisualUpdate();
        }

        private static Point addPoints(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);
        public static Point subtractPoints(Point a, Point b) => new Point(a.X - b.X, a.Y - b.Y);
        public static Point dividePoint(Point a, int d) => new Point(a.X / d, a.Y / d);
        public static Point multiplyPoint(Point a, int d) => new Point(a.X * d, a.Y * d);
    }
}