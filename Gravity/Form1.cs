using System.Drawing;
using System.Windows.Forms;

namespace Gravity {
    public partial class Form1 : Form {
        bool isRunning = false;
        int steps = 0;
        List<Planet> planets = new List<Planet>();
        Bitmap bmp;
        Graphics graphics;

        public Form1() {
            InitializeComponent();
            planets.Add(new Planet(new Vector(600, 300), new Vector(0, 0), 2000, 70, new SolidBrush(Color.Red)));
            planets.Add(new Planet(new Vector(400, 300), new Vector(1.5, -2.5), 300, 20, new SolidBrush(Color.Blue)));
            bmp = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            graphics = Graphics.FromImage(bmp);
            pictureBox1.Image = bmp;
            VisualUpdate();
        }

        void VisualUpdate() {
            graphics.Clear(Color.Black);
            foreach (Planet p in planets) {
                graphics.FillEllipse(p.solidBrush, (int)Math.Round(p.position.x) - p.size / 2, (int)Math.Round(p.position.y) - p.size / 2, p.size, p.size);
            }
            pictureBox1.Refresh();
            label1.Text = "steps: " + steps.ToString();
        }

        private void button1_Click(object sender, EventArgs e) {
            isRunning = true;
            while (isRunning) {
                foreach (Planet pa in planets) {
                    foreach (Planet pb in planets) {
                        if (pa != pb) {
                            double d = (pa.position - pb.position).Length();
                            pa.velocity += ((pb.position - pa.position) / d) * (pb.mass / (d * d));
                        }
                    }
                }
                foreach (Planet p in planets) p.position += p.velocity;
                steps++;
                VisualUpdate();
                Application.DoEvents();
                Thread.Sleep(20);
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            isRunning = false;
        }
    }
}