using System.Data;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace Gravity {
    public partial class Form1 : Form {
        private bool isRunning = false;
        private bool pictureBoxClicked = true;
        private int steps = 0;
        private List<Planet> planets = new List<Planet>();
        private Bitmap bmp;
        private Graphics graphics;
        private Point mousePos = new Point(0, 0);
        private SolidBrush defaultBrush = new SolidBrush(Color.Red);

        public Form1() {
            InitializeComponent();

            KnownColor[] colors = Enum.GetValues(typeof(KnownColor)).Cast<KnownColor>().ToArray();
            comboBox1.DataSource = colors;

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
                graphics.FillEllipse(p.solidBrush, (int)Math.Round(p.position.x) - p.size / 2, (int)Math.Round(p.position.y) - p.size / 2, p.size, p.size);
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
            pictureBoxClicked = false;
            while (!pictureBoxClicked) {
                VisualUpdate();
                graphics.FillEllipse(defaultBrush, mousePos.X - 10 / 2, mousePos.Y - 10 / 2, 10, 10);
                pictureBox1.Refresh();
                Application.DoEvents();
                Thread.Sleep(10);
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e) {
            if (!pictureBoxClicked) {
                textBox1.Visible = true;
                textBox2.Visible = true;
                textBox3.Visible = true;
                comboBox1.Visible = true;
                button5.Visible = true;
            }
            pictureBoxClicked = true;
        }

        //upedate mousePos when the mouse is moved
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e) {
            if (!pictureBoxClicked) {
                mousePos = e.Location;
                label2.Text = mousePos.X.ToString() + ";" + mousePos.Y.ToString();
            }
        }

        //create nem planet
        private void button5_Click(object sender, EventArgs e) {
            Vector pos = new Vector(mousePos);

            int vx = 0, vy = 0;
            try {
                string[] numbers = textBox1.Text.Split(';');
                if (numbers.Length != 2) throw new Exception("this isnt 2 numbers");
                vx = int.Parse(numbers[0]);
                vy = int.Parse(numbers[1]);
            } catch {
                MessageBox.Show("velocity must be 2 numbers separated with a ';'");
                return;
            }
            Vector vel = new Vector(vx, vy);

            int mass = 0;
            try {
                mass = int.Parse(textBox2.Text);
                if (mass < 1) throw new Exception("this isn't positive");
            } catch {
                MessageBox.Show("mass must be a positive number");
                return;
            }

            int size = 0;
            try {
                size = int.Parse(textBox3.Text);
                if (size < 1) throw new Exception("this isn't positive");
            } catch {
                MessageBox.Show("size must be a positive number");
                return;
            }

            Color color = Color.Red;
            try {
                if (comboBox1.SelectedItem == null) throw new Exception("you must select a color");
                KnownColor knownColor = (KnownColor)comboBox1.SelectedItem;
                color = Color.FromKnownColor(knownColor);
            } catch {
                MessageBox.Show("size must be a positive number");
                return;
            }

            planets.Add(new Planet(pos, vel, mass, size, new SolidBrush(color)));

            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            comboBox1.Visible = false;
            button5.Visible = false;
            label2.Text = "";

            VisualUpdate();
        }
    }
}